using EasyNetQ;
using System.IO;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public class ClientSendContentFileToServerHandler : BaseSendContentFileHandler, ISendContentFileToQueueHandler
    {
        public ClientSendContentFileToServerHandler(IBus bus) : base(bus)
        {
        }

        public async Task SendContentFileToQueueAsync(FileInfo fileInfo, string content, params string[] topics)
        {
            using (var releaser = await asyncLock.LockAsync())
            {
                await SendToQueueAsync(fileInfo, content);
            }
        }
    }
}
