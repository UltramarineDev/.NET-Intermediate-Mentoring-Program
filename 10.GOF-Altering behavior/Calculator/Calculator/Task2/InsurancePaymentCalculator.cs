using System;
using Calculator.Task2.Interfaces;
using Calculator.Task2.Models;

namespace Calculator.Task2
{
    public class InsurancePaymentCalculator : ICalculator
    {
        private ICurrencyService _currencyService;
        private ITripRepository _tripRepository;

        public InsurancePaymentCalculator(
            ICurrencyService currencyService,
            ITripRepository tripRepository)
        {
            _currencyService = currencyService;
            _tripRepository = tripRepository;
        }

        public decimal CalculatePayment(string touristName)
        {
            if (string.IsNullOrEmpty(touristName))
            {
                throw new ArgumentNullException(nameof(touristName));
            }

            var rate = _currencyService.LoadCurrencyRate();
            var tripDetails = _tripRepository.LoadTrip(touristName);

            if (tripDetails == null)
            {
                throw new ArgumentNullException($"Received null trip details for tourist {touristName}.");
            }

            return rate * (Constants.A * tripDetails.FlyCost + Constants.B * tripDetails.AccomodationCost + Constants.C * tripDetails.ExcursionCost);
        }
    }
}
