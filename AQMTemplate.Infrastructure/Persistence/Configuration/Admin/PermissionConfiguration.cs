using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin
{
	public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
	{
		public void Configure(EntityTypeBuilder<Permission> builder)
		{
			builder.ToTable("permissions", "Develop");

			builder.HasKey(p => p.Id);
			builder.Property(p => p.Id)
				.HasColumnName("id")
				.IsRequired();

			builder.Property(p => p.PermissionId)
				.HasColumnName("permissionid")
				.HasComputedColumnSql("('P' || lpad(id::text, 2, '0'::text))", stored: true)
				.IsRequired()
				.ValueGeneratedOnAddOrUpdate();

			builder.HasIndex(p => p.PermissionId)
				.IsUnique();

			builder.Property(p => p.PermissionName)
				.HasColumnName("permissionname")
				.HasMaxLength(100)
				.IsRequired();

			builder.Property(p => p.Description)
				.HasColumnName("description")
				.IsRequired(false);

			builder.Property(p => p.CreatedAt)
				.HasColumnName("createdat")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(p => p.ModifiedAt)
				.HasColumnName("modifiedat")
				.IsRequired()
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}
