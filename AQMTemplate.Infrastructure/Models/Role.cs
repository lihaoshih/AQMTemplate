using System;
using System.Collections.Generic;

namespace AQMTemplate.Infrastructure.Models;

/// <summary>
/// 使用者角色
/// </summary>
public partial class Role
{
    public int Id { get; set; }

    public string Roleid { get; set; } = null!;

    public string Rolename { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Modifiedat { get; set; }

    public virtual ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
