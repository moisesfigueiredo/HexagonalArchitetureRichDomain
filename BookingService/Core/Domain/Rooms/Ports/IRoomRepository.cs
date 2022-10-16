using Domain.Rooms.Entities;

namespace Domain.Rooms.Ports
{
    public interface IRoomRepository
    {
        Task<Room> Get(int Id);
        Task<int> Create(Entities.Room room);
        Task<Room> GetAggregate(int Id);
    }
}
