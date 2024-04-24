using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Sticker
{
    public class Sticker
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? idPokemon { get; set; } // para ver que pokemon es 
        public int? idUser { get; set; }
        public int? state { get; set; } 
        public string version { get; set; }
        public DateTime? lastModification { get; set; }
        public DateTime? registerDate { get; set; }

    }
}
