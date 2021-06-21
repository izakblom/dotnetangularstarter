using Common.PubSub;
using Google.Cloud.PubSub.V1;
using MediatR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.PubSub
{
    /// <summary>
    /// This is the exchange that decides what to do with every set of grouped messages as it is read from the queue.
    /// </summary>
    public class GroupedMessageProcessor : ISubFactoryGrouped
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public GroupedMessageProcessor(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> DoWork(List<ReceivedMessage> messages)
        {
            if (messages.Count == 0) return "No grouped messages to process";


            var tasks = new List<Task<bool>>();

            // Group List<ReceivedMessage> by type  
            var uniqueTypes = messages.Select(m => m.Message.Attributes["type"]).Distinct().ToList();

            uniqueTypes.ForEach(type =>
            {
                var type_messages = messages.Where(m => m.Message.Attributes["type"] == type).ToList();

                // TODO: Implement when we need to support processing of bulk events
                //switch (type)
                //{
                //    case "type-meta-tag-used-when-sending-the-message":
                //        tasks.Add(_mediator.Send(new SomeIMediatorCommand(type_messages)));
                //        break;
                //}
            });

            await Task.Factory.ContinueWhenAll(tasks.ToArray(), ConcurrentTasksCompleted);

            return $"Successfully processed {messages.Count} grouped messages";


        }

        /// <summary>
        /// Event is fired when all event tasks complete.
        /// </summary>
        /// <param name="tasks"></param>
        private void ConcurrentTasksCompleted(Task<bool>[] tasks)
        {
            Console.WriteLine($"Group processor handled {tasks.Length} grouped messages");
        }
    }
}
