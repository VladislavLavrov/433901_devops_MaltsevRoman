using System.Globalization;
using Calculator.Data;
using Microsoft.AspNetCore.Mvc;
using Calculator.Services;
using Confluent.Kafka;
using System.Text.Json;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;
        private readonly KafkaProducerService<Null, string> _producer;

        public CalculatorController(
    CalculatorContext context,
    KafkaProducerService<Null, string> producer)
        {
            _context = context;
            _producer = producer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Calculate(
    double num1,
    double num2,
    Operation operation)
        {
            if (operation == Operation.Divide && num2 == 0)
            {
                ViewBag.Error = "Деление на ноль невозможно.";
                return View("Index");
            }

            var dataInputVariant = new DataInputVariant
            {
                Operand_1 = num1.ToString(
                    "G",
                    CultureInfo.InvariantCulture),

                Operand_2 = num2.ToString(
                    "G",
                    CultureInfo.InvariantCulture),

                Type_operation = operation.ToString(),
                Result = null
            };

            await SendDataToKafka(dataInputVariant);

            ViewBag.Message = "Данные отправлены в Kafka на обработку.";

            return View("Index");
        }

        [HttpPost]
        public IActionResult Callback(
        [FromBody] DataInputVariant inputData)
        {
            _context.DataInputVariants.Add(inputData);
            _context.SaveChanges();

            return Ok();
        }

        private async Task SendDataToKafka(
        DataInputVariant dataInputVariant)
        {
            var json = JsonSerializer.Serialize(dataInputVariant);

            await _producer.ProduceAsync(
                "maltsev",
                new Message<Null, string>
                {
                    Value = json
                });
        }
    }
}
