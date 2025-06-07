using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("roles", "Develop");

			builder.HasKey(r => r.Id);
			builder.Property(r => r.Id)
				.HasColumnName("id")
				.IsRequired();

			builder.Property(r => r.RoleId)
				.HasColumnName("roleid")
				.HasComputedColumnSql("('R' || lpad(id::text, 3, '0'::text))", stored: true)
				.IsRequired()
				.ValueGeneratedOnAddOrUpdate();

			builder.HasIndex(r => r.RoleId)
				.IsUnique();

			builder.Property(r => r.RoleName)
				.HasColumnName("rolename")
				.HasMaxLength(20)
				.IsRequired();

			builder.Property(r => r.Description)
				.HasColumnName("description")
				.IsRequired(false);

			builder.Property(r => r.CreatedAt)
				.HasColumnName("createdat")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(r => r.ModifiedAt)
				.HasColumnName("modifiedat")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}
