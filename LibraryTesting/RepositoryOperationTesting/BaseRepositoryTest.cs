using LibraryTesting.DataGenerator;

namespace LibraryTesting.RepositoryOperationTesting;

public class BaseRepositoryTest : BaseTest
{
    public BaseRepositoryTest()
    {
        Generator = new ScheduleDataSetGenerator();
    }

    protected ScheduleDataSetGenerator Generator { get; }
}