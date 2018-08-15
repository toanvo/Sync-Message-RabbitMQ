using NLog;
using System;

namespace DineConnect.Common
{
    public class QueueWatcher 
    {
        protected static Logger logger = LogManager.GetLogger("dbLoggerInfo");

        private readonly IMessageService messageService;

        public QueueWatcher(IMessageService messageService)
        {            
            this.messageService = messageService;
        }        

        public bool Start()
        {
            logger.Trace("Client Service Started!");
            messageService.Start();
            return true;
        }

        public bool Stop()
        {
            logger.Trace("Client Service Started!");
            messageService.Stop();
            return true;
        }        
    }
}
