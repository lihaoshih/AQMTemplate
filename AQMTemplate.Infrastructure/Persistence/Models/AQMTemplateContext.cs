using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AQMTemplate.Web.Models;

public partial class AQMTemplateContext : DbContext
{
    public AQMTemplateContext(DbContextOptions<AQMTemplateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menus> Menus { get; set; }

    public virtual DbSet<Permissions> Permissions { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Users> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menus>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.MenuId }).HasName("PK_Admin_Menus");

            entity.ToTable("Menus", "Admin");

            entity.HasIndex(e => e.MenuId, "UK_Admin_Menus_MenuId").IsUnique();

            entity.Property(e => e.MenuId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](4),concat('M',right('000'+CONVERT([varchar](3),[Id]),(3)))))", true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MenuName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ParentMenuId)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.PermissionId)
                .HasMaxLength(4)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.ParentMenu).WithMany(p => p.InverseParentMenu)
                .HasPrincipalKey(p => p.MenuId)
                .HasForeignKey(d => d.ParentMenuId)
                .HasConstraintName("FK_Menus_ParentMenu");

            entity.HasOne(d => d.Permission).WithMany(p => p.Menus)
                .HasPrincipalKey(p => p.PermissionId)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_Menus_Permissions");
        });

        modelBuilder.Entity<Permissions>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.PermissionId }).HasName("PK_Admin_Permissions");

            entity.ToTable("Permissions", "Admin");

            entity.HasIndex(e => e.PermissionId, "UK_Admin_Permissions_PermissionId").IsUnique();

            entity.Property(e => e.PermissionId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](4),concat('P',right('000'+CONVERT([varchar](3),[Id]),(3)))))", true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PermissionName)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.RoleId }).HasName("PK_Admin_Roles");

            entity.ToTable("Roles", "Admin");

            entity.HasIndex(e => e.RoleId, "UK_Admin_Roles_RoleId").IsUnique();

            entity.Property(e => e.RoleId)
                .HasMaxLength(4)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](4),concat('R',right('000'+CONVERT([varchar](3),[Id]),(3)))))", true);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.RoleName)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasMany(d => d.Permission).WithMany(p => p.Role)
                .UsingEntity<Dictionary<string, object>>(
                    "RolePermissions",
                    r => r.HasOne<Permissions>().WithMany()
                        .HasPrincipalKey("PermissionId")
                        .HasForeignKey("PermissionId")
                        .HasConstraintName("FK_Admin_RolePermissions_Permissions"),
                    l => l.HasOne<Roles>().WithMany()
                        .HasPrincipalKey("RoleId")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Admin_RolePermissions_Roles"),
                    j =>
                    {
                        j.HasKey("RoleId", "PermissionId").HasName("PK_Admin_RolePermissions");
                        j.ToTable("RolePermissions", "Admin");
                        j.IndexerProperty<string>("RoleId")
                            .HasMaxLength(4)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("PermissionId")
                            .HasMaxLength(4)
                            .IsUnicode(false);
                    });
        });

        modelBuilder.Entity<Users>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.UserId }).HasName("PK_Admin_Users");

            entity.ToTable("Users", "Admin");

            entity.HasIndex(e => e.UserId, "UK_Admin_Users_UserId").IsUnique();

            entity.Property(e => e.UserId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasComputedColumnSql("(CONVERT([varchar](5),concat('U',right('0000'+CONVERT([varchar](4),[Id]),(4)))))", true);
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Alias).HasMaxLength(10);
            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Department).HasMaxLength(20);
            entity.Property(e => e.Displayname).HasMaxLength(20);
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.IdNumber)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobTitle).HasMaxLength(20);
            entity.Property(e => e.MailingAddress).HasMaxLength(100);
            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.PasswordSalt)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.PostalCode)
                .HasMaxLength(6)
                .IsUnicode(false);
            entity.Property(e => e.Username).HasMaxLength(10);

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "UserRoles",
                    r => r.HasOne<Roles>().WithMany()
                        .HasPrincipalKey("RoleId")
                        .HasForeignKey("RoleId")
                        .HasConstraintName("FK_Admin_UserRoles_Roles"),
                    l => l.HasOne<Users>().WithMany()
                        .HasPrincipalKey("UserId")
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_Admin_UserRoles_Users"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId").HasName("PK_Admin_UserRoles");
                        j.ToTable("UserRoles", "Admin");
                        j.IndexerProperty<string>("UserId")
                            .HasMaxLength(5)
                            .IsUnicode(false);
                        j.IndexerProperty<string>("RoleId")
                            .HasMaxLength(4)
                            .IsUnicode(false);
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
