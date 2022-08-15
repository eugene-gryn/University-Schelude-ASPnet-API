using DAL.Entities;

namespace DAL.Repository.Homework;

public interface IHomeworkRepository : IRepository<HomeworkTask>
{
    Task<bool> SetSubject(int id, int subjectId);
}