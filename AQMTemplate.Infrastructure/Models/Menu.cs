using System;
using System.Collections.Generic;

namespace AQMTemplate.Infrastructure.Models;

public partial class Menu
{
    public int Id { get; set; }

    public string Menuid { get; set; } = null!;

    public string? Parentmenuid { get; set; }

    public string Menuname { get; set; } = null!;

    public string? Icon { get; set; }

    public string? Url { get; set; }

    public string? Permissionid { get; set; }

    public int? Sortorder { get; set; }

    public bool Isvisible { get; set; }

    public DateTime Createdat { get; set; }

    public DateTime Modifiedat { get; set; }

    public virtual ICollection<Menu> InverseParentmenu { get; set; } = new List<Menu>();

    public virtual Menu? Parentmenu { get; set; }

    public virtual Permission? Permission { get; set; }
}
