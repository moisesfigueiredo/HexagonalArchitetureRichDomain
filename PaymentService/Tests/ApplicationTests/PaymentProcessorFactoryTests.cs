using Application.Bookings.Dtos;
using Application.Payment;
using Application.Rooms.Responses;
using Payment.Application;
using Payments.Application;

namespace ApplicationTests
{
    public class PaymentProcessorFactoryTests
    {
        [Fact]
        [Trait("Application", "Payment Processor Factory")]
        public void ShouldReturn_NotImplementedPaymentProvider_WhenAskingForStripeProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            Assert.Equal(typeof(NotImplementedPaymentProvider), provider.GetType());
        }

        [Fact]
        [Trait("Application", "Payment Processor Factory")]
        public void ShouldReturn_MercadoPagoAdapter_Provider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.MercadoPago);

            Assert.Equal(typeof(MercadoPagoAdapter), provider.GetType());
        }

        [Fact]
        [Trait("Application", "Payment Processor Factory")]
        public async Task ShouldReturnFalse_WhenCapturingPaymentFor_NotImplementedPaymentProvider()
        {
            var factory = new PaymentProcessorFactory();

            var provider = factory.GetPaymentProcessor(SupportedPaymentProviders.Stripe);

            var res = await provider.CapturePayment("https://myprovider.com/asdf");

            Assert.False(res.Success);
            Assert.Equal(ErrorCodes.PAYMENT_PROVIDER_NOT_IMPLEMENTED, res.ErrorCode);
            Assert.Equal("The selected payment provider is not available at the moment", res.Message);
        }
    }
}
