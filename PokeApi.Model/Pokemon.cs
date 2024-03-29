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
    public int id { get; set; }

    public int? IdPokemonApi { get; set; }

    public string? name { get; set; }

    public string? Evolution2 { get; set; }

    public string? Evolution3 { get; set; }
    public int weight { get; set; }


    public string? Url { get; set; }

}
