using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PokeApi.Model.Album
{
    public partial class AlbumBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int? version { get; set; }
        public int? state { get; set; }
        public int? pokemonStart { get; set; }
        public int? pokemonEnd { get; set; }
        public string url { get; set; }
        public DateTime? lastModification { get; set; }
        public DateTime? registerDate { get; set; }
    }
}
