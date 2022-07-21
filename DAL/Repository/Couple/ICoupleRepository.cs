namespace DAL.Repository.Couple;

public interface ICoupleRepository : IRepository<Entities.Couple>
{
    Task<bool> RemoveAll(int groupId);
}