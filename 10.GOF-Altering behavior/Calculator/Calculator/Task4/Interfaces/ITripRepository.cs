using Calculator.Task4.Models;

namespace Calculator.Task4.Interfaces
{
    public interface ITripRepository
    {
        TripDetails LoadTrip(string touristName);
    }
}
