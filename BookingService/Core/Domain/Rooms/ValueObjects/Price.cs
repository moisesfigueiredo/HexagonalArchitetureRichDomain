using Domain.Guests.Enums;

namespace Domain.Rooms.ValueObjects
{
    public class Price
    {
        public decimal Value { get; set; }
        public AcceptedCurrencies Currency { get; set; }
    }
}
