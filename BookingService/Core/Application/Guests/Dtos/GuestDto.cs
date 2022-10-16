using Domain.Guests.Entities;
using Domain.Guests.Enums;
using Domain.Guests.ValueObjects;

namespace Application.Guests.DTO
{
    public class GuestDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string IdNumber { get; set; }
        public int IdTypeCode { get; set; }
        public static Guest MapToEntity(GuestDto guestDTO)
        {
            return new Guest
            {
                Id = guestDTO.Id,
                Name = guestDTO.Name,
                Surname = guestDTO.Surname,
                Email = guestDTO.Email,
                DocumentId = new PersonId
                {
                    IdNumber = guestDTO.IdNumber,
                    DocumentType = (DocumentType)guestDTO.IdTypeCode
                }
            };
        }

        public static GuestDto MapToDto(Guest guest)
        {
            return new GuestDto
            {
                Id = guest.Id,
                Email = guest.Email,
                IdNumber = guest.DocumentId.IdNumber,
                IdTypeCode = (int)guest.DocumentId.DocumentType,
                Name = guest.Name,
                Surname = guest.Surname,
            };
        }
    }
}
