using Calculator.Task3.Models;

namespace Calculator.Task3.Interfaces
{
    public interface ITripRepository
    {
        TripDetails LoadTrip(string touristName);
    }
}
