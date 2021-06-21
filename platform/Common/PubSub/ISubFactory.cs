using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.PubSub
{
    /// <summary>
    /// Provides a method to handle messages synchronously (one at a time)
    /// </summary>
    public interface ISubFactory
    {
        Task<string> DoWork(ReceivedMessage message);
    }

    /// <summary>
    /// Provides a method to handle a group of messages asynchronously (more than one at a time)
    /// </summary>
    public interface ISubFactoryGrouped
    {
        Task<string> DoWork(List<ReceivedMessage> messages);
    }
}
