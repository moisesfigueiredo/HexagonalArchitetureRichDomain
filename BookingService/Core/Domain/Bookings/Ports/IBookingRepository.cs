using Domain.Bookings.Entities;

namespace Domain.Bookings.Ports
{
    public interface IBookingRepository
    {
        Task<Booking> Get(int id);
        Task<Booking> CreateBooking(Booking booking);
    }
}
