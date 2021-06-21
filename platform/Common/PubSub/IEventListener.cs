using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Common.PubSub
{
    /// <summary>
    /// Provides a template for a specific queue listener
    /// </summary>
    public interface IEventListener
    {
        /// <summary>
        /// Configure the topic and subscription
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="subscription"></param>
        /// <param name="delayBetweenPullsMs"></param>
        /// <returns></returns>
        Task Configure(string topic, string subscription, int delayBetweenPullsMs = 100);

        /// <summary>
        /// Listen and process incoming messages
        /// </summary>
        /// <param name="maxMessages"></param>
        /// <param name="groupMessages"></param>
        /// <returns></returns>
        Task DoWorkAsync(int maxMessages = 4, bool groupMessages = false);
    }
}
