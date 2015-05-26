﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;
using System.Web.Compilation;
using System.Web.Hosting;
using Castle.Core.Logging;
using Kore.Logging;
using Kore.Web.Plugins;

//Contributor: Umbraco (http://www.umbraco.com). Thanks a lot!
//SEE THIS POST for full details of what this does - http://shazwazza.com/post/Developing-a-plugin-framework-in-ASPNET-with-medium-trust.aspx

[assembly: PreApplicationStartMethod(typeof(PluginManager), "Initialize")]

namespace Kore.Web.Plugins
{
    /// <summary>
    /// Sets the application up for the plugin referencing
    /// </summary>
    public static class PluginManager
    {
        #region Const

        private const string InstalledPluginsFilePath = "~/App_Data/InstalledPlugins.txt";
        private const string PluginsPath = "~/Plugins";
        private const string ShadowCopyPath = "~/Plugins/bin";

        #endregion Const

        #region Fields

        private static readonly ReaderWriterLockSlim Locker = new ReaderWriterLockSlim();
        private static DirectoryInfo _shadowCopyFolder;
        private static bool _clearShadowDirectoryOnStartup;

        #endregion Fields

        private static ILogger Logger;

        static PluginManager()
        {
            Logger = LoggingUtilities.Resolve();
        }

        #region Methods

        /// <summary>
        /// Returns a collection of all referenced plugin assemblies that have been shadow copied
        /// </summary>
        public static IEnumerable<PluginDescriptor> ReferencedPlugins { get; set; }

        /// <summary>
        /// Returns a collection of all plugin which are not compatible with the current version
        /// </summary>
        public static IEnumerable<string> IncompatiblePlugins { get; set; }

        /// <summary>
        /// Initialize
        /// </summary>
        public static void Initialize()
        {
            Locker.EnterWriteLock();

            // TODO: Add verbose exception handling / raising here since this is happening on app startup and could
            // prevent app from starting altogether
            var pluginFolder = new DirectoryInfo(HostingEnvironment.MapPath(PluginsPath));
            _shadowCopyFolder = new DirectoryInfo(HostingEnvironment.MapPath(ShadowCopyPath));

            var referencedPlugins = new List<PluginDescriptor>();
            var incompatiblePlugins = new List<string>();

            _clearShadowDirectoryOnStartup = !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]) &&
               Convert.ToBoolean(ConfigurationManager.AppSettings["ClearPluginsShadowDirectoryOnStartup"]);

            try
            {
                var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());

                Debug.WriteLine("Creating shadow copy folder and querying for dlls");
                //ensure folders are created
                Directory.CreateDirectory(pluginFolder.FullName);
                Directory.CreateDirectory(_shadowCopyFolder.FullName);

                //get list of all files in bin
                var binFiles = _shadowCopyFolder.GetFiles("*", SearchOption.AllDirectories);
                if (_clearShadowDirectoryOnStartup)
                {
                    //clear out shadow copied plugins
                    foreach (var f in binFiles)
                    {
                        Debug.WriteLine("Deleting " + f.Name);
                        try
                        {
                            File.Delete(f.FullName);
                        }
                        catch (Exception x)
                        {
                            Logger.Error("Error deleting file " + f.Name, x);
                            Debug.WriteLine("Error deleting file " + f.Name + ". Exception: " + x);
                        }
                    }
                }

                //load description files
                foreach (var dfd in GetDescriptionFilesAndDescriptors(pluginFolder))
                {
                    var descriptionFile = dfd.Key;
                    var pluginDescriptor = dfd.Value;

                    //ensure that version of plugin is valid
                    if (!pluginDescriptor.SupportedVersions.Contains(KoreVersion.CurrentVersion, StringComparer.InvariantCultureIgnoreCase))
                    {
                        incompatiblePlugins.Add(pluginDescriptor.SystemName);
                        continue;
                    }

                    //some validation
                    if (string.IsNullOrWhiteSpace(pluginDescriptor.SystemName))
                    {
                        throw new Exception(string.Format("A plugin '{0}' has no system name. Try assigning the plugin a unique name and recompiling.", descriptionFile.FullName));
                    }
                    if (referencedPlugins.Contains(pluginDescriptor))
                    {
                        throw new Exception(string.Format("A plugin with '{0}' system name is already defined", pluginDescriptor.SystemName));
                    }

                    //set 'Installed' property
                    pluginDescriptor.Installed = installedPluginSystemNames
                        .FirstOrDefault(x => x.Equals(pluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase)) != null;

                    try
                    {
                        if (descriptionFile.Directory == null)
                        {
                            throw new Exception(string.Format("Directory cannot be resolved for '{0}' description file", descriptionFile.Name));
                        }

                        //get list of all DLLs in plugins (not in bin!)
                        var pluginFiles = descriptionFile.Directory.GetFiles("*.dll", SearchOption.AllDirectories)
                            //just make sure we're not registering shadow copied plugins
                            .Where(x => !binFiles.Select(q => q.FullName).Contains(x.FullName))
                            .Where(x => IsPackagePluginFolder(x.Directory))
                            .ToList();

                        //other plugin description info
                        var mainPluginFile = pluginFiles
                            .FirstOrDefault(x => x.Name.Equals(pluginDescriptor.PluginFileName, StringComparison.InvariantCultureIgnoreCase));
                        pluginDescriptor.OriginalAssemblyFile = mainPluginFile;

                        //shadow copy main plugin file
                        pluginDescriptor.ReferencedAssembly = PerformFileDeploy(mainPluginFile);

                        //load all other referenced assemblies now
                        foreach (var plugin in pluginFiles
                            .Where(x => !x.Name.Equals(mainPluginFile.Name, StringComparison.InvariantCultureIgnoreCase))
                            .Where(x => !IsAlreadyLoaded(x)))
                        {
                            PerformFileDeploy(plugin);
                        }

                        //init plugin type (only one plugin per assembly is allowed)
                        pluginDescriptor.PluginType = pluginDescriptor.ReferencedAssembly.GetTypes()
                            .Where(t =>
                                typeof(IPlugin).IsAssignableFrom(t) &&
                                !t.IsInterface &&
                                t.IsClass &&
                                !t.IsAbstract)
                            .FirstOrDefault();

                        referencedPlugins.Add(pluginDescriptor);
                    }
                    catch (ReflectionTypeLoadException x)
                    {
                        var msg = string.Empty;
                        foreach (var e in x.LoaderExceptions)
                        {
                            msg += e.Message + System.Environment.NewLine;
                        }

                        var fail = new Exception(msg, x);
                        Logger.Error(fail.Message, fail);
                        Debug.WriteLine(fail.Message, fail);

                        throw fail;
                    }
                }
            }
            catch (Exception x)
            {
                var msg = string.Empty;
                for (var e = x; e != null; e = e.InnerException)
                {
                    msg += e.Message + System.Environment.NewLine;
                }

                var fail = new Exception(msg, x);
                Logger.Error(fail.Message, fail);
                Debug.WriteLine(fail.Message, fail);

                throw fail;
            }

            ReferencedPlugins = referencedPlugins;
            IncompatiblePlugins = incompatiblePlugins;

            Locker.ExitWriteLock();
        }

        /// <summary>
        /// Mark plugin as installed
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsInstalled(string systemName)
        {
            if (String.IsNullOrEmpty(systemName))
            {
                throw new ArgumentNullException("systemName");
            }

            var filePath = HostingEnvironment.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }
            }

            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());

            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;

            if (!alreadyMarkedAsInstalled)
            {
                installedPluginSystemNames.Add(systemName);
            }

            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        /// <param name="systemName">Plugin system name</param>
        public static void MarkPluginAsUninstalled(string systemName)
        {
            if (string.IsNullOrEmpty(systemName))
            {
                throw new ArgumentNullException("systemName");
            }

            var filePath = HostingEnvironment.MapPath(InstalledPluginsFilePath);
            if (!File.Exists(filePath))
            {
                using (File.Create(filePath))
                {
                    //we use 'using' to close the file after it's created
                }
            }

            var installedPluginSystemNames = PluginFileParser.ParseInstalledPluginsFile(GetInstalledPluginsFilePath());

            bool alreadyMarkedAsInstalled = installedPluginSystemNames
                .FirstOrDefault(x => x.Equals(systemName, StringComparison.InvariantCultureIgnoreCase)) != null;

            if (alreadyMarkedAsInstalled)
            {
                installedPluginSystemNames.Remove(systemName);
            }

            PluginFileParser.SaveInstalledPluginsFile(installedPluginSystemNames, filePath);
        }

        /// <summary>
        /// Mark plugin as uninstalled
        /// </summary>
        public static void MarkAllPluginsAsUninstalled()
        {
            var filePath = HostingEnvironment.MapPath(InstalledPluginsFilePath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static bool IsPluginInstalled(string systemName)
        {
            return ReferencedPlugins.Any(x => x.SystemName == systemName && x.Installed);
        }

        #endregion Methods

        #region Utilities

        /// <summary>
        /// Get description files
        /// </summary>
        /// <param name="pluginFolder">Plugin direcotry info</param>
        /// <returns>Original and parsed description files</returns>
        private static IEnumerable<KeyValuePair<FileInfo, PluginDescriptor>> GetDescriptionFilesAndDescriptors(DirectoryInfo pluginFolder)
        {
            if (pluginFolder == null)
            {
                throw new ArgumentNullException("pluginFolder");
            }

            //create list (<file info, parsed plugin descritor>)
            var result = new List<KeyValuePair<FileInfo, PluginDescriptor>>();
            //add display order and path to list
            foreach (var descriptionFile in pluginFolder.GetFiles("Description.txt", SearchOption.AllDirectories))
            {
                if (!IsPackagePluginFolder(descriptionFile.Directory))
                {
                    continue;
                }

                //parse file
                var pluginDescriptor = PluginFileParser.ParsePluginDescriptionFile(descriptionFile.FullName);

                //populate list
                result.Add(new KeyValuePair<FileInfo, PluginDescriptor>(descriptionFile, pluginDescriptor));
            }

            //sort list by display order. NOTE: Lowest DisplayOrder will be first i.e 0 , 1, 1, 1, 5, 10
            //it's required
            result.Sort((firstPair, nextPair) => firstPair.Value.DisplayOrder.CompareTo(nextPair.Value.DisplayOrder));
            return result;
        }

        /// <summary>
        /// Indicates whether assembly file is already loaded
        /// </summary>
        /// <param name="fileInfo">File info</param>
        /// <returns>Result</returns>
        private static bool IsAlreadyLoaded(FileInfo fileInfo)
        {
            //compare full assembly name
            //var fileAssemblyName = AssemblyName.GetAssemblyName(fileInfo.FullName);
            //foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
            //{
            //    if (a.FullName.Equals(fileAssemblyName.FullName, StringComparison.InvariantCultureIgnoreCase))
            //        return true;
            //}
            //return false;

            //do not compare the full assembly name, just filename
            try
            {
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(fileInfo.FullName);
                if (fileNameWithoutExt == null)
                    throw new Exception(string.Format("Cannot get file extnension for {0}", fileInfo.Name));
                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    string assemblyName = a.FullName.Split(new[] { ',' }).FirstOrDefault();
                    if (fileNameWithoutExt.Equals(assemblyName, StringComparison.InvariantCultureIgnoreCase))
                        return true;
                }
            }
            catch (Exception x)
            {
                Logger.Error("Cannot validate whether an assembly is already loaded.", x);
                Debug.WriteLine("Cannot validate whether an assembly is already loaded. " + x);
            }
            return false;
        }

        /// <summary>
        /// Perform file deply
        /// </summary>
        /// <param name="plugin">Plugin file info</param>
        /// <returns>Assembly</returns>
        private static Assembly PerformFileDeploy(FileInfo plugin)
        {
            if (plugin.Directory.Parent == null)
                throw new InvalidOperationException("The plugin directory for the " + plugin.Name +
                    " file exists in a folder outside of the allowed kore folder heirarchy");

            FileInfo shadowCopiedPlugin;

            if (WebHelper.GetTrustLevel() != AspNetHostingPermissionLevel.Unrestricted)
            {
                //all plugins will need to be copied to ~/Plugins/bin/
                //this is aboslutely required because all of this relies on probingPaths being set statically in the web.config

                //we're running in med trust, so copy to custom bin folder
                var shadowCopyPlugFolder = Directory.CreateDirectory(_shadowCopyFolder.FullName);
                shadowCopiedPlugin = InitializeMediumTrust(plugin, shadowCopyPlugFolder);
            }
            else
            {
                var directory = AppDomain.CurrentDomain.DynamicDirectory;
                Debug.WriteLine(plugin.FullName + " to " + directory);
                //were running in full trust so copy to standard dynamic folder
                shadowCopiedPlugin = InitializeFullTrust(plugin, new DirectoryInfo(directory));
            }

            //we can now register the plugin definition
            var shadowCopiedAssembly = Assembly.Load(AssemblyName.GetAssemblyName(shadowCopiedPlugin.FullName));

            //add the reference to the build manager
            Debug.WriteLine("Adding to BuildManager: '{0}'", shadowCopiedAssembly.FullName);

            BuildManager.AddReferencedAssembly(shadowCopiedAssembly);

            return shadowCopiedAssembly;
        }

        /// <summary>
        /// Used to initialize plugins when running in Full Trust
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeFullTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));
            try
            {
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            catch (IOException x)
            {
                string msg = shadowCopiedPlug.FullName + " is locked, attempting to rename.";
                Logger.Error(msg, x);
                Debug.WriteLine(msg);
                //this occurs when the files are locked,
                //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                try
                {
                    var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                    File.Move(shadowCopiedPlug.FullName, oldFile);
                }
                catch (IOException x2)
                {
                    msg = shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin.";
                    Logger.Error(msg, x);
                    throw new IOException(msg, x2);
                }
                //ok, we've made it this far, now retry the shadow copy
                File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
            }
            return shadowCopiedPlug;
        }

        /// <summary>
        /// Used to initialize plugins when running in Medium Trust
        /// </summary>
        /// <param name="plug"></param>
        /// <param name="shadowCopyPlugFolder"></param>
        /// <returns></returns>
        private static FileInfo InitializeMediumTrust(FileInfo plug, DirectoryInfo shadowCopyPlugFolder)
        {
            var shouldCopy = true;
            var shadowCopiedPlug = new FileInfo(Path.Combine(shadowCopyPlugFolder.FullName, plug.Name));

            //check if a shadow copied file already exists and if it does, check if it's updated, if not don't copy
            if (shadowCopiedPlug.Exists)
            {
                //it's better to use LastWriteTimeUTC, but not all file systems have this property
                //maybe it is better to compare file hash?
                var areFilesIdentical = shadowCopiedPlug.CreationTimeUtc.Ticks >= plug.CreationTimeUtc.Ticks;
                if (areFilesIdentical)
                {
                    Debug.WriteLine("Not copying; files appear identical: '{0}'", shadowCopiedPlug.Name);
                    shouldCopy = false;
                }
                else
                {
                    //delete an existing file

                    Debug.WriteLine("New plugin found; Deleting the old file: '{0}'", shadowCopiedPlug.Name);
                    File.Delete(shadowCopiedPlug.FullName);
                }
            }

            if (shouldCopy)
            {
                try
                {
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
                catch (IOException x)
                {
                    string msg = shadowCopiedPlug.FullName + " is locked, attempting to rename.";
                    Logger.Error(msg, x);
                    Debug.WriteLine(msg);
                    //this occurs when the files are locked,
                    //for some reason devenv locks plugin files some times and for another crazy reason you are allowed to rename them
                    //which releases the lock, so that it what we are doing here, once it's renamed, we can re-shadow copy
                    try
                    {
                        var oldFile = shadowCopiedPlug.FullName + Guid.NewGuid().ToString("N") + ".old";
                        File.Move(shadowCopiedPlug.FullName, oldFile);
                    }
                    catch (IOException x2)
                    {
                        msg = shadowCopiedPlug.FullName + " rename failed, cannot initialize plugin.";
                        Logger.Error(msg, x2);
                        throw new IOException(msg, x2);
                    }
                    //ok, we've made it this far, now retry the shadow copy
                    File.Copy(plug.FullName, shadowCopiedPlug.FullName, true);
                }
            }

            return shadowCopiedPlug;
        }

        /// <summary>
        /// Determines if the folder is a bin plugin folder for a package
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        private static bool IsPackagePluginFolder(DirectoryInfo folder)
        {
            if (folder == null) return false;
            if (folder.Parent == null) return false;
            if (!folder.Parent.Name.Equals("Plugins", StringComparison.InvariantCultureIgnoreCase)) return false;
            return true;
        }

        /// <summary>
        /// Gets the full path of InstalledPlugins.txt file
        /// </summary>
        /// <returns></returns>
        private static string GetInstalledPluginsFilePath()
        {
            var filePath = HostingEnvironment.MapPath(InstalledPluginsFilePath);
            return filePath;
        }

        #endregion Utilities
    }
}