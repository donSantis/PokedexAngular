using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Filter
{
    public partial class FilterStickers
    {
        public int? status { get; set; }
        public int? version { get; set; }
        public int? shiny { get; set; }
    }
}
