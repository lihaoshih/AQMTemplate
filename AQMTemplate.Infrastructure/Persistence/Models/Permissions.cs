using System;
using System.Collections.Generic;

namespace AQMTemplate.Web.Models;

public partial class Permissions
{
    public int Id { get; set; }

    public string PermissionId { get; set; } = null!;

    public string PermissionName { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<Menus> Menus { get; set; } = new List<Menus>();

    public virtual ICollection<Roles> Role { get; set; } = new List<Roles>();
}
