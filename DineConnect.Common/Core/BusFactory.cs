using EasyNetQ;
using System;

namespace DineConnect.Common
{
    public class BusFactory : IBusFactory 
    {
        private readonly string connectionString;
        private readonly IBus publisherBus;
        private readonly IBus subcriberBus;

        public BusFactory(string connectionString)
        {
            this.connectionString = connectionString;
            publisherBus = RabbitHutch.CreateBus(RabbitMqConfiguration.Instance.GetConnectionString());
            subcriberBus = RabbitHutch.CreateBus(RabbitMqConfiguration.Instance.GetConnectionString());
        }

        public IBus PublisherBus => publisherBus;

        public IBus SubcriberBus => subcriberBus;

        #region IDisposable Support
        private bool disposedValue = false; 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    publisherBus?.SafeDispose();
                    subcriberBus?.SafeDispose();
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
