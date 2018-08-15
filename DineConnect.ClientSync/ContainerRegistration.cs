using DineConnect.Common;
using System.IO;
using DineConnect.Common.Handlers;

namespace DineConnect.ClientSync
{
    public class ContainerRegistration
    {
        private readonly IBusFactory busFactory;

        public ContainerRegistration(IBusFactory busFactory)
        {
            this.busFactory = busFactory;
        }
        
        public QueueWatcher GetClientQueueWatcher()
        {
            var configuration = RabbitMqConfiguration.Instance;            
            var fileWatcherFactory = GetFileInfoProcessingHandler(configuration, busFactory);

            var messageProcessorFactory = GetMessageProcessorFactory(configuration, busFactory);

            var queueFactory = new QueueWatcherFactory(fileWatcherFactory, messageProcessorFactory);
            return queueFactory.GetQueueWatcher(QueueType.Client);
        }

        private IFileWatcherFactory GetFileInfoProcessingHandler(RabbitMqConfiguration configuration, IBusFactory busFactory)
        {
            var observableFileWatcher = new ObservableFileSystemWatcher();
            var rxSchedulerService = new RxSchedulerService();
            var serverSendContentFile = new ClientSendContentFileToServerHandler(busFactory.PublisherBus);

            IProcessingHandler<FileInfo> processorHandler = new ClientFileInfoProcessingHandler(configuration, serverSendContentFile);
            return new FileWatcherFactory(observableFileWatcher, rxSchedulerService, processorHandler);
        }

        private IMessageProcessorFactory GetMessageProcessorFactory(RabbitMqConfiguration configuration, IBusFactory busFactory)
        {
            IProcessingHandler<ProcessingMessage> messageProcessorHandler = new ClientMessageProcessingHandler(configuration);
            return new MessageProcessorFactory(busFactory, messageProcessorHandler);
        }
    }
}
