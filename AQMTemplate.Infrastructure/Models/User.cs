using System;
using System.Collections.Generic;

namespace AQMTemplate.Infrastructure.Models;

public partial class User
{
    public int Id { get; set; }

    public string Userid { get; set; } = null!;

    public string Account { get; set; } = null!;

    public string Passwordsalt { get; set; } = null!;

    public string Passwordhash { get; set; } = null!;

    public string? Department { get; set; }

    public string? Jobtitle { get; set; }

    public string Username { get; set; } = null!;

    public string? Displayname { get; set; }

    public string? Avatarurl { get; set; }

    public string? Alias { get; set; }

    public char Gender { get; set; }

    public string? Idnumber { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Postalcode { get; set; }

    public string? Mailingaddress { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool Isactive { get; set; }

    public bool Islocked { get; set; }

    public int Failedloginattempt { get; set; }

    public DateTime? Lastlogin { get; set; }

    public string? Annotation { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Modifiedat { get; set; }

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
