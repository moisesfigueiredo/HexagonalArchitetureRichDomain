using Application.Bookings.Dtos;
using Application.Payment;

namespace Application.Payments
{
    public interface IPaymentProcessorFactory
    {
        IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider);
    }
}
