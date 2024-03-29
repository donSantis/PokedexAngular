using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace PokeApi.Model.PokeApiClasses
{
    public class PokemonApiResponse
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public PokemonApiResponse? Evolution1 { get; set; }
        public PokemonApiResponse? Evolution2 { get; set; }
        public PokemonApiResponse? Evolution3 { get; set; }
        public int? weight { get; set; }
        public List<Types>? types { get; set; }
        public Sprites? sprites { get; set; }
        public Url? species { get; set; }
        [JsonProperty("evolution_chain")]
        public Url? evolutionChain { get; set; }
        public Chain? chain { get; set; }

        public static implicit operator PokemonApiResponse(List<PokemonApiResponse> v)
        {
            throw new NotImplementedException();
        }
        [JsonProperty("evolves_from_species")]
        public Url? EvolveFrom { get; set; }
    }
}
