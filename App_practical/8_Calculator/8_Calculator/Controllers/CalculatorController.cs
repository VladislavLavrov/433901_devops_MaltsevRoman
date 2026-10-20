using System.Globalization;
using Calculator.Data;
using Microsoft.AspNetCore.Mvc;

namespace Calculator.Controllers
{
    public enum Operation { Add, Subtract, Multiply, Divide }

    public class CalculatorController : Controller
    {
        private readonly CalculatorContext _context;

        public CalculatorController(CalculatorContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(double num1, double num2, Operation operation)
        {
            double result;

            switch (operation)
            {
                case Operation.Add:
                    result = num1 + num2;
                    break;
                case Operation.Subtract:
                    result = num1 - num2;
                    break;
                case Operation.Multiply:
                    result = num1 * num2;
                    break;
                case Operation.Divide:
                    if (num2 == 0)
                    {
                        ViewBag.Error = "Деление на ноль невозможно.";
                        return View("Index");
                    }
                    result = num1 / num2;
                    break;
                default:
                    ViewBag.Error = "Выбрана неизвестная операция.";
                    return View("Index");
            }

            ViewBag.Result = result.ToString("G", CultureInfo.CurrentCulture);

            var record = new DataInputVariant
            {
                Operand_1 = num1.ToString("G", CultureInfo.InvariantCulture),
                Operand_2 = num2.ToString("G", CultureInfo.InvariantCulture),
                Type_operation = operation.ToString(),
                Result = result.ToString("G", CultureInfo.InvariantCulture)
            };

            _context.DataInputVariants.Add(record);
            _context.SaveChanges();

            return View("Index");
        }
    }
}
