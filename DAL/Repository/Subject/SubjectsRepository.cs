using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Subject;

public class SubjectsRepository : EFRepository<Entities.Subject>, ISubjectRepository {
    public SubjectsRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Subject item) {
        var addItem = new Entities.Subject {
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


        await Context.Subjects.AddAsync(addItem);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Subject> entities) {
        var list = entities.Select(item => new Entities.Subject {
            Id = 0,
            GroupId = item.GroupId,
            Couples = new List<Entities.Couple>(),
            Homework = new List<HomeworkTask>(),
            IsPractice = item.IsPractice,
            Location = item.Location,
            Name = item.Name,
            Teacher = item.Teacher,
            Url = item.Url
        });

        await Context.Subjects.AddRangeAsync(list);

        return true;
    }

    public override Task<bool> Add(out Entities.Subject item) {
        throw new NotImplementedException();
    }

    public override Task<bool> AddRange(out IEnumerable<Entities.Subject> entities) {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.Subject> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.Subject item) {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id) {
        var item = await Context.Subjects.Where(subject => subject.Id == id).FirstOrDefaultAsync();

        if (item != null) {
            Context.Subjects.Remove(item);
            return true;
        }

        return false;
    }

    public Task<bool> RemoveAll(int groupId) {
        throw new NotImplementedException();
    }
}