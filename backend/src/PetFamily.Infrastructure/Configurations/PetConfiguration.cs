using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Enum;
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

        builder.ComplexProperty(p => p.Nickname, nb =>
        {
            nb.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_NAME_LENGTH);
        });
        
        builder.ComplexProperty(p => p.PetType, pt =>
        {
            pt.Property(bs => bs.BiologicalSpeciesId)
                .HasConversion(
                    id => id.Value,
                    value => BiologicalSpeciesId.Create(value))
                .IsRequired();

            pt.Property(b => b.BreedId)
                .IsRequired();
        });
        
        builder.ComplexProperty(p => p.Description, db =>
        {
            db.Property(x => x.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
        });

        builder.ComplexProperty(p => p.Color, cb =>
        {
            cb.Property(x => x.Value)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_NAME_LENGTH);
        });
        
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

        builder.Property(x => x.AssistanceStatus)
            .IsRequired()
            .HasConversion(
                s => s.ToString(),
                s => (AssistanceStatus)Enum.Parse(typeof(AssistanceStatus), s));
    
        builder.ComplexProperty(p => p.Weight, wb =>
        {
            wb.Property(x => x.Value)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_WEIGHT);
        });
        
        builder.ComplexProperty(p => p.Height, hb =>
        {
            hb.Property(x => x.Value)
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_HEIGHT);
        });

        builder.ComplexProperty(p => p.PhoneNumber, cb =>
        {
            cb.Property(x => x.Value)
                .IsRequired(); 
        });
        
        builder.Property(p => p.IsNeutered)
            .IsRequired();
        
        builder.Property(p => p.Birthday)
            .IsRequired();
        
        builder.Property(p => p.IsVaccinated)
            .IsRequired();
      
        
        builder.ComplexProperty(p => p.DetailForAssistance, d =>
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

            plb.OwnsMany(x => x.PetPhotos, pb =>
            {
                pb.Property(pt => pt.FileName)
                    .HasConversion(
                        p => p.Path,
                        value => FilePath.Create(value).Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_FILENAME_LENGH);

                pb.Property(pt => pt.IsMain)
                    .IsRequired();
            });
        });
        
        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");

    }
}