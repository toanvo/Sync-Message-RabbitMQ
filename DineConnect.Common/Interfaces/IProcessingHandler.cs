using System.Threading.Tasks;

namespace DineConnect.Common
{
    public interface IProcessingHandler<T>
    {
        Task ProcessingMessageAsync(T message, params string[] topicMessages);
    }
}
