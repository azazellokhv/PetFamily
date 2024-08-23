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
        
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_DESCRIPTION_LENGTH);

        builder.Property(v => v.WorkExperience)
            .IsRequired()
            .HasMaxLength(Constants.MAX_WORK_EXPERIENCE);

        builder.Property(v => v.ContactPhone)
            .IsRequired();

        builder.OwnsOne(v => v.VolunteerDetails, vb =>
            {
                vb.ToJson();

                vb.OwnsMany(x => x.SocialNetworks, sb =>
                {
                    sb.Property(sn => sn.Title)
                        .IsRequired();

                    sb.Property(sn => sn.Link)
                        .IsRequired();
                });

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
            .HasForeignKey("volunteer_Id");
    }
}