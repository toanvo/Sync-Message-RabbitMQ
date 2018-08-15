using EasyNetQ;
using NLog;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public abstract class BaseSendContentFileHandler
    {
        protected static Logger logger = LogManager.GetLogger("dbLoggerInfo");
        protected readonly AsyncLock asyncLock = new AsyncLock();

        protected IBus bus;

        public BaseSendContentFileHandler(IBus bus)
        {
            this.bus = bus;
        }
        
        protected async Task SendToQueueAsync(FileInfo fileInfo, string content, params string[] topicMessages)
        {            
            if (topicMessages != null && topicMessages.Length > 0)
            {
                foreach (var topicMessage in topicMessages)
                {
                    var message = new ProcessingMessage()
                    {
                        Id = Guid.NewGuid(),
                        Name = fileInfo.Name,
                        ExtensionFileType = fileInfo.Extension,
                        ContentFile = content
                    };

                    await bus.PublishAsync<ProcessingMessage>(message, topicMessage);
                }
            }
            else
            {
                var message = new ProcessingMessage()
                {
                    Id = Guid.NewGuid(),
                    Name = fileInfo.Name,
                    ExtensionFileType = fileInfo.Extension,
                    ContentFile = content
                };

                await bus.PublishAsync<ProcessingMessage>(message);
            }

            logger.Info($"Send queue completed.");
        }
    }
}
