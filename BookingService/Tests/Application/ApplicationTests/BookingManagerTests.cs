using Application.Bookings.Dtos;
using Application.Bookings.Ports;
using Application.Payment;
using Application.Payments;
using Application.Payments.Dtos;
using Application.Payments.Responses;
using Domain.Bookings.Ports;
using Domain.Guests.Ports;
using Domain.Rooms.Ports;
using Moq;

namespace ApplicationTests
{
    public class BookingManagerTests
    {
        [Fact]
        [Trait("Application", "Booking Manager")]

        public async Task Should_PayForABooking()
        {
            var dto = new PaymentRequestDto
            {
                SelectedPaymentProvider = SupportedPaymentProviders.MercadoPago,
                PaymentIntention = "https://www.mercadopago.com.br/asdf",
                SelectedPaymentMethod = SupportedPaymentMethods.CreditCard
            };

            var bookingRepository = new Mock<IBookingRepository>();
            var roomRepository = new Mock<IRoomRepository>();
            var guestRepository = new Mock<IGuestRepository>();
            var paymentProcessorFactory = new Mock<IPaymentProcessorFactory>();
            var paymentProcessor = new Mock<IPaymentProcessor>();

            var responseDto = new PaymentStateDto
            {
                CreatedDate = DateTime.Now,
                Message = $"Successfully paid {dto.PaymentIntention}",
                PaymentId = "123",
                Status = Status.Success
            };

            var response = new PaymentResponse
            {
                Data = responseDto,
                Success = true,
                Message = "Payment successfully processed"
            };

            paymentProcessor.
                Setup(x => x.CapturePayment(dto.PaymentIntention))
                .ReturnsAsync(response);

            paymentProcessorFactory
                .Setup(x => x.GetPaymentProcessor(dto.SelectedPaymentProvider))
                .Returns(paymentProcessor.Object);

            var bookingManager = new BookingManager(
                bookingRepository.Object,
                roomRepository.Object,
                guestRepository.Object,
                paymentProcessorFactory.Object);

            var res = await bookingManager.PayForABooking(dto);

            Assert.NotNull(res);
            Assert.True(res.Success);
            Assert.Equal("Payment successfully processed", res.Message);
        }
    }
}
