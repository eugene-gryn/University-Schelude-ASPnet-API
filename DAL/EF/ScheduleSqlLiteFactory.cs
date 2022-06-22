using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ScheduleSqlLiteFactory : ContextFactory<ScheduleContext>
{
    public ScheduleSqlLiteFactory(string databaseName) : base(databaseName)
    {
        var folderPath = Environment.CurrentDirectory;
        var databasePath = Path.Join(folderPath, databaseName);

        ConnectionString = $"Data Source={databasePath}";
    }

    public override DbContextOptions<ScheduleContext> CreateOptions()
    {
        var builder = new DbContextOptionsBuilder<ScheduleContext>();

        builder.UseSqlite(ConnectionString);

        return builder.Options;
    }
}