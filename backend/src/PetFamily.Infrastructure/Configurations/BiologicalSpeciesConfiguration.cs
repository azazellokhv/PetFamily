using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.BiologicalSpeciesManagement.AggregateRoot;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class BiologicalSpeciesConfiguration : IEntityTypeConfiguration<BiologicalSpecies>
{
    public void Configure(EntityTypeBuilder<BiologicalSpecies> builder)
    {
        builder.ToTable("biological_species");
        
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BiologicalSpeciesId.Create(value));
        
        builder.Property(b => b.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_NAME_LENGTH);
        
        builder.HasMany(v => v.Breeds)
            .WithOne()
            .HasForeignKey("biological_species_id")
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
        
    }
}

