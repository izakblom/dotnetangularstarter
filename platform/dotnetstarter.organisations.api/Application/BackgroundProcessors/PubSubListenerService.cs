using Common.PubSub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.BackgroundProcessors
{
    public class PubSubListenerService: IHostedService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;
        private readonly IHostingEnvironment _env;

        public PubSubListenerService(
            IServiceProvider services,
            IConfiguration configuration,
            IHostingEnvironment env)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _env = env ?? throw new ArgumentNullException(nameof(env));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWorkAsync();
            return;
        }

        private async Task DoWorkAsync()
        {
            List<Task> tasks = new List<Task>();

            // we have multiple subscriptions in this service.
            using (var scope = _services.CreateScope())
            {
                // Example single processor
                var singleMessageProcessor = new EventListener(
                                    scope.ServiceProvider.GetRequiredService<IConfiguration>(),
                                    new MessageService(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"], _env.IsDevelopment()),
                                    scope.ServiceProvider.GetRequiredService<ISubFactory>(),
                                    scope.ServiceProvider.GetRequiredService<ISubFactoryGrouped>());

                await singleMessageProcessor.Configure(
                    _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:PubSub:General:Single:Topic"],
                    _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:PubSub:General:Single:Sub"]
                    );

                tasks.Add(Listen(singleMessageProcessor));

                // Example group processor
                var groupMessageProcessor = new EventListener(
                                    scope.ServiceProvider.GetRequiredService<IConfiguration>(),
                                    new MessageService(_configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:GCSettings:project_id"], _env.IsDevelopment()),
                                    scope.ServiceProvider.GetRequiredService<ISubFactory>(),
                                    scope.ServiceProvider.GetRequiredService<ISubFactoryGrouped>());

                await groupMessageProcessor.Configure(
                    _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:PubSub:General:Grouped:Topic"],
                    _configuration[$"{Environment.GetEnvironmentVariable("TENANT")}:PubSub:General:Grouped:Sub"]
                    );

                tasks.Add(Listen(groupMessageProcessor, 5000, true, 1000 * 30));

                // TODO: if you need to fire pu more listeners (listeners for other PUBSUB TOPICS, instantiate them here and add to task queue. 

                await Task.WhenAll(tasks);

            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Listen for messages from the pub sub topic
        /// </summary>
        /// <param name="listener">The listener (topic and subscription)</param>
        /// <param name="maxMessages">The number of messages to process</param>
        /// <param name="groupMessages">Process messages asynchornously or sequentaially</param>
        /// <param name="delayBetweenPullsMs">Delay befor we start listening again</param>
        /// <returns></returns>
        private Task Listen(IEventListener listener, int maxMessages = 4, bool groupMessages = false, int delayBetweenPullsMs = 100)
        {
            return listener.DoWorkAsync(maxMessages, groupMessages);

        }

    }
}
