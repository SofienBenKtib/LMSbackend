using eduflowbackend.Core.Resource;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eduflowbackend.Infrastructure.EntityConfigurations;

public class ResourceEntityConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Title)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Description)
            .HasMaxLength(50);
        
        builder.HasOne(resource => resource.Creator)
            .WithMany()
            .HasForeignKey(resource => resource.CreatorId);
        
        builder.HasOne(resource => resource.Session)
            .WithMany()
            .HasForeignKey(resource => resource.SessionId);
    }
}