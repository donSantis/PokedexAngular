using System;
using System.Collections.Generic;

namespace PokeApi.Model.Menu;

public partial class MenuRol
{
    public int id { get; set; }

    public int? idMenu { get; set; }

    public int? idRol { get; set; }

    public virtual Menu? idMenuNavigation { get; set; }

    public virtual Rol? idRolNavigation { get; set; }
}
