using Application.Guests.Requests;
using Application.Responses;

namespace Application.Guests.Ports
{
    public interface IGuestManager
    {
        Task<GuestResponse> CreateGuest(CreateGuestRequest request);
        Task<GuestResponse> GetGuest(int guestId);
    }
}
