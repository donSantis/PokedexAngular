using System;
using System.Collections.Generic;

namespace PokeApi.Model.Menu;

public partial class Menu
{
    public int id { get; set; }

    public string? name { get; set; }

    public string? icon { get; set; }

    public string? url { get; set; }

    public virtual ICollection<MenuRol> menuRols { get; } = new List<MenuRol>();
}
