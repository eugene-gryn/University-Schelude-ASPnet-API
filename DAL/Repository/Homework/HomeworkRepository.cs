using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.Homework
{
    public class HomeworkRepository : EFRepository<HomeworkTask>, IHomeworkRepository
    {
        public HomeworkRepository(ScheduleContext context) : base(context)
        {
        }

        public override async Task<bool> Add(HomeworkTask item)
        {
            item.Id = 0;

            await Context.Homework.AddAsync(item);

            return true;
        }

        public override async Task<bool> AddRange(IEnumerable<HomeworkTask> entities)
        {
            var list = entities as Entities.HomeworkTask[] ?? entities.ToArray();
            foreach (var item in list)
            {
                item.Id = 0;

                // TODO VALIDATIONS
            }

            await Context.Homework.AddRangeAsync(list);

            return true;
        }

        public override IQueryable<HomeworkTask> Read()
        {
            return Context.Homework.AsQueryable();
        }

        public override IQueryable<HomeworkTask> ReadById(int id)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Update(HomeworkTask item)
        {
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

        public Task<bool> SetDescription(int id, string description)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetDeadline(int id, DateTime deadline)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetSubject(int id, int subjectId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SetPriority(int id, byte priority)
        {
            throw new NotImplementedException();
        }
    }
}
