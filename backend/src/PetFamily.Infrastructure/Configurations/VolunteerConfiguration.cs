using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Shared.Ids;

namespace PetFamily.Infrastructure.Configurations;

public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");
        
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.ComplexProperty(v => v.FullName,fn =>
            {
             fn.Property(x => x.LastName)
                 .IsRequired()
                 .HasMaxLength(Constants.MAX_NAME_LENGTH);
             
             fn.Property(x => x.FirstName)
                 .IsRequired()
                 .HasMaxLength(Constants.MAX_NAME_LENGTH);
             
             fn.Property(x => x.Patronymic)
                 .IsRequired()
                 .HasMaxLength(Constants.MAX_NAME_LENGTH);
            });

        builder.ComplexProperty(v => v.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);
        });
        
        builder.ComplexProperty(v => v.WorkExperience, wb =>
        {
            wb.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_WORK_EXPERIENCE);
        });

        builder.ComplexProperty(v => v.PhoneNumber, cb =>
        {
            cb.Property(x => x.Value)
                .IsRequired();
        });

        builder.OwnsOne(v => v.SocialNetworkList, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(x => x.SocialNetworks, sb =>
                {
                    sb.Property(sn => sn.Title)
                        .IsRequired();

                    sb.Property(sn => sn.Link)
                        .IsRequired();
                });
            });
        
        builder.OwnsOne(v => v.VolunteerDetailsList, vb =>
        {
            vb.ToJson();

            vb.OwnsMany(x => x.DetailsForAssistance, db =>
            {
                db.Property(da => da.Title)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_NAME_LENGTH);
                    
                db.Property(da => da.Description)
                    .IsRequired(false)
                    .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);

                db.Property(da => da.ContactPhoneAssistance)
                    .IsRequired();
                    
                db.Property(da => da.BankCardAssistance)
                    .IsRequired(false);

            });
        });
 
        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_Id")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property<bool>("_isDeleted")
            .UsePropertyAccessMode(PropertyAccessMode.Field)
            .HasColumnName("is_deleted");
    }
}