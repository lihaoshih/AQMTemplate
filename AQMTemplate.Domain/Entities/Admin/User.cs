using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AQMTemplate.Domain.Entities.Admin
{
	public class User
	{		
		public int Id { get; set; }		
		public string UserId { get; set; } = null!;		
		public string Account { get; set; } = null!;		
		public string PasswordSalt { get; set; } = null!;		
		public string PasswordHash { get; set; } = null!;		
		public string? Department { get; set; }		
		public string? JobTitle { get; set; }		
		public string UserName { get; set; } = null!;		
		public string? DisplayName { get; set; }		
		public string? AvatarUrl { get; set; }		
		public string? Alias { get; set; }		
		public char Gender { get; set; }      // 1:Male  2:Female   3.Rather not to say   4.No Correspond Option
		public string? IdNumber { get; set; }
		public DateOnly? Birthday { get; set; }		
		public string? PostalCode { get; set; }
		public string? MailingAddress { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public bool IsActive { get; set; } = true;
		public bool IsLocked { get; set; } = false;
		public int FailedLoginAttempt { get; set; } = 0;
		public DateTime? LastLogin { get; set; } = DateTime.Now;
		public string? Annotation { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime ModifiedAt { get; set; } = DateTime.Now;

		// 導航
		//public virtual ICollection<Role> Roles { get; private set; } = new List<Role>();
		public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

		private User() { }

		public User(string account, string passwordSalt, string passwordHash, string userName, char gender)
		{
			Account = account;
			PasswordSalt = passwordSalt;
			PasswordHash = passwordHash;
			UserName = userName;
			Gender = gender;
			CreatedAt = DateTime.Now;
			ModifiedAt = DateTime.Now;			
		}

		public void UpdatePassword(string passwordSalt, string passwordHash)
		{
			PasswordSalt = passwordSalt;
			PasswordHash = passwordHash;
			ModifiedAt = DateTime.Now;
		}


		public void UpdateProfile(string? department, string? jobTitle, string? displayName, 
			string? avatarUrl, string? alias, string? idNumber, DateOnly? birthday,
			string? postalCode, string? mailingAddress, string? email, string? phone)
		{
			Department = department;
			JobTitle = jobTitle;
			DisplayName = displayName;
			AvatarUrl = avatarUrl;
			Alias = alias;
			IdNumber = idNumber;
			Birthday = birthday;
			PostalCode = postalCode;
			Email = email;
			Phone = phone;
			ModifiedAt = DateTime.Now;
		}

		public void SetActive(bool isActive)
		{
			IsActive = isActive;
			ModifiedAt = DateTime.Now;
		}

		public void RecordLoginAttempt(bool successful)
		{
			if (successful)
			{
				FailedLoginAttempt = 0;
				LastLogin = DateTime.Now;
			}
			else
			{
				FailedLoginAttempt++;
				if (FailedLoginAttempt >= 5)
				{
					IsLocked = true;
				}
			}

			ModifiedAt = DateTime.Now;
		}

		public void Unlock()
		{
			IsLocked = false;
			FailedLoginAttempt = 0;
			ModifiedAt = DateTime.Now;
		}

		public void SetAnnotation(string? annotation)
		{
			Annotation = annotation;
			ModifiedAt = DateTime.Now;
		}
	}
}
