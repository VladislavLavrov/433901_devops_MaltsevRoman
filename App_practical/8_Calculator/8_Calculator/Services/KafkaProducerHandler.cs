
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Calculator.Services
{
    public class KafkaProducerHandler
    {
        private readonly ProducerConfig _producerConfig;

        public KafkaProducerHandler(IConfiguration config)
        {
            _producerConfig = new ProducerConfig();
            config.GetSection("Kafka:ProducerSettings")
                  .Bind(_producerConfig);
        }

        public IProducer<TKey, TValue> CreateProducer<TKey, TValue>()
        {
            return new ProducerBuilder<TKey, TValue>(_producerConfig)
                .Build();
        }
    }
}
