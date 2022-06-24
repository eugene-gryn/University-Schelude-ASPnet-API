using DAL.Entities;
using DAL.Repository;

namespace DAL.UOW;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> Users { get; }
    IRepository<Group> Groups { get; }
    IRepository<Couple> Couples { get; }
    IRepository<Subject> Subjects { get; }
    IRepository<HomeworkTask> Homework { get; }

    void Save();
}