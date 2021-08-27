using Calculator.Task2.Models;

namespace Calculator.Task2.Interfaces
{
    public interface ITripRepository
    {
        TripDetails LoadTrip(string touristName);
    }
}
