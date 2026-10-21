using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UserManagement.Domain.Entities;

namespace UserManagement.Infra.Data.Mappings
{
    public class UserMapping : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder) {

            builder.ToTable("users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).ValueGeneratedNever();

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(u => u.Cpf)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false);

            builder.Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(u => u.Status)
                .IsRequired();

            builder.HasIndex(u => u.Email)
                .IsUnique();

            builder.HasIndex(u => u.Cpf)
                .IsUnique();
        }
    }
}
