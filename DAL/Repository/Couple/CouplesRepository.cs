using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Couple;

public class CouplesRepository : EFRepository<Entities.Couple>, ICoupleRepository {
    public CouplesRepository(ScheduleContext context) : base(context) { }

    public override async Task<bool> Add(Entities.Couple item) {
        var addItem = MapAdd(item);

        await Context.Couples.AddAsync(addItem);

        return true;
    }

    public override async Task<bool> AddRange(IEnumerable<Entities.Couple> entities) {
        var list = entities.Select(MapAdd).ToList();

        await Context.Couples.AddRangeAsync(list);

        return true;
    }

    public override bool Add(ref Entities.Couple item) {
        var addItem = MapAdd(item);

        item = Context.Couples.Add(addItem).Entity;

        return true;
    }

    public override IQueryable<Entities.Couple> ReadById(int id) {
        return Read().Where(el => el.Id == id).AsQueryable();
    }

    public override Task<bool> UpdateAsync(Entities.Couple item) {
        Context.Entry(item).State = EntityState.Modified;

        return Task.FromResult(true);
    }

    public override bool Update(Entities.Couple item) {
        Context.Entry(item).State = EntityState.Modified;

        return true;
    }

    public override async Task<bool> Delete(int id) {
        var couple = await Context.Couples.Where(coupleT => coupleT.Id == id).FirstOrDefaultAsync();

        if (couple == null) return false;

        Context.Remove(couple);
        
        return true;
    }

    public async Task<bool> RemoveAll(int groupId) {
        var group = await Context.Groups
            .Where(g => g.Id == groupId)
            .Include(g => g.Couples)
            .SingleOrDefaultAsync();

        if (group == null) return false;

        group.Couples.Clear();
        Context.Groups.Update(group);

        return true;
    }

    protected override Entities.Couple MapAdd(Entities.Couple item) {
        return new Entities.Couple {
            Id = 0,
            Begin = item.Begin,
            End = item.End,
            SubjectId = item.SubjectId,
            GroupId = item.GroupId
        };
    }
}