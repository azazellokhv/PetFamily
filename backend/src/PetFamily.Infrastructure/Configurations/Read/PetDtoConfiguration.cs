using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.DTOs.Queries;
using PetFamily.Domain.PetManagement.ValueObjects;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(v => v.PetId);
        
        builder.Property(x => x.PetPhotos)
            .HasConversion(
                petPhotos => JsonSerializer
                    .Serialize(string.Empty, JsonSerializerOptions.Default),
                json => JsonSerializer.Deserialize<IEnumerable<PetPhoto>>(
                    json, JsonSerializerOptions.Default)!
                    .Select(p => new PetPhotoDto
                    {
                        FilePath = p.FilePath.ToString(),
                        IsMain = p.IsMain,
                    }).ToList());
    }
}
