using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.EF;

public class ScheduleContext : DbContext
{
    public ScheduleContext(ContextFactory<ScheduleContext> factory) : base(factory.CreateOptions())
    {
    }
    public ScheduleContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<Couple> Couples { get; set; } = null!;
    public DbSet<Subject> Subjects { get; set; } = null!;
    public DbSet<HomeworkTask> Homework { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Owned user settings
        modelBuilder.Entity<User>()
            .OwnsOne(user => user.Settings);

        modelBuilder.Entity<User>()
            .OwnsOne(u => u.Token);

        modelBuilder.Entity<User>()
            .OwnsOne(u => u.ProfileImage);

        // Set Many-To-Many Groups to Users

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

        // Setting user homework list
        // User has a homework

        modelBuilder.Entity<User>()
            .HasMany(u => u.Homework)
            .WithOne(h => h.User)
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setting Groups couples list

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Couples)
            .WithOne(c => c.Group)
            .HasForeignKey(c => c.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Setting Subjects list

        modelBuilder.Entity<Group>()
            .HasMany(group => group.Subjects)
            .WithOne(subject => subject.OwnerGroup)
            .HasForeignKey(s => s.GroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Couple has a subject

        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Couples)
            .WithOne(c => c.Subject)
            .HasForeignKey(couple => couple.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);

        // Homework has a subject

        modelBuilder.Entity<Subject>()
            .HasMany(s => s.Homework)
            .WithOne(h => h.Subject)
            .HasForeignKey(h => h.SubjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}