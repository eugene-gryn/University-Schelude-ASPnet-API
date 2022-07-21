using DAL.Entities;

namespace DAL.Repository.Group;

public interface IGroupRepository : IRepository<Entities.Group>
{
    Task<bool> AddUser(int groupId, int userId);
    Task<bool> RemoveUser(int groupId, int userId);
    Task<Entities.Couple> GetCouple(int groupId, int coupleId);
    Task<Entities.Couple> NearCouple(int groupId);
    Task<List<Entities.Couple>> TodayCouples(int groupId);
    Task<Entities.Group> GetFullGroup(int groupId);
    Task<bool> SetNewCreator(int groupId, int userId);
    Task<bool> AddModerator(int groupId, int userId);
    Task<bool> RemoveModerator(int groupId, int userId);
    Task<bool> SetName(int groupId, string name);
    Task<bool> SetPrivacy(int groupId, bool privacy);
    Task<bool> AddHomeworkTask(int groupId, HomeworkTask task);
}