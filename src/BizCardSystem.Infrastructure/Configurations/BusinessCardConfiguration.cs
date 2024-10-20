using BizCardSystem.Domain.BusinessCards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BizCardSystem.Infrastructure.Configurations;

public sealed class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
{
    public void Configure(EntityTypeBuilder<BusinessCard> builder)
    {
        builder.HasIndex(businessCard => businessCard.Id);
        builder.HasIndex(businessCard => businessCard.Email);

        builder.Property(businessCard => businessCard.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(businessCard => businessCard.Email)
               .IsRequired()
               .HasMaxLength(150);

        builder.OwnsOne(businessCard => businessCard.Address);

        builder.Property(businessCard => businessCard.Gender)
               .IsRequired()
               .HasConversion<string>();

        builder.Property(businessCard => businessCard.DateofBirth)
               .IsRequired()
               .HasColumnType("date");

        builder.Property(businessCard => businessCard.Phone)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(businessCard => businessCard.Photo)
               .HasMaxLength(1048576)
               .IsRequired(false);

        builder.HasIndex(businessCard => businessCard.Phone);
    }
}
