namespace RedisDB.RabbitMQ
{
    public interface IMessageProcedure
    {
        void SendMessage<T>(T message);
    }
}
