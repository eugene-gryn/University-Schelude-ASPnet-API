using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.CoupleRepository
{
    [TestFixture]
    public class SubjectRepoTests : BaseRepositoryTest
    {
        [Test]
        public async Task Creation_Successful()
        {
            CreateSubject();
            var item = Generator.Subjects.FirstOrDefault();

            var result = await Uow.Subjects.Add(item);
            Uow.Save();

            result.Should().Be(true);
            Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count);
        }
        [Test]
        public async Task RangeCreation_Successful()
        {
            int COUNT = 4;
            CreateSubject(COUNT);

            var result = await Uow.Subjects.AddRange(Generator.Subjects);
            Uow.Save();

            result.Should().Be(true);
            Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count);
        }
    }
}
