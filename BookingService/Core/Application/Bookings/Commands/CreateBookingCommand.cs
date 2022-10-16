using Application.Bookings.Dtos;
using Application.Bookings.Ports;
using MediatR;

namespace Application.Bookings.Commands
{
    public class CreateBookingCommand : IRequest<BookingResponse>
    {
        public BookingDto BookingDto {get; set;}
    }
}
