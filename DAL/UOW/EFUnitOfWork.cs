using DAL.EF;
using DAL.Entities;
using DAL.Repository;
using DAL.Repository.Couple;
using DAL.Repository.Group;
using DAL.Repository.Homework;
using DAL.Repository.Subject;
using DAL.Repository.User;

namespace DAL.UOW;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ScheduleContext _context;

    private ICoupleRepository? _couples;
    private bool _disposed;

    private IGroupRepository? _groups;

    private IHomeworkRepository? _homework;

    private ISubjectRepository? _subjects;

    private IUserRepository? _users;

    public EfUnitOfWork(ScheduleContext context)
    {
        _context = context;
    }

    public IUserRepository Users => _users ??= new UserRepository(_context);
    public IGroupRepository Groups => _groups ??= new GroupsRepository(_context);
    public ICoupleRepository Couples => _couples ??= new CouplesRepository(_context);
    public ISubjectRepository Subjects => _subjects ??= new SubjectsRepository(_context);
    public IHomeworkRepository Homework => _homework ??= new HomeworkRepository(_context);

    public void Save()
    {
        _context.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing) _context.Dispose();

        _disposed = true;
    }
}