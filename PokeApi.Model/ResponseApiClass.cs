    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace PokeApi.Model
    {
        public class ResponseApiClass
        {
            public string? count { get; set; }
            public string? next { get; set; }
            public string? previous { get; set; }
            public List<ResponseApiResultsClass>? results { get; set; }

        }
    }
