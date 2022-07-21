using DAL.Entities;

namespace DAL.Repository.Homework;

public interface IHomeworkRepository : IRepository<HomeworkTask>
{
    Task<bool> SetDescription(int id, string description);
    Task<bool> SetDeadline(int id, DateTime deadline);
    Task<bool> SetSubject(int id, int subjectId);
    Task<bool> SetPriority(int id, byte priority);
}