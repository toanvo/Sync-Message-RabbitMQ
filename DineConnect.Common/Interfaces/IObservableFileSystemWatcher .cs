﻿using System;
using System.IO;

namespace DineConnect.Common
{
    public interface IObservableFileSystemWatcher : IDisposable
    {
        void SetFile(FileSystemWatcher watcher);

        IObservable<FileSystemEventArgs> Changed { get; }

        IObservable<RenamedEventArgs> Renamed { get; }

        IObservable<FileSystemEventArgs> Deleted { get; }

        IObservable<ErrorEventArgs> Errors { get; }

        IObservable<FileSystemEventArgs> Created { get; }

        IObservable<FileSystemEventArgs> All { get; }
    }
}
