using PokeApi.Model.Album;
using PokeApi.Model.Sticker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Filter
{
    public partial class Filter
    {
        public string? name { get; set; }
        public AlbumBase? AlbumBase { get; set; }
        public FilterPages? Pages { get; set; }
        public User? user{ get; set; }
        public Stickers? Sticker { get; set; }
    }
}
