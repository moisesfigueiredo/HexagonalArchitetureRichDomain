using Domain.Guests.Entities;

namespace Domain.Bookings.Ports
{
    public interface IBookingRepository
    {
        Task<Guests.Entities.Booking> Get(int id);
        Task<Booking> CreateBooking(Booking booking);
    }
}
