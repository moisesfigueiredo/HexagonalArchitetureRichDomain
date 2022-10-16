using Domain.Bookings.Ports;
using Domain.Guests.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Bookings
{
    public class BookingRepository : IBookingRepository
    {
        private readonly HotelDbContext _dbContext;
        public BookingRepository(HotelDbContext hotelDbContext)
        {
            _dbContext = hotelDbContext;
        }
        public async Task<Booking> CreateBooking(Booking booking)
        {
            _dbContext.Bookings.Add(booking);
            await _dbContext.SaveChangesAsync();
            return booking;
        }

        public Task<Booking> Get(int id)
        {
            return _dbContext.Bookings
                .Include(b => b.Guest)
                .Include(b => b.Room)
                .Where(x => x.Id == id).FirstAsync();
        }
    }
}
