using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entities;

namespace Portfolio.Infrastructure.Persistence.Configurations;

public class ProjectTechnologyConfiguration : IEntityTypeConfiguration<ProjectTechnology>
{
    public void Configure(EntityTypeBuilder<ProjectTechnology> builder)
    {
        builder.ToTable("ProjectTechnologies");

        builder.HasKey(pt => pt.Id);

        builder.Property(pt => pt.Id)
            .ValueGeneratedNever();

        builder.Property(pt => pt.ProjectId)
            .IsRequired();

        builder.Property(pt => pt.TechnologyId)
            .IsRequired();

        builder.HasIndex(pt => new { pt.ProjectId, pt.TechnologyId })
            .IsUnique()
            .HasDatabaseName("IX_ProjectTechnologies_ProjectId_TechnologyId");

        builder.Property(pt => pt.AssignedAt)
            .IsRequired();

        builder.Property(pt => pt.Notes)
            .HasMaxLength(1000);

        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(pt => pt.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Technology>()
            .WithMany()
            .HasForeignKey(pt => pt.TechnologyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}