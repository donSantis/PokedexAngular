using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PokeApi.Model;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public string? name { get; set; }
    public string? secondName { get; set; }
    public string? email { get; set; }
    public int? idRol { get; set; }
    public string? password { get; set; }
    public bool? status { get; set; }
    public DateTime? registerDate { get; set; }
    public virtual Rol? idRolNavigation { get; set; }

    public object Select(Func<object, object> value)
    {
        throw new NotImplementedException();
    }
}
