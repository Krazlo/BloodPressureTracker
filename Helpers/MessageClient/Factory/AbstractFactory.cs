using Helpers.MessageClient.Drivers.EasyNetQ.MessagingStrategies;

namespace Helpers.MessageClient.Factory
{
    public abstract class MessageClientFactory
    {
      public abstract MessageClient<TMessage> FactoryMethod<TMessage>(MessagingStrategy messagingStrategy);
    }
};