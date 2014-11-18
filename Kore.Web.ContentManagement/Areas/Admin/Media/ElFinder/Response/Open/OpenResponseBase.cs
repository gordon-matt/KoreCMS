using System.Collections.Generic;
using System.Runtime.Serialization;
using ElFinder.DTO;

namespace ElFinder.Response
{
    [DataContract]
    internal class OpenResponseBase
    {
        protected List<DTOBase> _files;

        private static Debug _debug = new Debug();

        private DTOBase _currentWorkingDirectory;

        public OpenResponseBase(DTOBase currentWorkingDirectory)
        {
            _files = new List<DTOBase>();
            _currentWorkingDirectory = currentWorkingDirectory;
        }

        [DataMember(Name = "cwd")]
        public DTOBase CurrentWorkingDirectory { get { return _currentWorkingDirectory; } }

        [DataMember(Name = "debug")]
        public Debug Debug { get { return _debug; } }

        [DataMember(Name = "files")]
        public List<DTOBase> Files { get { return _files; } }

        [DataMember(Name = "options")]
        public Options Options { get; protected set; }

        public void AddResponse(DTOBase item)
        {
            _files.Add(item);
        }
    }
}