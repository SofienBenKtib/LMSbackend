using eduflowbackend.Core.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eduflowbackend.Infrastructure.EntityConfigurations;

public class SessionEntityConfiguration : IEntityTypeConfiguration<Session>
{
    public void Configure(EntityTypeBuilder<Session> builder)
    {
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Title).HasMaxLength(50).IsRequired();
        
        builder.Property(x => x.Description).HasMaxLength(200).IsRequired();
        
        builder.Property(x => x.StartDate).IsRequired();
        
        builder.HasOne(x => x.Instructor)
            .WithMany()
            .HasForeignKey(x => x.InstructorId);
    }
}