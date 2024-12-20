using Helpers.MessageClient.Drivers.EasyNetQ;
using Helpers.MessageClient.Drivers.EasyNetQ.MessagingStrategies;

namespace Helpers.MessageClient.Factory;

public class EasyNetQFactory : MessageClientFactory
{
    public override MessageClient<TMessage> FactoryMethod<TMessage>(MessagingStrategy messagingStrategy)
    {
        //var connectionString = Environment.GetEnvironmentVariable("EASYNETQ_CONNECTION_STRING");
        var connectionString = "host=localhost;port=5672;virtualHost=/;username=guest;password=guest";

        if (connectionString is null or "")
        {
            throw new InvalidOperationException("EASYNETQ_CONNECTION_STRING environment variable not set");
        }

        return new MessageClient<TMessage>(
          new EasyNetQDriver<TMessage>(
            connectionString,
            messagingStrategy
          )
        );
    }

    public MessageClient<TMessage> CreatePubSubMessageClient<TMessage>(string clientId)
    {
        var pubSubStrategy = new PubSubStrategy(clientId);
        return FactoryMethod<TMessage>(pubSubStrategy);
    }

    public MessageClient<TMessage> CreateSendReceiveMessageClient<TMessage>(string queueName)
    {
        var sendReceiveStrategy = new SendReceiveStrategy(queueName);
        return FactoryMethod<TMessage>(sendReceiveStrategy);
    }

    public MessageClient<TMessage> CreateTopicMessageClient<TMessage>(string clientId, string topic)
    {
        var topicStrategy = new TopicStrategy(clientId, topic);
        return FactoryMethod<TMessage>(topicStrategy);
    }
}