using Microsoft.EntityFrameworkCore;
using Check.Database.Entities;
using Check.Interfaces;

namespace Check.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Gift> Gifts { get; set; } = null!;
    public DbSet<Transaction> Transactions { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        User admin = new User { Id = Guid.Parse("4326ac10-5919-4262-9614-42df12c80b4e"), Username = "Admin", Role = "Admin", IsVerified = true };
        User user = new User { Id = Guid.Parse("1123ac10-5919-4262-9614-42df12c80b4e"), Username = "Ivan", Role = "User", IsVerified = false };
        Gift gift = new Gift { Id = Guid.Parse("bc00ac10-5919-4262-9614-42df12c80b4e"), Title = "some shit", Price = 100, UserId = admin.Id };

        builder.Entity<User>().HasData(admin, user);
        builder.Entity<Gift>().HasData(gift);
        builder.Entity<Transaction>().HasData(new Transaction
        {
            Id = Guid.Parse("1caaac10-5919-4262-9614-42df12c80b4e"),
            IsCompleted = false,
            UserId = admin.Id,
            GifterId = user.Id,
            GiftId = gift.Id
        });
    }

    #region Modification Tracking

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        OnBeforeSaving();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        OnBeforeSaving();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    /// <summary> Track entites with modification dates </summary>
    private void OnBeforeSaving()
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is ITimestamped baseEntity)
            {
                var now = DateTime.UtcNow;

                switch (entry.State)
                {
                    case EntityState.Modified:
                        baseEntity.UpdatedDate = now;
                        break;

                    case EntityState.Added:
                        baseEntity.CreatedDate = now;
                        baseEntity.UpdatedDate = now;
                        break;
                }
            }
        }
    }

    #endregion
}
