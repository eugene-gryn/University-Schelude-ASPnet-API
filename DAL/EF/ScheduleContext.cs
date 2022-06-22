using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ScheduleContext : DbContext
{
    public ScheduleContext(DbContextOptions<ScheduleContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Couple> Couples { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Homework> Homeworks { get; set; }

    private void UserModelBuild(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne<Settings>();

        modelBuilder.Entity<User>()
            .HasMany<Group>()
            .WithOne(group => group.Creator);

        modelBuilder.Entity<User>()
            .HasMany<Group>()
            .WithMany(group => group.Moderators);

        modelBuilder.Entity<User>()
            .HasMany<Group>()
            .WithMany(group => group.Users);


        modelBuilder.Entity<User>()
            .HasMany<Homework>()
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);
    }

    private void GroupModelBuild(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>()
            .HasMany(group => group.Couples)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Subjects)
            .WithOne(subject => subject.OwnerGroup)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Group>()
            .HasOne(group => group.Creator)
            .WithMany(user => user.Groups);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Moderators)
            .WithMany(user => user.Groups);

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Users)
            .WithMany(user => user.Groups);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        UserModelBuild(modelBuilder);

        GroupModelBuild(modelBuilder);

        // Couple

        modelBuilder.Entity<Couple>()
            .HasOne<Subject>();

        // Subject

        modelBuilder.Entity<Subject>()
            .HasOne<Group>();

        // Homework

        modelBuilder.Entity<Homework>()
            .HasOne<Subject>();
    }
}