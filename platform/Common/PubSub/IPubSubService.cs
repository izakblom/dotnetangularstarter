using Google.Cloud.PubSub.V1;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.PubSub
{
    /// <summary>
    /// This is how a running instance of the pubsub service will look.
    /// </summary>
    public interface IPubSubService
    {
        /// <summary>
        /// Sends a message on a topic over pubsub
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="topic"></param>
        /// <param name="metadata"></param>
        /// <returns></returns>
        Task<bool> Send<T>(T msg, string topic, Dictionary<string, string> metadata = null);

        /// <summary>
        /// Creates a topic that will receive messages if it does not exist.
        /// </summary>
        /// <param name="topicName"></param>
        /// <returns></returns>
        Task CreateTopic(string topicName);

        /// <summary>
        /// Creates a subscription if it does not exist.
        /// </summary>
        /// <param name="subscriptionStr">The subscription to create (subscriber)</param>
        /// <param name="topicStr">The topic to which the subscription belong (Subscriber will listen for messages on this topic)</param>
        /// <returns></returns>
        Task<bool> CreateSubscription(string subscriptionStr, string topicStr);

        /// <summary>
        /// Listens for and selects one or more message from the subscription
        /// </summary>
        /// <param name="maxMessages">Number of messages to retrieve at a time</param>
        /// <returns></returns>
        Task<List<ReceivedMessage>> GetMessagesFromSubscription(int maxMessages = 4);

        /// <summary>
        /// Every message should be confirmed as received as soon as it is processed. This process removes the message from the queue and assumes that the message was handled.
        /// </summary>
        /// <param name="ackIds"></param>
        void AcknowledgeMessagesReceived(List<String> ackIds);

    }
}
