using EasyNetQ;
using NLog;
using System;

namespace DineConnect.Common
{
    public class BaseProcessorService 
    {
        protected static Logger logger = LogManager.GetLogger("dbLoggerInfo");

        protected readonly IFileWatcherFactory fileWatcherFactory;
        protected readonly IMessageProcessorFactory messageProcessorFactory;

        public BaseProcessorService(IFileWatcherFactory fileWatcherFactory, IMessageProcessorFactory messageProcessorFactory) 
        {
            this.messageProcessorFactory = messageProcessorFactory;
            this.fileWatcherFactory = fileWatcherFactory;            
        }
    }
}
