
using Domain.Bookings.Entities;
using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Domain.Rooms.Entities;

namespace Application.Bookings.Dtos
{
    public class BookingDto
    {
        public BookingDto()
        { 
            this.PlacedAt = DateTime.UtcNow;
        }

        public int Id { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public int RoomId { get; set; }
        public int GuestId { get; set; }
        public BookingStatus Status { get; set; }

        public static Booking MapToEntity(BookingDto bookingDto)
        {
            return new Booking
            {
                Id = bookingDto.Id,
                Start = bookingDto.Start,
                Guest = new Guest { Id = bookingDto.GuestId },
                Room = new Room { Id = bookingDto.RoomId },
                End = bookingDto.End,
                PlacedAt = bookingDto.PlacedAt,
            };
        }

        public static BookingDto MapToDto(Booking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                End = booking.End,
                GuestId = booking.Guest.Id,
                PlacedAt = booking.PlacedAt,
                RoomId = booking.Room.Id,
                Status = booking.Status,
                Start = booking.Start
            };
        }
    }
}
