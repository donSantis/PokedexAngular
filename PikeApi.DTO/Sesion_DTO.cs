using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PikeApi.DTO
{
    public class Sesion_DTO
    {
        public int id { get; set; }
        public string? name { get; set; }
        public string? secondName { get; set; }
        public string? email { get; set; }
        public int? idRol { get; set; }
        public string? rol { get; set; }
    }
}
