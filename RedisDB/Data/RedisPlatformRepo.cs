using RedisDB.Models;
using RedisDB.RabbitMQ;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisDB.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {

        private readonly IConnectionMultiplexer _redis;
        private readonly IMessageProcedure _rabbitmq;

        public RedisPlatformRepo(IConnectionMultiplexer redis, IMessageProcedure rabbitmq)
        {
            _redis = redis;
            _rabbitmq = rabbitmq;
        }

        public void CreatePlatform(Platform plat)
        {
            if (plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(plat);

            //db.StringSet(plat.Id, serialPlat);

            db.HashSet("hashplatform", new HashEntry[]
                {new HashEntry(plat.Id, serialPlat)});

            _rabbitmq.SendMessage(plat);
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();

            var compeleteSet = db.HashGetAll("hashplatform");

            if (compeleteSet.Length > 0)
            {
                var obj = Array.ConvertAll(compeleteSet, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
                return obj;
            }

            return null;
        }

        public Platform? GetPlatformById(string id)
        {
            var db = _redis.GetDatabase();

            //var plat = db.StringGet(id);

            var plat = db.HashGet("hashplatform", id);
            
            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }

            return null;
        }
    }
}
