namespace _8_Calculator.Models
{
    public class CalculatorViewModel
    {
        public double Number1 { get; set; }

        public double Number2 { get; set; }

        public string Operation { get; set; } = "Add";

        public double? Result { get; set; }

        public string? Error { get; set; }
    }
}