
using Calculator.Controllers;
using Calculator.Data;
using Calculator.Services;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;

namespace Calculator.Services
{
    public class KafkaConsumerService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;

        public KafkaConsumerService(
            IConfiguration configuration,
            IHttpClientFactory clientFactory)
        {
            _configuration = configuration;
            _clientFactory = clientFactory;
        }

        protected override Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            return Task.Run(
                () => StartConsumerLoop(stoppingToken),
                stoppingToken);
        }

        private async Task StartConsumerLoop(
            CancellationToken cancellationToken)
        {
            var consumerConfig = new ConsumerConfig();

            _configuration
                .GetSection("Kafka:ConsumerSettings")
                .Bind(consumerConfig);

            using var consumer =
                new ConsumerBuilder<Ignore, string>(
                    consumerConfig).Build();

            var topic = _configuration["Kafka:TopicName"];

            if (string.IsNullOrWhiteSpace(topic))
            {
                throw new InvalidOperationException(
                    "Не задан Kafka:TopicName.");
            }

            consumer.Subscribe(topic);

            var httpClient = _clientFactory.CreateClient();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    var message = consumer.Consume(
                        cancellationToken);

                    var inputData =
                        JsonSerializer.Deserialize<DataInputVariant>(
                            message.Message.Value);

                    if (inputData == null)
                    {
                        Console.WriteLine(
                            "Получено пустое сообщение Kafka.");
                        continue;
                    }

                    var num1 = double.Parse(
                        inputData.Operand_1!,
                        CultureInfo.InvariantCulture);

                    var num2 = double.Parse(
                        inputData.Operand_2!,
                        CultureInfo.InvariantCulture);

                    var result = CalculatorLibrary.CalculateOperation(
                        num1,
                        num2,
                        inputData.Type_operation!);

                    inputData.Result = result.ToString(
                        "G",
                        CultureInfo.InvariantCulture);

                    var response = await httpClient.PostAsJsonAsync(
                        "http://localhost:5008/Calculator/Callback",
                        inputData,
                        cancellationToken);

                    response.EnsureSuccessStatusCode();

                    Console.WriteLine(
                        $"Kafka: {num1} {inputData.Type_operation} " +
                        $"{num2} = {result}");
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (ConsumeException ex)
                {
                    Console.WriteLine(
                        $"Ошибка Kafka: {ex.Error.Reason}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"Ошибка обработки сообщения: {ex.Message}");
                }
            }

            consumer.Close();
        }
    }
}
