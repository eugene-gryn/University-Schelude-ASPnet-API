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

        modelBuilder.Entity<User>()
            .OwnsOne(user => user.Settings);

        modelBuilder.Entity<UserRole>()
            .HasKey(r => new {r.UserId, r.GroupId});

        modelBuilder.Entity<UserRole>()
            .HasOne(r => r.User)
            .WithMany(u => u.UsersRoles)
            .HasForeignKey(r => r.UserId);
        
        modelBuilder.Entity<UserRole>()
            .HasOne(r => r.Group)
            .WithMany(g => g.UsersRoles)
            .HasForeignKey(r => r.GroupId);

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