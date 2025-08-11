using Commit.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace Commit.Data.ModelConfigurations;

public class ApplicationUserConfig : IEntityTypeConfiguration<ApplicationUser>
{

    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasIndex(x => x.NormalizedEmailAddress).IsUnique();
        builder.HasIndex(x => x.NormalizedPhoneNumber).IsUnique();
    }
}