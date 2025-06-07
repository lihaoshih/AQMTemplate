using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin
{
	public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
	{
		public void Configure(EntityTypeBuilder<RolePermission> builder)
		{
			builder.ToTable("rolepermissions", "Develop");

			builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

			builder.HasOne(rp => rp.Role)
				.WithMany(r => r.RolePermissions)
				.HasForeignKey(rp => rp.RoleId)
				.HasPrincipalKey(r => r.RoleId)
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.HasOne(rp => rp.Permission)
				.WithMany(p => p.RolePermissions)
				.HasForeignKey(rp => rp.PermissionId)
				.HasPrincipalKey(p => p.PermissionId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
