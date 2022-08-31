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
        await GenerateRandomDataSet(3);

        var couple = Generator.GenEmptyCouple(Uow.Subjects.Read().First(),
            Uow.Subjects.Read().First().OwnerGroup, 1).First();

        var result = await Uow.Couples.Add(couple);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + 1);
    }
    [Test]
    public async Task RangeCreation_Successful()
    {
        await GenerateRandomDataSet(3);

        int COUNT = 4;

        var couples = Generator.GenEmptyCouple(Uow.Groups.Read().Include(g => g.Subjects).First().Subjects.First(),
            Uow.Groups.Read().First(), COUNT);

        var result = await Uow.Couples.AddRange(couples);
        Uow.Save();

        result.Should().Be(true);
        Uow.Couples.Read().Count().Should().Be(Generator.Couples.Count + couples.Count);
    }

    [Test]
    public async Task Update_FoundAndUpdateItem_Success()
    {
        await GenerateRandomDataSet(3);
        var newProp = DateTime.MinValue;

        var coupleInfo = Generator.Groups.First();
        var couple = await Uow.Couples.ReadById(coupleInfo.Id).FirstOrDefaultAsync();

        couple.Begin = newProp;

        var res = await Uow.Couples.UpdateAsync(couple);
        Uow.Save();

        couple = await Uow.Couples.ReadById(coupleInfo.Id).FirstOrDefaultAsync();

        res.Should().BeTrue();
        couple.Should().NotBeNull();
        couple.Begin.Should().Be(newProp);
    }

    [Test]
    public async Task Remove_AllDependedListCheck_Removed()
    {
        await GenerateRandomDataSet(3);

        var couplesCount = Uow.Couples.Read().Count();
        var firstGroupFirstCouple = await Uow.Groups.ReadById(1)
            .Include(g => g.Couples)
            .SingleOrDefaultAsync();


        var res = await Uow.Couples.Delete(firstGroupFirstCouple!.Id);
        Uow.Save();

        res.Should().BeTrue();
        (couplesCount - 1).Should().Be(Uow.Couples.Read().Count());
    }

    [Test]
    public async Task RemoveAll_CouplesRemovedFromGroup_Successful() {
        await GenerateRandomDataSet();

        var group = await Uow.Groups.ReadById(1)
            .Include(c => c.Couples)
            .SingleOrDefaultAsync();
        var couplesRemoveCount = group!.Couples.Count;
        var couplesCount = Uow.Couples.Read().Count();

        await Uow.Couples.RemoveAll(group!.Id);
        Uow.Save();

        group.Should().NotBeNull();
        Uow.Groups.ReadById(group.Id).Include(g => g.Couples).SingleOrDefault()!.Couples.Count.Should().Be(0);
        Uow.Couples.Read().Count().Should().Be(couplesCount - couplesRemoveCount);
    }
}