using System.IO;
using System.Threading.Tasks;

namespace DineConnect.Common
{
    public interface ISendContentFileToQueueHandler
    {
        Task SendContentFileToQueueAsync(FileInfo fileInfo, string content, params string[] topics);
    }
}
