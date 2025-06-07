using AQMTemplate.Domain.Entities.Admin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AQMTemplate.Infrastructure.Persistence.Configuration.Admin
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("users", "develop");
			
			builder.HasKey(u => u.Id);
			builder.Property(u => u.Id)
				.HasColumnName("id")
				.IsRequired();
			
			builder.Property(u => u.UserId)
				.HasColumnName("userid")
				.HasComputedColumnSql("('U' || lpad(id::text, 4, '0'::text))", stored: true)
				.IsRequired()
				.ValueGeneratedOnAddOrUpdate();

			builder.HasIndex(u => u.UserId)
				.IsUnique();

			builder.Property(u => u.Account)
				.HasColumnName("account")
				.HasMaxLength(20);

			builder.Property(u => u.PasswordSalt)
				.HasColumnName("passwordsalt");

			builder.Property(u => u.PasswordHash)
				.HasColumnName("passwordhash");

			builder.Property(u => u.Department)
				.HasColumnName("department")
				.HasMaxLength(20)
				.IsRequired(false);

			builder.Property(u => u.JobTitle)
				.HasColumnName("jobtitle")
				.HasMaxLength(20)
				.IsRequired(false);

			builder.Property(u => u.UserName)
				.HasColumnName("username")
				.HasMaxLength(10)
				.IsRequired();

			builder.Property(u => u.DisplayName)
				.HasColumnName("displayname")
				.HasMaxLength(20);

			builder.Property(u => u.AvatarUrl)
				.HasColumnName("avatarurl")
				.HasMaxLength(100);

			builder.Property(u => u.Alias)
				.HasColumnName("alias")
				.HasMaxLength(20);

			builder.Property(u => u.Gender)
				.HasColumnName("gender")
				.HasColumnType("char");

			builder.Property(u => u.IdNumber)
				.HasColumnName("idnumber");

			builder.Property(u => u.Birthday)
				.HasColumnName("birthday");

			builder.Property(u => u.PostalCode)
				.HasColumnName("postalcode")
				.HasMaxLength(6);

			builder.Property(u => u.MailingAddress)
				.HasColumnName("mailingaddress");

			builder.Property(u => u.Email)
				.HasColumnName("email")
				.HasMaxLength(40);

			builder.Property(u => u.Phone)
				.HasColumnName("phone")
				.HasMaxLength(20);

			builder.Property(u => u.IsActive)
				.HasColumnName("isactive")
				.HasDefaultValue(true);

			builder.Property(u => u.IsLocked)
				.HasColumnName("islocked")
				.HasDefaultValue(false);

			builder.Property(u => u.FailedLoginAttempt)
				.HasColumnName("failedloginattempt")
				.HasDefaultValue(0);

			builder.Property(u => u.LastLogin)
				.HasColumnName("lastlogin")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(u => u.Annotation)
				.HasColumnName("annotation")
				.IsRequired(false);

			builder.Property(u => u.CreatedAt)
				.HasColumnName("createdat")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");

			builder.Property(u => u.ModifiedAt)
				.HasColumnName("modifiedat")
				.HasDefaultValueSql("CURRENT_TIMESTAMP");
		}
	}
}
