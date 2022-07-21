using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Subject
{
    public class SubjectsRepository : EFRepository<Subject>
    {
        public SubjectsRepository(ScheduleContext context) : base(context)
        {
        }

        public override async Task<Subject> Create(Subject item)
        {
            item.Id = 0;

            await Context.Subjects.AddAsync(item);

            return item;
        }

        public override IQueryable<Subject> Read()
        {
            return Context.Subjects.AsQueryable();
        }

        public override Task<bool> Update(Subject item)
        {
            Context.Entry(item).State = EntityState.Modified;

            return Task.FromResult(true);
        }

        public override async Task<bool> Delete(int id)
        {
            var item = await Context.Subjects.Where(subject => subject.Id == id).FirstOrDefaultAsync();

            if (item != null)
            {
                Context.Subjects.Remove(item);
                return true;
            }

            return false;
        }
    }
}
