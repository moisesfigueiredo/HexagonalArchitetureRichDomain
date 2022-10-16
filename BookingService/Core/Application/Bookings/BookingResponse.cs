using Application.Bookings.Dtos;
using Application.Rooms.Responses;

namespace Application.Bookings.Ports
{
    public class BookingResponse : Response
    {
        public BookingDto Data;
    }
}
