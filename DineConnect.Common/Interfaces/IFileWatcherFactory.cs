using System;
using System.IO;

namespace DineConnect.Common
{
    public interface IFileWatcherFactory : IDisposable
    {
        void StartWatchFolder(FileInfo fileWatcher, params string[] topicMessages);
    }
}
