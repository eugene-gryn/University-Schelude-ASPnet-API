using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Homework;

public class HomeworkRepository : EFRepository<HomeworkTask>, IHomeworkRepository {
    public HomeworkRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(HomeworkTask item) {
        var addItem = new HomeworkTask {
            Id = 0,
            SubjectId = item.SubjectId,
            Deadline = item.Deadline,
            Description = item.Description,
            Priority = item.Priority,
            UserId = item.UserId
        };

        if (addItem.Deadline < DateTime.Now) return false; // TODO: TEST VALIDATION

        await Context.Homework.AddAsync(item);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<HomeworkTask> entities) {
        var list = entities.Select(item => new HomeworkTask {
            Id = 0,
            SubjectId = item.SubjectId,
            Deadline = item.Deadline,
            Description = item.Description,
            Priority = item.Priority,
            UserId = item.UserId
        }).ToList();

        await Context.Homework.AddRangeAsync(list);

        return true;
    }

    public override Task<bool> Add(out HomeworkTask item) {
        throw new NotImplementedException();
    }

    public override Task<bool> AddRange(out IEnumerable<HomeworkTask> entities) {
        throw new NotImplementedException();
    }

    public override IQueryable<HomeworkTask> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(HomeworkTask item) {
        if (item.Deadline < DateTime.Now) return Task.FromResult(false);

        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id) {
        var item = await Context.Homework.Where(task => task.Id == id).FirstOrDefaultAsync();

        if (item != null) {
            Context.Homework.Remove(item);

            return true;
        }

        return false;
    }

    public Task<bool> SetSubject(int id, int subjectId) {
        throw new NotImplementedException();
    }
}