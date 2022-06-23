using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace LibraryTesting.DataGenerator.OptionsFactory
{
    public class ScheduleInMemoryDbFactory : DAL.EF.ContextFactory<DAL.EF.ScheduleContext>
    {
        public ScheduleInMemoryDbFactory() : base("")
        {
        }

        public override DbContextOptions<ScheduleContext> CreateOptions()
        {
            throw new NotImplementedException();
        }
    }
}
