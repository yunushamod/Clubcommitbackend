using System.Reflection;
using Commit.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
namespace Commit.Data;

public class ApplicationDbContext(IConfiguration configuration) : DbContext
{
    public required DbSet<ApplicationUser> ApplicationUsers { get; init; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DBConnection"),
        options => 
        {
            options.MigrationsHistoryTable("Commit_EFMigrationHistory");
            options.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), null);
        });
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if ((Nullable.GetUnderlyingType(property.ClrType) ?? property.ClrType).IsEnum)
                {
                    property.SetProviderClrType(typeof(string));
                }
                if (property.ClrType == typeof(decimal) &&
                    property.Name.Contains("Price", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetPrecision(11);
                    property.SetScale(2);
                }

                if (property.ClrType == typeof(DateTime) &&
                    property.Name.Contains("DateCreated", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetDefaultValueSql("GETUTCDATE()");
                }

                if (property.ClrType == typeof(decimal) &&
                    property.Name.Contains("Interest", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetPrecision(5);
                    property.SetScale(2);
                }

                if (property.ClrType == typeof(string) &&
                    property.Name.Contains("Url", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetIsUnicode(false);
                    property.SetMaxLength(500);
                }

                if (property.ClrType == typeof(decimal) &&
                    property.Name.Contains("Percentage", StringComparison.InvariantCultureIgnoreCase))
                {
                    property.SetPrecision(5);
                    property.SetScale(2);
                }
                if (property.ClrType == typeof(decimal) &&
                    (property.Name.Contains("Amount", StringComparison.InvariantCultureIgnoreCase)
                    || property.Name.Contains("Balance", StringComparison.InvariantCultureIgnoreCase)))
                {
                    property.SetPrecision(11);
                    property.SetScale(2);
                }
            }
        }
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}