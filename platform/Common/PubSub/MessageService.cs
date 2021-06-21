using Google.Apis.Storage.v1.Data;
using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Channel = Grpc.Core.Channel;

namespace Common.PubSub
{
    public class MessageService : IPubSubService
    {
        private readonly PublisherServiceApiClient _pub;
        private SubscriberServiceApiClient _sub;
        private readonly string _projectId;
        private SubscriptionName subscriptionName;
        private bool IsDev;

        public MessageService(string projId, bool isDev)
        {
            IsDev = isDev;
            _projectId = projId;

            if (IsDev)
                _pub = PublisherServiceApiClient.Create(new Channel("localhost:8085", ChannelCredentials.Insecure));
            else
                _pub = PublisherServiceApiClient.Create();

        }


        //publish data to the topic
        public async Task<bool> Send<T>(T msg, string topic, Dictionary<string, string> metadata = null)
        {
            //define the topic to publish to
            var topicName = new TopicName(_projectId, topic);

            var json = JsonConvert.SerializeObject(msg);

            var pubMsg = new PubsubMessage
            {
                Data = Google.Protobuf.ByteString.CopyFromUtf8(json)
            };

            if (metadata != null)
                pubMsg.Attributes.Add(metadata);

            var resp = await _pub.PublishAsync(topicName, new List<PubsubMessage>() { pubMsg });

            return true;

        }

        public async Task CreateTopic(string topicName)
        {
            try
            {
                var topic = new TopicName(_projectId, topicName);
                await _pub.CreateTopicAsync(topic);

            }
            catch (RpcException e)
            //when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                // Already exists.  That's fine.
            }
        }

        //Subscriber functions

        public async Task<bool> CreateSubscription(string subscriptionStr, string topicStr)
        {
            if (IsDev)
                _sub = SubscriberServiceApiClient.Create(new Channel("localhost:8085", ChannelCredentials.Insecure));
            else
                _sub = await SubscriberServiceApiClient.CreateAsync();

            subscriptionName = new SubscriptionName(_projectId, subscriptionStr);

            //define the topic to subscribe to
            var topicName = new TopicName(_projectId, topicStr);

            try
            {
                Subscription subscription = await _sub.CreateSubscriptionAsync(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            }
            catch (RpcException e)
            //when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                // Already exists.  That's fine.
            }

            try
            {
                await _sub.GetSubscriptionAsync(subscriptionName);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GCRP Error: {0}", ex);
            }

            return true;

        }


        public async Task<List<ReceivedMessage>> GetMessagesFromSubscription(int maxMessages = 4)
        {
            //https://cloud.google.com/pubsub/docs/pull#synchronous-pull
            var pullResp = await _sub.PullAsync(subscriptionName, returnImmediately: false, maxMessages: maxMessages);

            var res = pullResp.ReceivedMessages.ToList();

            return res;
        }

        public void AcknowledgeMessagesReceived(List<String> ackIds)
        {
            _sub.Acknowledge(subscriptionName, ackIds);
        }

    }
}
