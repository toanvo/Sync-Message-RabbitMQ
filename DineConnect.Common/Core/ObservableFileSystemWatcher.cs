using System;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace DineConnect.Common
{
    public class ObservableFileSystemWatcher : IObservableFileSystemWatcher
    {
        private FileSystemWatcher fileWatcher;

        public void SetFile(FileSystemWatcher watcher)
        {
            fileWatcher = watcher;

            Changed = Observable
                .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => fileWatcher.Changed += h, h => fileWatcher.Changed -= h)
                .Select(x => x.EventArgs);

            Renamed = Observable
                .FromEventPattern<RenamedEventHandler, RenamedEventArgs>(h => fileWatcher.Renamed += h, h => fileWatcher.Renamed -= h)
                .Select(x => x.EventArgs);

            Deleted = Observable
                .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => fileWatcher.Deleted += h, h => fileWatcher.Deleted -= h)
                .Select(x => x.EventArgs);

            Errors = Observable
                .FromEventPattern<ErrorEventHandler, ErrorEventArgs>(h => fileWatcher.Error += h, h => fileWatcher.Error -= h)
                .Select(x => x.EventArgs);

            Created = Observable
                .FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(h => fileWatcher.Created += h, h => fileWatcher.Created -= h)
                .Select(x => x.EventArgs);

            All = Changed.Merge(Renamed).Merge(Deleted).Merge(Created);
            fileWatcher.EnableRaisingEvents = true;
        }

        public void Dispose()
        {
            fileWatcher.EnableRaisingEvents = false;
            fileWatcher.Dispose();
        }

        public IObservable<FileSystemEventArgs> Changed { get; private set; }

        public IObservable<RenamedEventArgs> Renamed { get; private set; }

        public IObservable<FileSystemEventArgs> Deleted { get; private set; }

        public IObservable<ErrorEventArgs> Errors { get; private set; }

        public IObservable<FileSystemEventArgs> Created { get; private set; }

        public IObservable<FileSystemEventArgs> All { get; private set; }
    }
}
