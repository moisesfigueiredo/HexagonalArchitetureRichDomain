using Application.Payments.Dtos;
using Application.Rooms.Responses;

namespace Application.Payments.Responses
{
    public class PaymentResponse : Response
    {
        public PaymentStateDto Data { get; set; }
    }
}
