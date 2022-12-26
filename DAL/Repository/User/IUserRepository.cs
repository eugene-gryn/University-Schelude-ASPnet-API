using DAL.Entities;

namespace DAL.Repository.User;

public interface IUserRepository : IRepository<Entities.User> {
    Task<List<UserRole>> GetUserGroups(int userId);

    Task<int> GroupCount(int userId);
    Task<bool> GroupJoin(int userId, int groupId, bool fullAccess);
    Task<bool> GroupLeave(int userId, int groupId);
}