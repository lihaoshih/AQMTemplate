using System;
using System.Collections.Generic;

namespace AQMTemplate.Web.Models;

public partial class Menus
{
    public int Id { get; set; }

    public string MenuId { get; set; } = null!;

    public string? ParentMenuId { get; set; }

    public string MenuName { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Url { get; set; }

    public string? PermissionId { get; set; }

    public int SortOrder { get; set; }

    public bool IsVisible { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }

    public virtual ICollection<Menus> InverseParentMenu { get; set; } = new List<Menus>();

    public virtual Menus? ParentMenu { get; set; }

    public virtual Permissions? Permission { get; set; }
}
