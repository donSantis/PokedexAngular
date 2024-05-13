using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PokeApi.Model.Exchange;
using System.Threading.Tasks;

namespace PokeApi.Model.Filter
{
    public class FilterExchange
    {
        public int? idExchange { get; set; }
        public int? status { get; set; }
        public List<Exchanges> exchanges { get; set; }
}
}
