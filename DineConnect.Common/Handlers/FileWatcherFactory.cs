using NLog;
using System;
using System.IO;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public class FileWatcherFactory : IFileWatcherFactory
    {
        private readonly Logger logger = LogManager.GetLogger("dbLoggerInfo");

        private readonly SerialDisposable configWatcherDisposable = new SerialDisposable();
        private readonly IObservableFileSystemWatcher observableFileSystemWatcher;        
        private readonly IProcessingHandler<FileInfo> fileProcessingHandler;
        private IRxSchedulerService rxSchedulerService;

        public FileWatcherFactory(IObservableFileSystemWatcher observableFileSystemWatcher, IRxSchedulerService rxSchedulerService, IProcessingHandler<FileInfo> fileProcessingHandler)
        {
            this.fileProcessingHandler = fileProcessingHandler;
            this.observableFileSystemWatcher = observableFileSystemWatcher;
            this.rxSchedulerService = rxSchedulerService;
            this.fileProcessingHandler = fileProcessingHandler;
        }

        public void StartWatchFolder(FileInfo configFileInfo, params string[] topicMessages)
        {
            if (configFileInfo == null)
            {
                throw new ArgumentNullException(nameof(configFileInfo));
            }

            if (!IsFolder(configFileInfo.FullName))
            {
                throw new ArgumentException("The configuration folder is not a real folder");
            }

            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = configFileInfo.FullName,
                NotifyFilter =
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.FileName |
                NotifyFilters.DirectoryName,
                Filter = configFileInfo.Extension,
                EnableRaisingEvents = true,
            };
            
            observableFileSystemWatcher.SetFile(watcher);

            configWatcherDisposable.Disposable = observableFileSystemWatcher.Changed.SubscribeOn(
                rxSchedulerService.TaskPool).Throttle(TimeSpan.FromMilliseconds(500)).Subscribe(
                    async x =>
                    {
                        await this.fileProcessingHandler.ProcessingMessageAsync(configFileInfo, topicMessages);
                    },
                    ex =>
                    {
                        logger.Info($"Error encountered attempting to read new config data from config file {configFileInfo.Name}");
                    },
                    () =>
                    {
                        logger.Info($"Finish processing file {configFileInfo.Name}");
                    });
        }

        private static bool IsFolder(string fullPath)
        {
            return File.GetAttributes(fullPath).HasFlag(FileAttributes.Directory);
        }
        
        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    configWatcherDisposable?.Dispose();
                    observableFileSystemWatcher?.Dispose();
                    rxSchedulerService = null;
                }
                
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {            
            Dispose(true);            
            GC.SuppressFinalize(this);
        }

        public Task WatchFolderAsync(FileInfo fileInfo)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
