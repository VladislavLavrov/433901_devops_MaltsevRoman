namespace Calculator.Services
{
    public static class CalculatorLibrary
    {
        public static double CalculateOperation(
            double num1,
            double num2,
            string operation)
        {
            switch (operation)
            {
                case "Add":
                    return num1 + num2;

                case "Subtract":
                    return num1 - num2;

                case "Multiply":
                    return num1 * num2;

                case "Divide":
                    if (num2 == 0)
                    {
                        throw new DivideByZeroException(
                            "Деление на ноль невозможно."
                        );
                    }

                    return num1 / num2;

                default:
                    throw new ArgumentException(
                        $"Неизвестная операция: {operation}"
                    );
            }
        }
    }
}