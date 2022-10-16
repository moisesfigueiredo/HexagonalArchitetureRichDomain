using Application.Bookings.Dtos;
using Application.Payment;
using Application.Rooms.Responses;
using Payment.Application;

namespace ApplicationTests
{
    public class MercadoPagoTests
    {
        [Fact]
        [Trait("Application", "Mercado Pago")]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.Equal(typeof(MercadoPagoAdapter), provider.GetType());
        }

        [Fact]
        [Trait("Application", "Mercado Pago")]
        public async Task Should_FailWhenPaymentIntentionStringIsInvalid()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("");

            Assert.False(res.Success);
            Assert.Equal(ErrorCodes.PAYMENT_INVALID_PAYMENT_INTENTION, res.ErrorCode);
            Assert.Equal("The selected payment intention is invalid", res.Message);
        }

        [Fact]
        [Trait("Application", "Mercado Pago")]
        public async Task Should_SuccessfullyProcessPayment()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            var res = await provider.CapturePayment("https://mercadopago.com.br/asdf");

            Assert.True(res.Success);
            Assert.Equal("Payment successfully processed", res.Message);
            Assert.NotNull(res.Data);
            Assert.NotNull(res.Data.CreatedDate);
            Assert.NotNull(res.Data.PaymentId);
        }
    }
}