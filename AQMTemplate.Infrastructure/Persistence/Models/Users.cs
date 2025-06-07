using System;
using System.Collections.Generic;

namespace AQMTemplate.Web.Models;

public partial class Users
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Account { get; set; } = null!;

    public string PasswordSalt { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string? Department { get; set; }

    public string? JobTitle { get; set; }

    public string? Username { get; set; }

    public string? Displayname { get; set; }

    public string? AvatarUrl { get; set; }

    public string? Alias { get; set; }

    public string Gender { get; set; } = null!;

    public string? IdNumber { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? PostalCode { get; set; }

    public string? MailingAddress { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    public bool IsLocked { get; set; }

    public int FailedLoginAttempt { get; set; }

    public DateTime? LastLogin { get; set; }

    public string? Annotation { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<Roles> Role { get; set; } = new List<Roles>();
}
