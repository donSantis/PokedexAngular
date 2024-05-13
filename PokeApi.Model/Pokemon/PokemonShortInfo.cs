using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.PokeApiClasses
{
    public class PokemonShortInfo
    {
        public int? id { get; set; }
        public string? name { get; set; }
        public Sprites? sprites { get; set; }
        public Url? EvolveFrom { get; set; }

    }
}
