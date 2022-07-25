using DAL.Repository.Couple;
using DAL.Repository.Group;
using DAL.Repository.Homework;
using DAL.Repository.Subject;
using DAL.Repository.User;

namespace DAL.UOW;

public interface IUnitOfWork : IDisposable
{
    IUserRepository Users { get; }
    IGroupRepository Groups { get; }
    ICoupleRepository Couples { get; }
    ISubjectRepository Subjects { get; }
    IHomeworkRepository Homework { get; }

    void Save();
}