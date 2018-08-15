using System.IO;

namespace DineConnect.Common
{
    public class ClientMesssageService : BaseProcessorService, IMessageService
    {
        public ClientMesssageService(IFileWatcherFactory fileWatcherFactory, IMessageProcessorFactory messageProcessorFactory) 
            : base(fileWatcherFactory, messageProcessorFactory)
        {            
        }
        
        public void Start()
        {
            var fileInfo = new FileInfo(RabbitMqConfiguration.Instance.GetFileWatcher());
            var topicConsumers = RabbitMqConfiguration.Instance.GetTopicConsumers();
            var subcriptionId = RabbitMqConfiguration.Instance.GetSubcriptionId();

            logger.Info($"Watching Folder {fileInfo.FullName}.");
            fileWatcherFactory.StartWatchFolder(fileInfo);

            messageProcessorFactory.RegisterSubcribe(subcriptionId, topicConsumers);
        }        

        public void Stop()
        {
            fileWatcherFactory.Dispose();
            messageProcessorFactory.Dispose();
            logger.Info("Stop Processing Message at client side.");
        }              
    }
}
