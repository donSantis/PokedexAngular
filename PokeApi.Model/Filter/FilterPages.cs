using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Filter
{
    public partial class FilterPages
    {
        public string? actualPageUrl { get; set; }
        public string? prevPageUrl { get; set; }
        public string? nextPageUrl { get; set; }
        public int? actualPage { get; set; }
        public int? nextPage { get; set; }
        public int? prevPage { get; set; }
        public int? maxPage { get; set; }
        public int? minPage { get; set; }
        public int? offSet { get; set; }
        public int? actualOffSet { get; set; }
        public int? actuallimitPokemonPage { get; set; }
        public int? limitPokemonPage { get; set; }
    }
}
