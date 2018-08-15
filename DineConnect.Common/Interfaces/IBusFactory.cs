using EasyNetQ;
using System;

namespace DineConnect.Common
{
    public interface IBusFactory : IDisposable
    {
        IBus PublisherBus { get; }
        IBus SubcriberBus { get; }
    }
}
