using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Common.PubSub
{
    public class EventListener : IEventListener
    {
        private IConfiguration _configuration { get; set; }
        private IPubSubService _pubSubService;
        private ISubFactory _subFactory;
        private ISubFactoryGrouped _groupedSubFactory;
        private int _delayBetweenPullsMs { get; set; } = 100;

        public EventListener(
            IConfiguration configuration,
            IPubSubService pubSubService,
            ISubFactory subFactory,
            ISubFactoryGrouped groupedSubFactor)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _pubSubService = pubSubService ?? throw new ArgumentNullException(nameof(pubSubService));
            _subFactory = subFactory ?? throw new ArgumentNullException(nameof(subFactory));
            _groupedSubFactory = groupedSubFactor ?? throw new ArgumentNullException(nameof(groupedSubFactor));
        }
        /// <summary>
        /// Event is fired when all event tasks complete.
        /// </summary>
        /// <param name="tasks"></param>
        private void ConcurrentTasksCompleted(Task<string>[] tasks)
        {
            Console.WriteLine("================================================");
            Console.WriteLine("Pub Sub listener completed");
            Console.WriteLine("================================================");
            int failures = tasks.Where(t => t.Exception != null).Count();
            Console.WriteLine("{0} successes and {1} failures", tasks.Length - failures, failures);
            Console.WriteLine("================================================");
            tasks.Where(t => t.Exception != null).ToList().ForEach((t) => {
                Console.WriteLine(t.Exception.InnerException.Message);
            });


        }

        public async Task Configure(string topic, string subscription, int delayBetweenPullsMs = 100)
        {
            _delayBetweenPullsMs = delayBetweenPullsMs;

            await _pubSubService.CreateTopic(topic);
            await _pubSubService.CreateSubscription(subscription, topic);
        }

        public async Task DoWorkAsync(int maxMessages = 4, bool groupMessages = false)
        {
            while (true)
            {
                try
                {
                    var resp = await _pubSubService.GetMessagesFromSubscription(maxMessages);

                    var aknIds = new List<string>();
                    var tasks = new List<Task<string>>();

                    resp.ForEach(m =>
                    {
                        if (!groupMessages) tasks.Add(_subFactory.DoWork(m));

                        aknIds.Add(m.AckId);
                    });

                    if (groupMessages && resp.Count > 0)
                        tasks.Add(_groupedSubFactory.DoWork(resp));

                    if (aknIds.Count > 0)
                        _pubSubService.AcknowledgeMessagesReceived(aknIds);

                    //Execute the tasks and continue when all either fail or complete..
                    if (tasks.Count > 0)
                        Task.Factory.ContinueWhenAll(tasks.ToArray(), ConcurrentTasksCompleted).Wait();

                }
                catch (Exception ex)
                {
                    //will only throw when the subscription/topic is not found.
                    //sleep
                    //Console.WriteLine(ex.ToString());
                }
                finally
                {
                    Thread.Sleep(_delayBetweenPullsMs);
                }

            }
        }
    }
}
