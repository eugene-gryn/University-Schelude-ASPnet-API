using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ScheduleContext : DbContext
{
    public ScheduleContext(ContextFactory<ScheduleContext> factory) : base(factory.CreateOptions())
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Couple> Couples { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<HomeworkTask> Homework { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<User>()
        //    .HasOne<Settings>()
        //    .WithOne()
        //    .HasForeignKey<Settings>(settings => settings.SettingsId)
        //    .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .OwnsOne(user => user.Settings);

        modelBuilder.Entity<User>()
            .HasMany(user => user.Groups)
            .WithOne(group => group.Creator)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany<HomeworkTask>()
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Couples)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Subjects)
            .WithOne(subject => subject.OwnerGroup)
            .OnDelete(DeleteBehavior.Cascade);
    }
}