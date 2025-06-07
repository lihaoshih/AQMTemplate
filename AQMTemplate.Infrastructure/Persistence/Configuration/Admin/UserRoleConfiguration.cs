using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin
{
	public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
	{
		public void Configure(EntityTypeBuilder<UserRole> builder)
		{
			builder.ToTable("userroles", "Develop");
			
			builder.HasKey(r => new { r.UserId, r.RoleId });
			
			builder.HasOne(ur => ur.User)
				.WithMany(u => u.UserRoles)
				.HasForeignKey(ur => ur.UserId)
				.HasPrincipalKey(u => u.UserId)
				.OnDelete(DeleteBehavior.Cascade);
			
			builder.HasOne(ur => ur.Role)
				.WithMany(r => r.UserRoles)
				.HasForeignKey(ur => ur.RoleId)
				.HasPrincipalKey(r => r.RoleId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
