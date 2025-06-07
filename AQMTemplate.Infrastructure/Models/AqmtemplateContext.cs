using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AQMTemplate.Infrastructure.Models;

public partial class AqmtemplateContext : DbContext
{
    public AqmtemplateContext()
    {
    }

    public AqmtemplateContext(DbContextOptions<AqmtemplateContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<廢氣燃燒塔十五分鐘值> 廢氣燃燒塔十五分鐘值s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=10.171.1.26;Database=AQMTemplate;Username=developer;Password=1qazxcv!");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("menus_pkey");

            entity.ToTable("menus", "develop");

            entity.HasIndex(e => e.Menuid, "menus_menuid_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Icon)
                .HasMaxLength(100)
                .HasColumnName("icon");
            entity.Property(e => e.Isvisible)
                .HasDefaultValue(true)
                .HasColumnName("isvisible");
            entity.Property(e => e.Menuid)
                .HasComputedColumnSql("('M'::text || lpad((id)::text, 3, '0'::text))", true)
                .HasColumnName("menuid");
            entity.Property(e => e.Menuname)
                .HasMaxLength(100)
                .HasColumnName("menuname");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Parentmenuid).HasColumnName("parentmenuid");
            entity.Property(e => e.Permissionid)
                .HasMaxLength(100)
                .HasColumnName("permissionid");
            entity.Property(e => e.Sortorder)
                .HasDefaultValue(0)
                .HasColumnName("sortorder");
            entity.Property(e => e.Url)
                .HasMaxLength(200)
                .HasColumnName("url");

            entity.HasOne(d => d.Parentmenu).WithMany(p => p.InverseParentmenu)
                .HasPrincipalKey(p => p.Menuid)
                .HasForeignKey(d => d.Parentmenuid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("menus_parentmenuid_fkey");

            entity.HasOne(d => d.Permission).WithMany(p => p.Menus)
                .HasPrincipalKey(p => p.Permissionid)
                .HasForeignKey(d => d.Permissionid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("menus_permissionid_fkey");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("permissions_pkey");

            entity.ToTable("permissions", "develop");

            entity.HasIndex(e => e.Permissionid, "permissions_permissionid_key").IsUnique();

            entity.HasIndex(e => e.Permissionname, "permissions_permissionname_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Permissionid)
                .HasComputedColumnSql("('P'::text || lpad((id)::text, 2, '0'::text))", true)
                .HasColumnName("permissionid");
            entity.Property(e => e.Permissionname)
                .HasMaxLength(100)
                .HasColumnName("permissionname");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("roles_pkey");

            entity.ToTable("roles", "develop", tb => tb.HasComment("使用者角色"));

            entity.HasIndex(e => e.Roleid, "roles_roleid_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Roleid)
                .HasComputedColumnSql("('R'::text || lpad((id)::text, 3, '0'::text))", true)
                .HasColumnName("roleid");
            entity.Property(e => e.Rolename)
                .HasMaxLength(20)
                .HasColumnName("rolename");

            entity.HasMany(d => d.Permissions).WithMany(p => p.Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "Rolepermission",
                    r => r.HasOne<Permission>().WithMany()
                        .HasPrincipalKey("Permissionid")
                        .HasForeignKey("Permissionid")
                        .HasConstraintName("rolepermissions_permissionid_fkey"),
                    l => l.HasOne<Role>().WithMany()
                        .HasPrincipalKey("Roleid")
                        .HasForeignKey("Roleid")
                        .HasConstraintName("rolepermissions_roleid_fkey"),
                    j =>
                    {
                        j.HasKey("Roleid", "Permissionid").HasName("rolepermissions_pk");
                        j.ToTable("rolepermissions", "develop");
                        j.IndexerProperty<string>("Roleid").HasColumnName("roleid");
                        j.IndexerProperty<string>("Permissionid").HasColumnName("permissionid");
                    });
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users", "develop");

            entity.HasIndex(e => e.Account, "unique_account").IsUnique();

            entity.HasIndex(e => e.Userid, "users_userid_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Account)
                .HasMaxLength(20)
                .HasColumnName("account");
            entity.Property(e => e.Alias)
                .HasMaxLength(20)
                .HasColumnName("alias");
            entity.Property(e => e.Annotation).HasColumnName("annotation");
            entity.Property(e => e.Avatarurl)
                .HasMaxLength(100)
                .HasColumnName("avatarurl");
            entity.Property(e => e.Birthday).HasColumnName("birthday");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Department)
                .HasMaxLength(20)
                .HasColumnName("department");
            entity.Property(e => e.Displayname)
                .HasMaxLength(20)
                .HasColumnName("displayname");
            entity.Property(e => e.Email)
                .HasMaxLength(40)
                .HasColumnName("email");
            entity.Property(e => e.Failedloginattempt)
                .HasDefaultValue(0)
                .HasColumnName("failedloginattempt");
            entity.Property(e => e.Gender)
                .HasMaxLength(1)
                .HasColumnName("gender");
            entity.Property(e => e.Idnumber).HasColumnName("idnumber");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Islocked)
                .HasDefaultValue(false)
                .HasColumnName("islocked");
            entity.Property(e => e.Jobtitle)
                .HasMaxLength(20)
                .HasColumnName("jobtitle");
            entity.Property(e => e.Lastlogin)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastlogin");
            entity.Property(e => e.Mailingaddress).HasColumnName("mailingaddress");
            entity.Property(e => e.Modifiedat)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("modifiedat");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Passwordsalt).HasColumnName("passwordsalt");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasColumnName("phone");
            entity.Property(e => e.Postalcode)
                .HasMaxLength(6)
                .HasColumnName("postalcode");
            entity.Property(e => e.Userid)
                .HasComputedColumnSql("('U'::text || lpad((id)::text, 4, '0'::text))", true)
                .HasColumnName("userid");
            entity.Property(e => e.Username)
                .HasMaxLength(10)
                .HasColumnName("username");

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Userrole",
                    r => r.HasOne<Role>().WithMany()
                        .HasPrincipalKey("Roleid")
                        .HasForeignKey("Roleid")
                        .HasConstraintName("userroles_roleid_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasPrincipalKey("Userid")
                        .HasForeignKey("Userid")
                        .HasConstraintName("userroles_userid_fkey"),
                    j =>
                    {
                        j.HasKey("Userid", "Roleid").HasName("userroles_pkey");
                        j.ToTable("userroles", "develop");
                        j.IndexerProperty<string>("Userid").HasColumnName("userid");
                        j.IndexerProperty<string>("Roleid").HasColumnName("roleid");
                    });
        });

        modelBuilder.Entity<廢氣燃燒塔十五分鐘值>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("廢氣燃燒塔十五分鐘值", "Data");

            entity.Property(e => e.Abbr).HasColumnName("abbr");
            entity.Property(e => e.Cno).HasColumnName("cno");
            entity.Property(e => e.Code2).HasColumnName("code2");
            entity.Property(e => e.Code2desc).HasColumnName("code2desc");
            entity.Property(e => e.Epb).HasColumnName("epb");
            entity.Property(e => e.Flareno).HasColumnName("flareno");
            entity.Property(e => e.Item).HasColumnName("item");
            entity.Property(e => e.Itemdesc).HasColumnName("itemdesc");
            entity.Property(e => e.Locationno).HasColumnName("locationno");
            entity.Property(e => e.MTime).HasColumnName("m_time");
            entity.Property(e => e.MVal).HasColumnName("m_val");
            entity.Property(e => e.Unit).HasColumnName("unit");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
