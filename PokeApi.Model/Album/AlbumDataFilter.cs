using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Album
{
    public class AlbumDataFilter : AlbumBase
    {
        public string? urlPrevious { get; set; }
        public string? urlNext { get; set; }
        public int? pokemonNumber { get; set; }
        public int? pokemon { get; set; }

    }
}
