﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PikeApi.DTO
{
    public class Sesion_DTO
    {
        public int IdUser { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; }
        public int? IdRol { get; set; }
        public string? Rol { get; set; }
    }
}
