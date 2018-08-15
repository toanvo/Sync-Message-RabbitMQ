using EasyNetQ;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public class ServerSendContentToClientHandler : BaseSendContentFileHandler, ISendContentFileToQueueHandler
    {
        public ServerSendContentToClientHandler(IBus bus) : base(bus)
        {
        }

        public async Task SendContentFileToQueueAsync(FileInfo fileInfo, string content, params string[] topics)
        {
            if (topics == null)
            {
                throw new ArgumentNullException(nameof(topics));
            }

            using (var releaser = await asyncLock.LockAsync())
            {
                await SendToQueueAsync(fileInfo, content, topics);
            }
        }        
    }
}
