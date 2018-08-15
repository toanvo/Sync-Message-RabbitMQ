using System.IO;

namespace DineConnect.Common
{
    public class ServerMesssageService : BaseProcessorService, IMessageService
    {
        public ServerMesssageService(IFileWatcherFactory fileWatcherFactory, IMessageProcessorFactory messageProcessorFactory) 
            : base(fileWatcherFactory, messageProcessorFactory)
        {
        }

        public void Start()
        {
            var fileInfo = new FileInfo(RabbitMqConfiguration.Instance.GetFileWatcher());
            var topicPublishers = RabbitMqConfiguration.Instance.GetTopicsPublisher();
            var subcriptionId = RabbitMqConfiguration.Instance.GetSubcriptionId();

            logger.Info($"Watching Folder {fileInfo.FullName} at server side.");
            fileWatcherFactory.StartWatchFolder(fileInfo, topicPublishers);
            messageProcessorFactory.RegisterSubcribe(subcriptionId);
        }

        public void Stop()
        {
            fileWatcherFactory.Dispose();
            messageProcessorFactory.Dispose();
            logger.Info("Stop Processing Message at server side.");
        }        
    }
}
