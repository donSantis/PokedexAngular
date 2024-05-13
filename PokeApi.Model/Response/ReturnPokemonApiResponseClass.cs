using PokeApi.Model.PokeApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Response
{
    public class ReturnPokemonApiResponseClass
    {
        public string? count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<PokemonApiResponse>? results { get; set; }

        public static implicit operator List<object>(ReturnPokemonApiResponseClass v)
        {
            throw new NotImplementedException();
        }
    }
}
