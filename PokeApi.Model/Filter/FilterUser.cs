using PikeApi.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeApi.Model.Filter
{
    public partial class FilterUser
    {
        public List<int?> idsUsers { get; set; }
        public int? idUser { get; set; }
        public string? Email { get; set; }
        public int? status { get; set; }

    }
}
