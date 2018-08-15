using DineConnect.Common;
using System.IO;
using DineConnect.Common.Handlers;

namespace DineConnect.ServerSync
{
    public class ContainerRegistration
    {
        private readonly IBusFactory busFactory;

        public ContainerRegistration(IBusFactory busFactory)
        {
            this.busFactory = busFactory;
        }

        public QueueWatcher GetServerQueueWatcher()
        {
            var configuration = RabbitMqConfiguration.Instance;            
            var fileWatcherFactory = GetFileInfoProcessingHandler(configuration, busFactory);

            var messageProcessorFactory = GetMessageProcessorFactory(configuration, busFactory);

            var queueFactory = new QueueWatcherFactory(fileWatcherFactory, messageProcessorFactory);
            return queueFactory.GetQueueWatcher(QueueType.Server);
        }

        private IFileWatcherFactory GetFileInfoProcessingHandler(RabbitMqConfiguration configuration, IBusFactory busFactory)
        {
            var observableFileWatcher = new ObservableFileSystemWatcher();
            var rxSchedulerService = new RxSchedulerService();
            var serverSendContentFile = new ServerSendContentToClientHandler(busFactory.PublisherBus);

            IProcessingHandler<FileInfo> processorHandler = new ServerFileInfoProcessingHandler(configuration, serverSendContentFile);
            return new FileWatcherFactory(observableFileWatcher, rxSchedulerService, processorHandler);
        }

        private IMessageProcessorFactory GetMessageProcessorFactory(RabbitMqConfiguration configuration, IBusFactory busFactory)
        {
            IProcessingHandler<ProcessingMessage> messageProcessorHandler = new ServerMessageProcessingHandler(configuration);
            return new MessageProcessorFactory(busFactory, messageProcessorHandler);
        }
    }
}
