using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PikeApi.DTO;


namespace PokeApi.Model.Sticker;

public partial class Stickers
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int id { get; set; }
    public int? idPokemon { get; set; } // para ver que pokemon es 
    public int? idUser { get; set; }
    public int? status { get; set; }
    public int? version { get; set; }
    public int? shiny { get; set; }
    public DateTime? lastModification { get; set; }
    public DateTime? registerDate { get; set; }

}
