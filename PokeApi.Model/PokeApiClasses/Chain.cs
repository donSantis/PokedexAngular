using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.PokeApiClasses
{
    public class Chain
    {
        [JsonProperty("evolves_to")]
        public List<EvolveTo>? EvolveTo { get; set; }

    }
}
