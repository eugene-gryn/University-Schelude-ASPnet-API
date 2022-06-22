using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using DAL.Entities;

namespace DAL.Repository
{
    public class CouplesRepository : EFRepository<Couple>
    {
        public CouplesRepository(ScheduleContext context) : base(context)
        {
        }

        public override Task<Couple> Create(Couple item)
        {
            throw new NotImplementedException();
        }

        public override IQueryable<Couple> Read()
        {
            return Context.Couples.AsQueryable();
        }

        public override Task<bool> Update(Couple item)
        {
            throw new NotImplementedException();
        }

        public override Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
