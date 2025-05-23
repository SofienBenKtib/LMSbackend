using eduflowbackend.Core.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eduflowbackend.Infrastructure.EntityConfigurations;

public class UserEntityConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(p => p.Id);
        
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.Email).IsRequired().HasMaxLength(50);
        
        builder.Property(x => x.PhoneNumber).HasMaxLength(20);

        builder.Property(x => x.Role).IsRequired();
    }
}