using Domain.Guests.Enums;
using Domain.Guests.ValueObjects;
using Domain.Rooms.Entities;

namespace Application.Rooms.Dtos
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public bool InMaintenance { get; set; }
        public decimal Price { get; set; }
        public AcceptedCurrencies Currency { get; set; }

        public static Room MapToEntity(RoomDto dto)
        {
            return new Room
            {
                Id = dto.Id,
                Name = dto.Name,
                Level = dto.Level,
                InMaintenance = dto.InMaintenance,
                Price = new Price { Currency = dto.Currency, Value = dto.Price }
            };
        }
    }
}
