namespace Hb.MarsRover.Domain.Messages
{
    public interface ICommand<out TResponse> : IMessage
    {
    }
}