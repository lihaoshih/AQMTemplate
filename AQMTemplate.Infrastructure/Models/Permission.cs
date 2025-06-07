using System;
using System.Collections.Generic;

namespace AQMTemplate.Infrastructure.Models;

public partial class Permission
{
    public int Id { get; set; }

    public string Permissionid { get; set; } = null!;

    public string Permissionname { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Modifiedat { get; set; }

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
