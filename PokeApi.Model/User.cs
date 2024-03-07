using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PokeApi.Model;

public partial class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUser { get; set; }
    public string? Name { get; set; }
    public string? SecondName { get; set; }
    public string? Email { get; set; }
    public int? IdRol { get; set; }
    public string? Password { get; set; }
    public bool? Status { get; set; }
    public DateTime? RegisterDate { get; set; }
    public virtual Rol? IdRolNavigation { get; set; }
}
