using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;

namespace AQMTemplate.Infrastructure.Persistence
{
	public class AppDbContext : DbContext
	{
		public AppDbContext() { }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{ }

		public DbSet<User> Users => Set<User>();
		public DbSet<Role> Roles => Set<Role>();
		public DbSet<Permission> Permissions => Set<Permission>();
		public DbSet<Menu> Menus => Set<Menu>();
		public DbSet<UserRole> UserRoles => Set<UserRole>();
		public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return await base.SaveChangesAsync(cancellationToken);
		}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	modelBuilder.ApplyConfiguration(new UserConfiguration());
		//	modelBuilder.ApplyConfiguration(new RoleConfiguration());
		//	modelBuilder.ApplyConfiguration(new PermissionConfiguration());
		//	modelBuilder.ApplyConfiguration(new MenuConfiguration());
		//	modelBuilder.ApplyConfiguration(new UserRoleConfiguration());
		//	modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());

		//	base.OnModelCreating(modelBuilder);
		//}

		//protected override void OnModelCreating(ModelBuilder modelBuilder)
		//{
		//	modelBuilder.Entity<Menu>(entity =>
		//	{
		//		entity.HasKey(e => e.Id).HasName("menus_pkey");

		//		entity.ToTable("menus", "develop");

		//		entity.HasIndex(e => e.MenuId, "menus_menuid_key").IsUnique();

		//		entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
		//		entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("createdat");
		//		entity.Property(e => e.Icon).HasMaxLength(100).HasColumnName("icon");
		//		entity.Property(e => e.IsVisible).HasDefaultValue(true).HasColumnName("isvisible");
		//		entity.Property(e => e.MenuId).HasComputedColumnSql("('M'::text || lpad((id)::text, 3, '0'::text))", true).HasColumnName("menuid");
		//		entity.Property(e => e.MenuName).HasMaxLength(100).HasColumnName("menuname");
		//		entity.Property(e => e.ModifiedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("modifiedat");
		//		entity.Property(e => e.ParentMenuId).HasColumnName("parentmenuid");
		//		entity.Property(e => e.PermissionId).HasMaxLength(100).HasColumnName("permissionid");
		//		entity.Property(e => e.SortOrder).HasDefaultValue(0).HasColumnName("sortorder");
		//		entity.Property(e => e.Url).HasMaxLength(200).HasColumnName("url");

		//		entity.HasOne(d => d.ParentMenu).WithMany(p => p.ChildMenus)
		//			.HasPrincipalKey(p => p.MenuId)
		//			.HasForeignKey(d => d.ParentMenuId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("menus_parentmenuid_fkey");

		//		entity.HasOne(d => d.Permission).WithMany(p => p.Menus)
		//			.HasPrincipalKey(p => p.PermissionId)
		//			.HasForeignKey(d => d.PermissionId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("menus_permissionid_fkey");
		//	});

		//	modelBuilder.Entity<Permission>(entity =>
		//	{
		//		entity.HasKey(e => e.Id).HasName("permissions_pkey");

		//		entity.ToTable("permissions", "develop");

		//		entity.HasIndex(e => e.PermissionId, "permissions_permissionid_key").IsUnique();

		//		entity.HasIndex(e => e.PermissionName, "permissions_permissionname_key").IsUnique();

		//		entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
		//		entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("createdat");
		//		entity.Property(e => e.Description).HasColumnName("description");
		//		entity.Property(e => e.ModifiedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("modifiedat");
		//		entity.Property(e => e.PermissionId).HasComputedColumnSql("('P'::text || lpad((id)::text, 2, '0'::text))", true).HasColumnName("permissionid");
		//		entity.Property(e => e.PermissionName).HasMaxLength(100).HasColumnName("permissionname");
		//	});

		//	modelBuilder.Entity<Role>(entity =>
		//	{
		//		entity.HasKey(e => e.Id).HasName("roles_pkey");

		//		entity.ToTable("roles", "develop", tb => tb.HasComment("使用者角色"));

		//		entity.HasIndex(e => e.RoleId, "roles_roleid_key").IsUnique();

		//		entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
		//		entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("createdat");
		//		entity.Property(e => e.Description).HasColumnName("description");
		//		entity.Property(e => e.ModifiedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("modifiedat");
		//		entity.Property(e => e.RoleId).HasComputedColumnSql("('R'::text || lpad((id)::text, 3, '0'::text))", true).HasColumnName("roleid");
		//		entity.Property(e => e.RoleName).HasMaxLength(20).HasColumnName("rolename");
		//	});

		//	modelBuilder.Entity<User>(entity =>
		//	{
		//		entity.HasKey(e => e.Id).HasName("users_pkey");

		//		entity.ToTable("users", "develop");

		//		entity.HasIndex(e => e.Account, "unique_account").IsUnique();

		//		entity.HasIndex(e => e.UserId, "users_userid_key").IsUnique();

		//		entity.Property(e => e.Id).ValueGeneratedNever().HasColumnName("id");
		//		entity.Property(e => e.Account).HasMaxLength(20).HasColumnName("account");
		//		entity.Property(e => e.Alias).HasMaxLength(20).HasColumnName("alias");
		//		entity.Property(e => e.Annotation).HasColumnName("annotation");
		//		entity.Property(e => e.AvatarUrl).HasMaxLength(100).HasColumnName("avatarurl");
		//		entity.Property(e => e.Birthday).HasColumnName("birthday");
		//		entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("createdat");
		//		entity.Property(e => e.Department).HasMaxLength(20).HasColumnName("department");
		//		entity.Property(e => e.DisplayName).HasMaxLength(20).HasColumnName("displayname");
		//		entity.Property(e => e.Email).HasMaxLength(40).HasColumnName("email");
		//		entity.Property(e => e.FailedLoginAttempt).HasDefaultValue(0).HasColumnName("failedloginattempt");
		//		entity.Property(e => e.Gender).HasMaxLength(1).IsFixedLength().HasColumnName("gender");
		//		entity.Property(e => e.IdNumber).HasColumnName("idnumber");
		//		entity.Property(e => e.IsActive).HasDefaultValue(true).HasColumnName("isactive");
		//		entity.Property(e => e.IsLocked).HasDefaultValue(false).HasColumnName("islocked");
		//		entity.Property(e => e.JobTitle).HasMaxLength(20).HasColumnName("jobtitle");
		//		entity.Property(e => e.LastLogin).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("lastlogin");
		//		entity.Property(e => e.MailingAddress).HasColumnName("mailingaddress");
		//		entity.Property(e => e.ModifiedAt).HasDefaultValueSql("CURRENT_TIMESTAMP").HasColumnType("timestamp without time zone").HasColumnName("modifiedat");
		//		entity.Property(e => e.PasswordHash).HasColumnName("passwordhash");
		//		entity.Property(e => e.PasswordSalt).HasColumnName("passwordsalt");
		//		entity.Property(e => e.Phone).HasMaxLength(20).HasColumnName("phone");
		//		entity.Property(e => e.PostalCode).HasMaxLength(6).HasColumnName("postalcode");
		//		entity.Property(e => e.UserId).HasComputedColumnSql("('U'::text || lpad((id)::text, 4, '0'::text))", true).HasColumnName("userid");
		//		entity.Property(e => e.UserName).HasMaxLength(10).HasColumnName("username");
		//	});

		//	modelBuilder.Entity<UserRole>(entity =>
		//	{
		//		entity.HasKey(e => new { e.UserId, e.RoleId }).HasName("userroles_pkey");
		//		entity.ToTable("userroles", "develop");

		//		entity.HasOne(ur => ur.User)
		//			.WithMany(u => u.UserRoles)
		//			.HasForeignKey(ur => ur.UserId)
		//			.HasPrincipalKey(u => u.UserId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("userroles_userid_fkey");

		//		entity.HasOne(ur => ur.Role)
		//			.WithMany(r => r.UserRoles)
		//			.HasForeignKey(ur => ur.RoleId)
		//			.HasPrincipalKey(r => r.RoleId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("userroles_roleid_fkey");
		//	});

		//	modelBuilder.Entity<RolePermission>(entity =>
		//	{
		//		entity.HasKey(e => new { e.RoleId, e.PermissionId }).HasName("rolepermissions_pkey");
		//		entity.ToTable("rolepermissions", "develop");

		//		entity.HasOne(rp => rp.Role)
		//			.WithMany(r => r.RolePermissions)
		//			.HasForeignKey(rp => rp.RoleId)
		//			.HasPrincipalKey(r => r.RoleId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("rolepermissions_roleid_fkey");

		//		entity.HasOne(rp => rp.Permission)
		//			.WithMany(p => p.RolePermissions)
		//			.HasForeignKey(rp => rp.PermissionId)
		//			.HasPrincipalKey(p => p.PermissionId)
		//			.OnDelete(DeleteBehavior.Cascade)
		//			.HasConstraintName("rolepermission_permissionid_fkey");
		//	});

		//	//OnModelCreatingPartial(modelBuilder);
		//}

		////partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.HasDefaultSchema("Admin");

			modelBuilder.Entity<User>(entity =>
			{
				entity.HasKey(u => new { u.Id, u.UserId });
				entity.Property(u => u.UserId)
					.HasComputedColumnSql("CONVERT([varchar](5), concat('U', right('0000' + CONVERT([varchar](4), [Id]), 4)))")
					.ValueGeneratedOnAddOrUpdate();
				entity.HasIndex(e => e.UserId).IsUnique();
				entity.Property(e => e.IsActive).HasDefaultValue(true);
				entity.Property(e => e.IsLocked).HasDefaultValue(false);
				entity.Property(e => e.FailedLoginAttempt).HasDefaultValue(0);
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("sysdatetime()");
				entity.Property(e => e.ModifiedAt).HasDefaultValueSql("sysdatetime()");
			});

			modelBuilder.Entity<Role>(entity =>
			{
				entity.HasKey(r => new { r.Id, r.RoleId });
				entity.Property(r => r.RoleId)
					.HasComputedColumnSql("CONVERT([varchar](4), concat('R', right('000' + CONVERT([varchar](3), [Id]), 3)))")
					.ValueGeneratedOnAddOrUpdate();
				entity.HasIndex(r => r.RoleId).IsUnique();
				entity.Property(r => r.CreatedAt).HasDefaultValueSql("sysdatetime()");
				entity.Property(r => r.ModifiedAt).HasDefaultValueSql("sysdatetime()");
			});

			modelBuilder.Entity<Permission>(entity =>
			{
				entity.HasKey(p => new { p.Id, p.PermissionId });
				entity.Property(p => p.PermissionId)
					.HasComputedColumnSql("CONVERT([varchar](4), concat('P', right('000' + CONVERT([varchar](3), [Id]), 3)))")
					.ValueGeneratedOnAddOrUpdate();
				entity.HasIndex(p => p.PermissionId).IsUnique();
				entity.Property(p => p.CreatedAt).HasDefaultValueSql("sysdatetime()");
				entity.Property(p => p.ModifiedAt).HasDefaultValueSql("sysdatetime()");
			});

			modelBuilder.Entity<UserRole>(entity =>
			{
				entity.HasKey(ur => new { ur.UserId, ur.RoleId });

				entity.HasOne(u => u.User)
					.WithMany(ur => ur.UserRoles)
					.HasForeignKey(ur => ur.UserId)
					.HasPrincipalKey(u => u.UserId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(r => r.Role)
					.WithMany(ur => ur.UserRoles)
					.HasForeignKey(ur => ur.RoleId)
					.HasPrincipalKey(r => r.RoleId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<RolePermission>(entity =>
			{
				entity.HasKey(rp => new { rp.RoleId, rp.PermissionId });

				entity.HasOne(r => r.Role)
					.WithMany(rp => rp.RolePermissions)
					.HasForeignKey(rp => rp.RoleId)
					.HasPrincipalKey(r => r.RoleId)
					.OnDelete(DeleteBehavior.Cascade);

				entity.HasOne(p => p.Permission)
					.WithMany(rp => rp.RolePermissions)
					.HasForeignKey(rp => rp.PermissionId)
					.HasPrincipalKey(p => p.PermissionId)
					.OnDelete(DeleteBehavior.Cascade);
			});

			modelBuilder.Entity<Menu>(entity => 
			{
				entity.HasKey(m => new { m.Id, m.MenuId });

				entity.Property(m => m.MenuId)
					.HasComputedColumnSql("CONVERT([varchar](4), concat('M', right('000' + CONVERT([varchar](3), [Id]), 3)))")
					.ValueGeneratedOnAddOrUpdate();
				entity.HasIndex(m => m.MenuId).IsUnique();
				entity.Property(e => e.CreatedAt).HasDefaultValueSql("sysdatetime()");
				entity.Property(e => e.ModifiedAt).HasDefaultValueSql("sysdatetime()");

				entity.HasOne(m => m.ParentMenu)
					.WithMany(m => m.ChildMenus)
					.HasForeignKey(m => m.ParentMenuId)
					.HasPrincipalKey(m => m.MenuId);

				entity.HasOne(m => m.Permission)
					.WithMany(m => m.Menus)
					.HasForeignKey(e => e.PermissionId)
					.HasPrincipalKey(e => e.PermissionId);
			});
		}
	}
}
