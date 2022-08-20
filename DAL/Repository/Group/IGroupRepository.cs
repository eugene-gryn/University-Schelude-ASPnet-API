using DAL.Entities;

namespace DAL.Repository.Group;

public interface IGroupRepository : IRepository<Entities.Group>
{
    Task<bool> AddUser(int groupId, int userId);
    Task<bool> RemoveUser(int groupId, int userId);
    Task<Entities.Couple?> NearCouple(int groupId);
    Task<List<Entities.Couple>> DayCouples(int groupId, DateTime date);
    Task<bool> SetNewCreator(int groupId, int userId);
    Task<bool> AddModerator(int groupId, int userId);
    Task<bool> RemoveModerator(int groupId, int userId);
}