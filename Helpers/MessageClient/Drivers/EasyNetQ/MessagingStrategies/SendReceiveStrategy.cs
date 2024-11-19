using EasyNetQ;

namespace Helpers.MessageClient.Drivers.EasyNetQ.MessagingStrategies;

public class SendReceiveStrategy : MessagingStrategy
{
  private readonly string _queueName;
  public SendReceiveStrategy(string queueName)
  {
    _queueName = queueName;
  }
  public override void Send<TMessage>(TMessage message, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Send");
    
    bus.SendReceive.Send(_queueName, message);
  }

  public override IDisposable Listen<TMessage>(Action<TMessage> callback, IBus? bus)
  {
    if(bus == null)
      throw new InvalidOperationException("You must call Connect before Listen");
    return bus.SendReceive.Receive(_queueName, callback);
  }
}