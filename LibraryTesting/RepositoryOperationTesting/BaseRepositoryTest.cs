using LibraryTesting.DataGenerator;

namespace LibraryTesting.RepositoryOperationTesting;

public class BaseRepositoryTest : BaseTest
{
    public BaseRepositoryTest()
    {
        Generator = new ScheduleDataSetGenerator();
    }

    protected ScheduleDataSetGenerator Generator { get; }

    protected override void EveryTimeSetUp()
    {
        base.EveryTimeSetUp();

        Generator.Clear();
    }

    protected void CreateUser()
    {
        var user = Generator.RUser();

        Generator.Users.Add(user);
    }

    protected void CreateUsers(int count)
    {
        var users = Generator.RUsers(count);

        Generator.Users.AddRange(users);
    }
}