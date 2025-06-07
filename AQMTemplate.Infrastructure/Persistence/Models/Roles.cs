using System;
using System.Collections.Generic;

namespace AQMTemplate.Web.Models;

public partial class Roles
{
    public int Id { get; set; }

    public string RoleId { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<Permissions> Permission { get; set; } = new List<Permissions>();

    public virtual ICollection<Users> User { get; set; } = new List<Users>();
}
