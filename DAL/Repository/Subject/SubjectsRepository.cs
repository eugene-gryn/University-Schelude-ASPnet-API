using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Subject;

public class SubjectsRepository : EFRepository<Entities.Subject>, ISubjectRepository {
    public SubjectsRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Subject item) {
        var addItem = MapAdd(item);

        await Context.Subjects.AddAsync(addItem);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Subject> entities) {
        var list = entities.Select(MapAdd);

        await Context.Subjects.AddRangeAsync(list);

        return true;
    }

    public override bool Add(ref Entities.Subject item) {
        var addItem = MapAdd(item);

        item = Context.Subjects.Add(addItem).Entity;

        return true;
    }

    public override IQueryable<Entities.Subject> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.Subject item) {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    // TODO check removed tasks and couples
    public override async Task<bool> Delete(int id) {
        var item = await Context.Subjects.Where(subject => subject.Id == id).FirstOrDefaultAsync();

        if (item == null) return false;

        Context.Subjects.Remove(item);

        return true;
    }

    public async Task<bool> RemoveAll(int groupId) {
        var group = await Context.Groups
            .Where(g => g.Id == groupId)
            .Include(g => g.Subjects)
            .SingleOrDefaultAsync();

        if (group == null) return false;

        group.Subjects.Clear();

        Context.Update(group);

        return true;
    }

    protected override Entities.Subject MapAdd(Entities.Subject item) {
        return new Entities.Subject {
            Id = 0,
            GroupId = item.GroupId,
            Couples = new List<Entities.Couple>(),
            Homework = new List<HomeworkTask>(),
            IsPractice = item.IsPractice,
            Location = item.Location,
            Name = item.Name,
            Teacher = item.Teacher,
            Url = item.Url
        };
    }
}