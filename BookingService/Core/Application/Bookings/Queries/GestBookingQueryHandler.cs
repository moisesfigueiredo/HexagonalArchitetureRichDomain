using Application.Bookings.Dtos;
using Application.Bookings.Ports;
using Domain.Bookings.Ports;
using MediatR;

namespace Application.Bookings.Queries
{
    public class GestBookingQueryHandler : IRequestHandler<GetBookingQuery, BookingResponse>
    {
        private readonly IBookingRepository _bookingRepository;

        public GestBookingQueryHandler(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<BookingResponse> Handle(GetBookingQuery request, CancellationToken cancellationToken)
        {
            var booking =  await _bookingRepository.Get(request.Id);

            var bookingDto = BookingDto.MapToDto(booking);

            return new BookingResponse
            {
                Success = true,
                Data = bookingDto
            };
        }
    }
}
