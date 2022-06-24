using DAL.EF;
using DAL.Entities;
using DAL.Repository;

namespace DAL.UOW;

public class EfUnitOfWork : IUnitOfWork
{
    private readonly ScheduleContext _context;

    private IRepository<Couple>? _couples;
    private bool _disposed;

    private IRepository<Group>? _groups;

    private IRepository<HomeworkTask>? _homework;

    private IRepository<Subject>? _subjects;

    private IRepository<User>? _users;

    public EfUnitOfWork(ScheduleContext context)
    {
        _context = context;
    }

    public IRepository<User> Users => _users ??= new UserRepository(_context);
    public IRepository<Group> Groups => _groups ??= new GroupsRepository(_context);
    public IRepository<Couple> Couples => _couples ??= new CouplesRepository(_context);
    public IRepository<Subject> Subjects => _subjects ??= new SubjectsRepository(_context);

    public IRepository<HomeworkTask> Homework => _homework ??= new HomeworkRepository(_context);

    public async void Save()
    {
        await _context.SaveChangesAsync();
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