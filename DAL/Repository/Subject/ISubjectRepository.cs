namespace DAL.Repository.Subject;

public interface ISubjectRepository : IRepository<Entities.Subject>
{
    Task<bool> RemoveAll(int groupId);
}