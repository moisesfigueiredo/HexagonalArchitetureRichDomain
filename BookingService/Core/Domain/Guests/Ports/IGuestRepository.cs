namespace Domain.Guests.Ports
{
    public interface IGuestRepository
    {
        Task<Entities.Guest> Get(int Id);
        Task<int> Create(Entities.Guest guest);
    }
}
