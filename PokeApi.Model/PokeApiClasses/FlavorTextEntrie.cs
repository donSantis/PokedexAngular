using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.PokeApiClasses
{
    public class FlavorTextEntrie
    {
        [JsonProperty("flavor_text")]
        public string? flavorText { get; set; }
        public Version? version { get; set; }
        public Language? language { get; set; }


    }
}
