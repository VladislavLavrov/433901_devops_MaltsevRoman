using Microsoft.AspNetCore.Mvc;
using _8_Calculator.Models;

namespace _8_Calculator.Controllers
{
    public class CalculatorController : Controller
    {
        public IActionResult Index()
        {
            return View(new CalculatorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Calculate(CalculatorViewModel model)
        {
            switch (model.Operation)
            {
                case "Add":
                    model.Result = model.Number1 + model.Number2;
                    break;

                case "Subtract":
                    model.Result = model.Number1 - model.Number2;
                    break;

                case "Multiply":
                    model.Result = model.Number1 * model.Number2;
                    break;

                case "Divide":
                    if (model.Number2 == 0)
                    {
                        model.Error = "На ноль делить нельзя!";
                    }
                    else
                    {
                        model.Result = model.Number1 / model.Number2;
                    }
                    break;

                default:
                    model.Error = "Неизвестная операция!";
                    break;
            }

            return View("Index", model);
        }
    }
}