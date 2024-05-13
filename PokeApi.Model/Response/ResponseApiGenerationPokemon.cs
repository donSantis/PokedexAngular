using Newtonsoft.Json;
using PokeApi.Model.PokeApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Response
{
    public class ResponseApiGenerationPokemon
    {
        public string? id { get; set; }
        public string? name { get; set; }
        [JsonProperty("pokemon_species")]
        public List<PokemonSpecies>? pokemonSpecies { get; set; }
    }
}
