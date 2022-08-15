using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Entities;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting.Repository;

[TestFixture]
public class CouplesRepoTests : BaseRepositoryTest
{
    [Test]
    public async Task Creation_Successful()
    {
        await LoadRandomDataSet(3);

        var couple = Generator.GenEmptyCouple(1, Uow.Subjects.Read().First()).First();

        var result = await Uow.Couples.Add(couple);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + 1);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        await LoadRandomDataSet(3);

        int COUNT = 4;
        var couples = Generator.GenEmptyCouple(COUNT, new Subject()
        {
            OwnerGroup = Uow.Groups.Read().First()
        });

        var result = await Uow.Couples.AddRange(couples);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + couples.Count);
    }

    [Test]
    public async Task Update_FoundAndUpdateItem_Success()
    {
        await LoadRandomDataSet(3);
        var newProp = DateTime.MinValue;

        var coupleInfo = Generator.Groups.First();
        var couple = await Uow.Couples.ReadById(coupleInfo.Id).FirstOrDefaultAsync();

        couple.Begin = newProp;

        var res = await Uow.Couples.Update(couple);
        Uow.Save();

        couple = await Uow.Couples.ReadById(coupleInfo.Id).FirstOrDefaultAsync();

        res.Should().BeTrue();
        couple.Should().NotBeNull();
        couple.Begin.Should().Be(newProp);
    }

    [Test]
    public async Task Remove_AllDependedListCheck_Removed()
    {
        await LoadRandomDataSet(3);

        var group = Generator.Groups.First();
        var couple = group.Couples.First();

        var res = await Uow.Couples.Delete(couple.Id);
        Uow.Save();

        res.Should().BeTrue();
        (Generator.Couples.Count - 1).Should().Be(Uow.Couples.Read().Count());
        (group.Couples.Count - 1).Should().Be(Uow.Groups.ReadById(group.Id)
            .Include(g => g.Couples).First().Couples.Count);
    }
}