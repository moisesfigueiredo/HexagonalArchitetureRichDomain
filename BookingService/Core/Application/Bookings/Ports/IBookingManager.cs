using Application.Bookings.Dtos;
using Application.Payments.Responses;

namespace Application.Bookings.Ports
{
    public interface IBookingManager
    {
        Task<BookingResponse> CreateBooking(BookingDto booking);
        Task<PaymentResponse> PayForABooking(PaymentRequestDto paymentRequestDto);
        Task<BookingDto> GetBooking(int id);
    }
}
