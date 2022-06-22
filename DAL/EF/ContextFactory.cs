using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public abstract class ContextFactory<TDbContext>
    where TDbContext : DbContext
{
    public string ConnectionString { get; protected set; }

    protected ContextFactory(string connectionString)
    {
        ConnectionString = connectionString;
    }

    public abstract DbContextOptions<TDbContext> CreateOptions();
}