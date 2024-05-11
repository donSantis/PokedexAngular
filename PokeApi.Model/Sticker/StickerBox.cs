using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;

namespace PokeApi.Model.Sticker
{
    public partial class StickerBox
    {
        public List<Stickers>? stickers { get; set; }

    }
}
