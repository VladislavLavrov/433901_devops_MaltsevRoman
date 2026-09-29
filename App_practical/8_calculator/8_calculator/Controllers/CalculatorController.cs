using Microsoft.AspNetCore.Mvc;
using SimpleCalc.Models;

namespace SimpleCalc.Controllers
{
    public class CalculatorController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View(new CalculatorModel());
        }

        [HttpPost]
        public IActionResult Index(CalculatorModel model)
        {
            switch (model.Operation)
            {
                case "+": model.Result = model.Num1 + model.Num2; break;
                case "-": model.Result = model.Num1 - model.Num2; break;
                case "*": model.Result = model.Num1 * model.Num2; break;
                case "/":
                    model.Result = model.Num2 == 0 ? null : model.Num1 / model.Num2;
                    break;
            }
            return View(model);
        }
    }
}