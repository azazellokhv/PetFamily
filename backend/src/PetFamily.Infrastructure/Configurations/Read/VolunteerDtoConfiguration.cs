using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Queries;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey(i => i.VolunteerId);
      
        builder.Property(v => v.VolunteerDetails)
            .HasConversion(
                value => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer
                    .Deserialize<IEnumerable<VolunteerDetailsDto>>(
                        json, JsonSerializerOptions.Default)!);
    }
}