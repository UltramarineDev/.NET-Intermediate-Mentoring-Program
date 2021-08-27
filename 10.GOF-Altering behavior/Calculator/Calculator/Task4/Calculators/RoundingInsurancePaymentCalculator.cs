using System;
using Calculator.Task4.Interfaces;

namespace Calculator.Task4.Calculators
{
    public class RoundingInsurancePaymentCalculator : CalculatorDecorator
    {
        public RoundingInsurancePaymentCalculator(ICalculator calculator) : base(calculator) { }

        public override decimal CalculatePayment(string touristName)
        {
            var payment = _calculator.CalculatePayment(touristName);

            return Math.Round(payment);
        }
    }
}
