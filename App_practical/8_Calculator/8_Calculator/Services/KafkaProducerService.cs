
using Confluent.Kafka;

namespace Calculator.Services
{
    public class KafkaProducerService<TKey, TValue> : IDisposable
    {
        private readonly IProducer<TKey, TValue> _producer;

        public KafkaProducerService(KafkaProducerHandler handler)
        {
            _producer = handler.CreateProducer<TKey, TValue>();
        }

        public Task<DeliveryResult<TKey, TValue>> ProduceAsync(
            string topic,
            Message<TKey, TValue> message)
        {
            return _producer.ProduceAsync(topic, message);
        }

        public void Dispose()
        {
            _producer.Flush(TimeSpan.FromSeconds(5));
            _producer.Dispose();
        }
    }
}
