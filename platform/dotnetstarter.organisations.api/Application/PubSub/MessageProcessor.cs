using Common.PubSub;
using Google.Cloud.PubSub.V1;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.PubSub
{
    /// <summary>
    /// This is the exchange that decides what to do with every message as it is read from the queue.
    /// </summary>
    public class MessageProcessor : ISubFactory
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;

        public MessageProcessor(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> DoWork(ReceivedMessage message)
        {
            byte[] json = message.Message.Data.ToByteArray();

            var type = message.Message.Attributes["type"];

            // TODO: implement use cases
            //switch (type)
            //{
            //    case "some_event_type":
            //        var dto = JsonConvert.DeserializeObject<DTO1>(Encoding.UTF8.GetString(json));    // See Common.DataObject.PubSub for DTOs that can be deserialized here.
            //        passed = await _mediator.Send(BuildCommand1FromDTO(dto));
            //        break;
            //    case "some_other_event_type":
            //        var dto = JsonConvert.DeserializeObject<DTO2>(Encoding.UTF8.GetString(json));   // See Common.DataObject.PubSub for DTOs that can be deserialized here.
            //        passed = await _mediator.Send(BuildCommand2FromDTO(dto));
            //        break;
                
            //}

            return message.AckId;
        }
    }
}
