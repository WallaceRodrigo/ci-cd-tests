using Crawlers.Core.Shared.Domain;
using Microsoft.EntityFrameworkCore;

namespace Crawlers.Core.Shared;

public class JobDatabaseContext : DbContext
{
    /// <inheritdoc />
    public JobDatabaseContext(DbContextOptions<JobDatabaseContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        SavingChanges += OnSavingChanges;
    }
    
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Technology> Technologies => Set<Technology>();
    public DbSet<Role> Roles => Set<Role>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Job>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ExternalId).IsRequired();
            e.Property(a => a.Description).IsRequired();
            e.Property(a => a.ExternalLink).IsRequired();
            e.Property(a => a.Title).IsRequired();
            e.Property(a => a.PostedAt);
            e.Property(a => a.Source)
                .IsRequired()
                .HasConversion(
                    v => v.ToString(),
                    v => (Source)Enum.Parse(typeof(Source), v));

            e.Property(a => a.Seniority)
                .HasConversion(
                    v => v.ToString(),
                    v => (Seniority)Enum.Parse(typeof(Seniority), v));
            
            e.HasMany(a => a.Technologies)
                .WithMany();
            
            e.HasIndex(a => a.ExternalId);
            e.HasIndex(a => a.Title);
        });
        
        modelBuilder.Entity<Technology>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ExternalId).IsRequired();
            e.Property(a => a.Name).IsRequired();

            e.HasIndex(a => a.ExternalId);
            e.HasIndex(a => a.Name);
        });
        
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.ExternalId).IsRequired();
            e.Property(a => a.Name).IsRequired();
            
            e.HasIndex(a => a.ExternalId);
            e.HasIndex(a => a.Name);
        });
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs args)
    {
        var modifiedEntries = ChangeTracker.Entries()
            .Where(x => x.State is EntityState.Added or EntityState.Modified);

        var identityName = Thread.CurrentPrincipal?.Identity?.Name;
        var now = DateTime.UtcNow;

        foreach (var entry in modifiedEntries)
        {
            if (entry.Entity is not MutableEntity entity) continue;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedBy = identityName;
                entity.CreatedAt = now;
            }

            if (entry.State == EntityState.Modified)
            {
                entity.LastUpdatedAt = now;
                entity.LastUpdatedBy = identityName;
            }
        }
    }
}