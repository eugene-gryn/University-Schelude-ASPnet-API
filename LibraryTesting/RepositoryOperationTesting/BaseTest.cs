using DAL.EF;
using DAL.UOW;
using LibraryTesting.DataGenerator.OptionsFactory;
using Moq;
using NUnit.Framework;

namespace LibraryTesting.RepositoryOperationTesting;

public class BaseTest
{
    private IUnitOfWork _uow;

    public BaseTest()
    {
        _uow = new EfUnitOfWork(new ScheduleContext(new ScheduleInMemoryDbFactory()));
    }

    public IUnitOfWork Uow => _uow;

    [SetUp]
    protected void EveryTimeSetUp()
    {
        var context = new ScheduleContext(new ScheduleInMemoryDbFactory());
        _uow = new EfUnitOfWork(context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    [TearDown]
    protected void EveryTimeTearDown()
    {
        _uow.Dispose();
    }

    protected void UowUpdate()
    {
        _uow.Dispose();

        _uow = new EfUnitOfWork(new ScheduleContext(new ScheduleInMemoryDbFactory()));
    }
}