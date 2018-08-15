using EasyNetQ;

namespace DineConnect.Common
{
    public enum QueueType
    {
        Client,
        Server
    }

    public class QueueWatcherFactory
    {
        private readonly IFileWatcherFactory fileWatcherFactory;
        private readonly IMessageProcessorFactory messageProcessorFactory;

        public QueueWatcherFactory(IFileWatcherFactory fileWatcherFactory, IMessageProcessorFactory messageProcessorFactory)
        {
            this.fileWatcherFactory = fileWatcherFactory;
            this.messageProcessorFactory = messageProcessorFactory;
        }

        public QueueWatcher GetQueueWatcher(QueueType queueType)
        {
            var messageProcessingHandler = GetMessageProcessingHandler(queueType);
            return new QueueWatcher(messageProcessingHandler);
        }

        private IMessageService GetMessageProcessingHandler(QueueType queueType)
        {
            if (queueType == QueueType.Client)
            {
                return new ClientMesssageService(fileWatcherFactory, messageProcessorFactory);
            }

            return new ServerMesssageService(fileWatcherFactory, messageProcessorFactory);
        }
    }
}
