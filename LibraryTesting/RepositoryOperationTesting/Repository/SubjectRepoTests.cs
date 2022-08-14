using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.Repository
{
    [TestFixture]
    public class SubjectRepoTests : BaseRepositoryTest
    {
        [Test]
        public async Task Creation_Successful()
        {
            await LoadRandomDataSet(10);

            var item = Generator.GenEmptySubject(1, Uow.Groups.Read().First()).First();

            var result = await Uow.Subjects.Add(item);
            Uow.Save();

            result.Should().Be(true);
            Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count + 1);
        }
        [Test]
        public async Task RangeCreation_Successful()
        {
            await LoadRandomDataSet(10);

            int COUNT = 4;
            var items = Generator.GenEmptySubject(COUNT, Uow.Groups.Read().First());

            var result = await Uow.Subjects.AddRange(items);
            Uow.Save();

            result.Should().Be(true);
            Uow.Subjects.Read().Count().Should().Be(Generator.Subjects.Count + items.Count);
        }
    }
}
