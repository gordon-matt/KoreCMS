using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Kore.IO;
using Kore.Serialization;

namespace Kore
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Serializes the specified System.Object and returns the data.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <returns>Serialized data of specified System.Object as a Base64 encoded String</returns>
        public static string Base64Serialize<T>(this T item)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                var bytes = memoryStream.GetBuffer();
                return string.Concat(bytes.Length, ":", Convert.ToBase64String(bytes, 0, bytes.Length, Base64FormattingOptions.None));
            }
        }

        /// <summary>
        /// Serializes the specified System.Object and returns the data.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <returns>Serialized data of specified System.Object as System.Byte[]</returns>
        public static byte[] BinarySerialize<T>(this T item)
        {
            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                return memoryStream.ToArray();
            }
        }

        /// <summary>
        /// Serializes the specified System.Object and writes the data to the specified file.
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="fileName">The name of the file to save the serialized data to.</param>
        public static void BinarySerialize<T>(this T item, string fileName)
        {
            using (Stream stream = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(stream, item);
                stream.Close();
            }
        }

        public static T ConvertTo<T>(this object source)
        {
            return (T)Convert.ChangeType(source, typeof(T));
        }

        public static object ConvertTo(this object source, Type type)
        {
            if (type == typeof(Guid))
            {
                return new Guid(source.ToString());
            }

            return Convert.ChangeType(source, type);
        }

        /// <summary>
        /// Creates a deep clone of the current System.Object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="item">The original object.</param>
        /// <returns>A clone of the original object</returns>
        public static T DeepClone<T>(this T item) where T : ISerializable
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, item);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return (T)binaryFormatter.Deserialize(memoryStream);
            }
        }

        /// <summary>
        /// Determines whether this T is contained in the specified 'IEnumerable of T'
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="enumerable">The 'IEnumerable of T' to check</param>
        /// <returns>true if enumerable contains this item, otherwise false.</returns>
        public static bool In<T>(this T t, IEnumerable<T> enumerable)
        {
            foreach (T item in enumerable)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        /// <summary>
        /// Determines whether this T is contained in the specified values
        /// </summary>
        /// <typeparam name="T">This System.Object's type</typeparam>
        /// <param name="t">This item</param>
        /// <param name="items">The values to compare</param>
        /// <returns>true if values contains this item, otherwise false.</returns>
        public static bool In<T>(this T t, params T[] items)
        {
            foreach (T item in items)
            {
                if (item.Equals(t))
                { return true; }
            }
            return false;
        }

        public static bool IsDefault<T>(this T item)
        {
            return EqualityComparer<T>.Default.Equals(item, default(T));
        }

        public static bool GenericEquals<T>(this T item, T other)
        {
            return EqualityComparer<T>.Default.Equals(item, other);
        }

        public static string SharpSerialize<T>(this T item)
        {
            var sharpSettings = new SharpSerializerXmlSettings
            {
                IncludeAssemblyVersionInTypeName = false,
                IncludeCultureInTypeName = false,
                IncludePublicKeyTokenInTypeName = false
            };

            using (var stream = new MemoryStream())
            {
                new SharpSerializer(sharpSettings).Serialize(item, stream);
                stream.Position = 0;

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// <para>Serializes the specified System.Object and writes the XML document</para>
        /// <para>to the specified file.</para>
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="fileName">The file to which you want to write.</param>
        /// <param name="removeNamespaces">
        ///     <para>Specify whether to remove xml namespaces.</para>para>
        ///     <para>If your object has any XmlInclude attributes, then set this to false</para>
        /// </param>
        /// <returns>true if successful, otherwise false.</returns>
        public static bool XmlSerialize<T>(this T item, string fileName, bool removeNamespaces = true, bool omitXmlDeclaration = true)
        {
            object locker = new object();

            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);

            var xmlSerializer = new XmlSerializer(item.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = omitXmlDeclaration
            };

            lock (locker)
            {
                using (var writer = XmlWriter.Create(fileName, settings))
                {
                    if (removeNamespaces)
                    {
                        xmlSerializer.Serialize(writer, item, xmlns);
                    }
                    else { xmlSerializer.Serialize(writer, item); }

                    writer.Close();
                }
            }

            return true;
        }

        /// <summary>
        /// Serializes the specified System.Object and returns the serialized XML
        /// </summary>
        /// <typeparam name="T">This item's type</typeparam>
        /// <param name="item">This item</param>
        /// <param name="removeNamespaces">
        ///     <para>Specify whether to remove xml namespaces.</para>para>
        ///     <para>If your object has any XmlInclude attributes, then set this to false</para>
        /// </param>
        /// <param name="itemType"></param>
        /// <returns>Serialized XML for specified System.Object</returns>
        public static string XmlSerialize<T>(this T item, bool removeNamespaces = true, bool omitXmlDeclaration = true, Encoding encoding = null)
        {
            object locker = new object();

            var xmlns = new XmlSerializerNamespaces();
            xmlns.Add(string.Empty, string.Empty);

            var xmlSerializer = new XmlSerializer(item.GetType());

            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = omitXmlDeclaration
            };

            lock (locker)
            {
                var stringBuilder = new StringBuilder();
                using (var stringWriter = new CustomEncodingStringWriter(encoding, stringBuilder))
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        if (removeNamespaces)
                        {
                            xmlSerializer.Serialize(xmlWriter, item, xmlns);
                        }
                        else { xmlSerializer.Serialize(xmlWriter, item); }

                        return stringBuilder.ToString();
                    }
                }
            }
        }

        #region Compute Hash

        public static string ComputeMD5Hash(this object instance)
        {
            return ComputeHash(instance, new MD5CryptoServiceProvider());
        }

        public static string ComputeSHA1Hash(this object instance)
        {
            return ComputeHash(instance, new SHA1CryptoServiceProvider());
        }

        private static string ComputeHash<T>(object instance, T cryptoServiceProvider) where T : HashAlgorithm, new()
        {
            var serializer = new DataContractSerializer(instance.GetType());
            using (var memoryStream = new MemoryStream())
            {
                serializer.WriteObject(memoryStream, instance);
                cryptoServiceProvider.ComputeHash(memoryStream.ToArray());
                return Convert.ToBase64String(cryptoServiceProvider.Hash);
            }
        }

        #endregion Compute Hash
    }
}