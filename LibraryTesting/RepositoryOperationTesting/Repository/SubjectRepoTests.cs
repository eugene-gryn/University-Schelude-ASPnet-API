using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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

        [Test]
        public async Task Update_FoundAndUpdateItem_Success()
        {
            await LoadRandomDataSet(3);
            var newProp = "Subject Name: Juvava";

            var subjectInfo = Generator.Groups.First();
            var subject = await Uow.Subjects.ReadById(subjectInfo.Id).FirstOrDefaultAsync();

            subject.Name = newProp;

            var res = await Uow.Subjects.Update(subject);
            Uow.Save();

            subject = await Uow.Subjects.ReadById(subjectInfo.Id).FirstOrDefaultAsync();

            res.Should().BeTrue();
            subject.Should().NotBeNull();
            subject.Name.Should().Be(newProp);
        }

        [Test]
        public async Task Remove_AllDependedListCheck_Removed()
        {
            await LoadRandomDataSet(3);

            var group = Generator.Groups.First();
            var subject = group.Subjects.First();

            var res = await Uow.Subjects.Delete(subject.Id);
            Uow.Save();

            res.Should().BeTrue();
            (Generator.Subjects.Count - 1).Should().Be(Uow.Subjects.Read().Count());
            (group.Subjects.Count - 1).Should().Be(Uow.Groups.ReadById(group.Id)
                .Include(g => g.Subjects).First().Subjects.Count);
        }
    }
}
