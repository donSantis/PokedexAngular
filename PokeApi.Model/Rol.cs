using System;
using System.Collections.Generic;
using PokeApi.Model.Menu;

namespace PokeApi.Model;

public partial class Rol
{
    public int id { get; set; }

    public string? name { get; set; }

    public DateTime? registerDate { get; set; }

    public virtual ICollection<MenuRol> menuRols { get; } = new List<MenuRol>();

    public virtual ICollection<User> users { get; } = new List<User>();
}
