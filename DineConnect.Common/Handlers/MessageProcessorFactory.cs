using System;
using NLog;

namespace DineConnect.Common
{
    public class MessageProcessorFactory : IMessageProcessorFactory
    {
        private readonly Logger logger = LogManager.GetLogger("dbLoggerInfo");
        private readonly IProcessingHandler<ProcessingMessage> messageProcessingHandler;
        private readonly IBusFactory busFactory;

        public MessageProcessorFactory(IBusFactory busFactory, IProcessingHandler<ProcessingMessage> messageProcessingHandler)
        {
            this.messageProcessingHandler = messageProcessingHandler;
            this.busFactory = busFactory;
        }

        public void RegisterSubcribe(string subcriptionId)
        {
            if (string.IsNullOrEmpty(subcriptionId))
            {
                throw new ArgumentNullException(nameof(subcriptionId));
            }

            this.busFactory.SubcriberBus.SubscribeAsync<ProcessingMessage>(subcriptionId, x => messageProcessingHandler.ProcessingMessageAsync(x));
        }

        public void RegisterSubcribe(string subcriptionId, string[] topics)
        {
            if (string.IsNullOrEmpty(subcriptionId))
            {
                throw new ArgumentNullException(nameof(subcriptionId));
            }

            if (topics == null || topics.Length == 0)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            foreach (var topic in topics)
            {
                this.busFactory.SubcriberBus.SubscribeAsync<ProcessingMessage>(subcriptionId, x => messageProcessingHandler.ProcessingMessageAsync(x), t => t.WithTopic(topic));
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    busFactory?.Dispose();
                }
                
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {            
            Dispose(true);            
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
