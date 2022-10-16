using Application.Bookings.Dtos;
using Application.Bookings.Ports;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GetBookingQuery : IRequest<BookingResponse>
    {
        public int Id { get; set; }
    }
}
