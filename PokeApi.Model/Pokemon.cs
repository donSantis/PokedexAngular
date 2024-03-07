using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PokeApi.Model;

public partial class Pokemon
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPokemon { get; set; }

    public int? IdPokemonApi { get; set; }

    public string? Name { get; set; }

    public string? Evolution2 { get; set; }

    public string? Evolution3 { get; set; }

    public string? Type { get; set; }
}
