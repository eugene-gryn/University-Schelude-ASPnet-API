using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Homework;

public class HomeworkRepository : EFRepository<HomeworkTask>, IHomeworkRepository
{
    public HomeworkRepository(ScheduleContext context) : base(context)
    {
    }

    public override async Task<bool> Add(HomeworkTask item)
    {
        item.Id = 0;
        item.Subject = Context.Subjects.FirstOrDefault(subj => subj.Id == item.Subject.Id)!;
        if (item.Deadline < DateTime.Now) return false; // TODO: TEST VALIDATION

        await Context.Homework.AddAsync(item);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<HomeworkTask> entities)
    {
        var list = entities as HomeworkTask[] ?? entities.ToArray();
        foreach (var item in list)
        {
            item.Id = 0;
            item.Subject = Context.Subjects.FirstOrDefault(subj => subj.Id == item.Subject.Id)!;
            if (item.Deadline < DateTime.Now) return false;
        }

        await Context.Homework.AddRangeAsync(list);

        return true;
    }

    public override IQueryable<HomeworkTask> ReadById(int id)
    {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(HomeworkTask item)
    {
        if (item.Deadline < DateTime.Now) return Task.FromResult(false);

        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id)
    {
        var item = await Context.Homework.Where(task => task.Id == id).FirstOrDefaultAsync();

        if (item != null)
        {
            Context.Homework.Remove(item);

            return true;
        }

        return false;
    }

    public Task<bool> SetSubject(int id, int subjectId)
    {
        throw new NotImplementedException();
    }
}