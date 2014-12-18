using System;

namespace Kore.Web.IO.FileSystems.LockFile
{
    public interface ILockFile : IDisposable
    {
        void Release();
    }
}