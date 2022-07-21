using DAL.Entities;

namespace DAL.Repository.User;

public interface IUserRepository : IRepository<Entities.User>
{
    Task<bool> SetName(int id, string name);
    Task<bool> SetPicture(int id, string picUrl);
    Task<bool> SetPassword(int id, byte[] password, byte[] salt);
    Task<bool> MakeAdmin(int id);
    Task<bool> RemoveAdmin(int id);
    Task<bool> SetSettings(int id, Settings settings);
}