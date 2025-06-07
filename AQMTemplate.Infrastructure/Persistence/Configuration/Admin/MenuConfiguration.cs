using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
	public void Configure(EntityTypeBuilder<Menu> builder)
	{
		builder.ToTable("menus");

		builder.HasKey(m => m.Id);
		builder.Property(m => m.Id)
			.HasColumnName("id")
			.IsRequired();

		builder.Property(m => m.MenuId)
			.HasColumnName("menuid")
			.HasComputedColumnSql("('M' || lpad(id::text, 3, '0'::text))", stored: true)
			.IsRequired()
			.ValueGeneratedOnAddOrUpdate();

		builder.HasIndex(m => m.MenuId)
			.IsUnique();

		builder.Property(m => m.ParentMenuId)
			.HasColumnName("parentmenuid")
			.IsRequired(false);

		builder.HasOne(m => m.ParentMenu)
			.WithMany(m => m.ChildMenus)
			.HasForeignKey(m => m.ParentMenuId)
			.HasPrincipalKey(m => m.MenuId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(m => m.MenuName)
			.HasColumnName("menuname")
			.HasMaxLength(100)
			.IsRequired();

		builder.Property(m => m.Icon)
			.HasColumnName("icon")
			.HasMaxLength(100)
			.IsRequired(false);

		builder.Property(m => m.Url)
			.HasColumnName("url")
			.HasMaxLength(200)
			.IsRequired(false);

		builder.Property(x => x.PermissionId)
				.HasColumnName("permissionid")
				.HasMaxLength(100)
				.IsRequired(false);
		
		builder.HasOne(m => m.Permission)
			.WithMany(p => p.Menus)
			.HasForeignKey(m => m.PermissionId)
			.HasPrincipalKey(p => p.PermissionId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.Property(x => x.SortOrder)
			.HasColumnName("sortorder")
			.HasColumnType("integer")
			.HasDefaultValue(0);

		builder.Property(x => x.IsVisible)
			.HasColumnName("isvisible")
			.IsRequired()
			.HasDefaultValue(true);

		builder.Property(x => x.CreatedAt)
			.HasColumnName("createdat")
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

		builder.Property(x => x.ModifiedAt)
			.HasColumnName("modifiedat")
			.IsRequired()
			.HasDefaultValueSql("CURRENT_TIMESTAMP");

	}
}
