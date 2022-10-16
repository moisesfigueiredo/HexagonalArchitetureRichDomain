using Application.Bookings.Dtos;
using Application.Payments;
using Payment.Application;
using Payments.Application;

namespace Application.Payment
{
    public class PaymentProcessorFactory : IPaymentProcessorFactory
    {

        public IPaymentProcessor GetPaymentProcessor(SupportedPaymentProviders selectedPaymentProvider)
        {
            switch (selectedPaymentProvider) 
            {
                case SupportedPaymentProviders.MercadoPago:
                    return new MercadoPagoAdapter();

                default: return new NotImplementedPaymentProvider();
            }
        }
    }
}
