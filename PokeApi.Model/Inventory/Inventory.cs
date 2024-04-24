using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Inventory
{
    public class Inventory
    {
        public int? id { get; set; }
        public int? idUser { get; set; }
        public int? space { get; set; }
        public DateTime? lastModification { get; set; }
        public DateTime? registerDate { get; set; }


    }
}
