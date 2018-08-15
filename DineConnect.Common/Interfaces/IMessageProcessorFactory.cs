using System;

namespace DineConnect.Common
{
    public interface IMessageProcessorFactory : IDisposable
    {
        void RegisterSubcribe(string subcriptionId);
        void RegisterSubcribe(string subcriptionId, string[] topics);        
    }
}
