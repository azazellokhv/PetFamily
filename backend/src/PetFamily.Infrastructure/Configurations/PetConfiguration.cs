using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");
        
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));
        
        builder.Property(p => p.Nickname)
            .IsRequired()
            .HasMaxLength(Constants.MAX_NAME_LENGTH);

        builder.ComplexProperty(p => p.PetType, pt =>
        {
            pt.Property(bs => bs.BiologicalSpeciesId)
                .HasConversion(
                    id => id.Value,
                    value => BiologicalSpeciesId.Create(value))
                .IsRequired();

            pt.Property(b => b.BreedId)
                .HasConversion(
                    id => id.Value,
                    value => BreedId.Create(value))
                .IsRequired();
        });
        
        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);

        builder.Property(p => p.ColorPet)
            .IsRequired()
            .HasMaxLength(Constants.MAX_NAME_LENGTH);
        
        builder.ComplexProperty(p => p.Health, h =>
            {
                h.Property(x => x.IsHealthy)
                 .IsRequired();
                
                h.Property(x => x.DescriptionDisease)
                 .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
            });

        builder.ComplexProperty(p => p.Address, a =>
            {
                a.Property(x => x.Country)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);
                
                a.Property(x => x.Locality)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                
                a.Property(x => x.Street)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                
                a.Property(x => x.BuildingNumber)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                
                a.Property(x => x.Comments)
                    .IsRequired(false)
                    .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
            });

        builder.ComplexProperty(p => p.AssistanceStatus, a =>
            {
                a.Property(x => x.Title)
                    .IsRequired();
            });
    
        builder.Property(p => p.Weight)
            .IsRequired();

        builder.Property(p => p.Height)
            .IsRequired();
        
        builder.Property(p => p.ContactPhone)
            .IsRequired();
        
        builder.Property(p => p.IsNeutered)
            .IsRequired();
        
        builder.Property(p => p.Birthday)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();
      
        
        builder.ComplexProperty(p => p.DetailsForAssistance, d =>
        {
            d.Property(da => da.Title)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);
                    
            d.Property(da => da.Description)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);

            d.Property(da => da.ContactPhoneAssistance)
                .IsRequired();
                    
            d.Property(da => da.BankCardAssistance)
                .IsRequired(false);
        });

        builder.Property(p => p.DateOfCreation)
            .IsRequired();

        builder.OwnsOne(p => p.PetPhotoList, plb =>
        {
            plb.ToJson();

            plb.OwnsMany(x => x.Photos, pb =>
            {
                pb.Property(pt => pt.FileName)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_FILENAME_LENGH);

                pb.Property(pt => pt.IsMain)
                    .IsRequired();
            });
        });

    }
}