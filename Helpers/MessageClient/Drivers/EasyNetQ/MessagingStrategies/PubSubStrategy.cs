using EasyNetQ;

namespace Helpers.MessageClient.Drivers.EasyNetQ.MessagingStrategies;

public class PubSubStrategy : MessagingStrategy
{
  private readonly string? _clientId;
  public PubSubStrategy(string? clientId)
  {
    _clientId = clientId;
  }
  public override void Send<TMessage>(TMessage message, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Send");
    
    bus.PubSub.Publish(message);
  }

  public override IDisposable Listen<TMessage>(Action<TMessage> callback, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Listen");
    
    return bus.PubSub.Subscribe(_clientId, callback);
  }
}