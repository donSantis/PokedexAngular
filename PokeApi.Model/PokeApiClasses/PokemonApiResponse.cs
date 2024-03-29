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
        public int id { get; set; }

        public int? IdPokemonApi { get; set; }

        public string? name { get; set; }

        public string? Evolution2 { get; set; }

        public string? Evolution3 { get; set; }
        public int weight { get; set; }
        public List<Types> types { get; set; }
        public Sprites sprites { get; set; }

        public string? Url { get; set; }

        //[JsonProperty("official-artwork")]
        public OfficialArtwork? officialArtwork { get; set; }

    }
}
