using System;
using PokeApi.Model;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokeApi.Model.Exchange
{
    public class Exchanges
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        public int idExchange { get; set; }
        public int idReceivingUser { get; set; }
        public int idSenderUser { get; set; }
        public int status { get; set; }
        public int idSticker { get; set; }
        public DateTime? lastModification { get; set; }
        public DateTime? registerDate { get; set; }
    }
}
