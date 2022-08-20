using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Couple;

public class CouplesRepository : EFRepository<Entities.Couple>, ICoupleRepository {
    public CouplesRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Couple item) {
        var addItem = new Entities.Couple {
            Id = 0,
            Begin = item.Begin,
            End = item.End,
            SubjectId = item.SubjectId,
            GroupId = item.GroupId
        };

        await Context.Couples.AddAsync(addItem);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Couple> entities) {
        var list = entities.Select(item => new Entities.Couple {
            Id = 0,
            Begin = item.Begin,
            End = item.End,
            SubjectId = item.SubjectId,
            GroupId = item.GroupId
        }).ToList();

        await Context.Couples.AddRangeAsync(list);

        return true;
    }

    public override Task<bool> Add(out Entities.Couple item) {
        throw new NotImplementedException();
    }

    public override Task<bool> AddRange(out IEnumerable<Entities.Couple> entities) {
        throw new NotImplementedException();
    }

    public override IQueryable<Entities.Couple> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> Update(Entities.Couple item) {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override async Task<bool> Delete(int id) {
        var couple = await Context.Couples.Where(coupleT => coupleT.Id == id).FirstOrDefaultAsync();

        if (couple != null) {
            Context.Remove(couple);
            return true;
        }

        return false;
    }

    public Task<bool> RemoveAll(int groupId) {
        throw new NotImplementedException();
    }
}