using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Homework;

public class HomeworkRepository : EFRepository<HomeworkTask>, IHomeworkRepository {
    public HomeworkRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(HomeworkTask item) {
        var addItem = MapAdd(item);

        if (addItem.Deadline < DateTime.Now) return false; // TODO: TEST VALIDATION

        await Context.Homework.AddAsync(item);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<HomeworkTask> entities) {
        var list = entities.Select(MapAdd).ToList();

        await Context.Homework.AddRangeAsync(list);

        return true;
    }

    public override bool Add(ref HomeworkTask item) {
        var addItem = MapAdd(item);

        item = Context.Homework.Add(addItem).Entity;

        return true;
    }

    public override IQueryable<HomeworkTask> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> UpdateAsync(HomeworkTask item) {
        if (item.Deadline < DateTime.Now) return Task.FromResult(false);

        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override bool Update(HomeworkTask item) {
        if (item.Deadline < DateTime.Now) return false;

        Context.Entry(item).State = EntityState.Modified;

        return true;
    }

    public override async Task<bool> Delete(int id) {
        var item = await Context.Homework.Where(task => task.Id == id).FirstOrDefaultAsync();

        if (item == null) return false;

        Context.Homework.Remove(item);

        return true;

    }

    public async Task<bool> SetSubject(int id, int subjectId) {
        var task = await ReadById(id).Include(h => h.Subject).SingleOrDefaultAsync();

        if (task == null) return false;
        if (!Context.Subjects.Any(s => s.Id == subjectId)) return false;

        task.SubjectId = subjectId;

        return await UpdateAsync(task);
    }

    protected override HomeworkTask MapAdd(HomeworkTask item) {
        return new HomeworkTask {
            Id = 0,
            SubjectId = item.SubjectId,
            Deadline = item.Deadline,
            Description = item.Description,
            Priority = item.Priority,
            UserId = item.UserId
        };
    }
}