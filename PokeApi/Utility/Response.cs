﻿using PokeApi.Model.PokeApiClasses;

namespace PokeApi.API.Utility
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public T Value { get; set; }
        public string Msg { get; set; }
    }

    public class PokemonResponse
    {
        public string? count { get; set; }
        public string? next { get; set; }
        public string? previous { get; set; }
        public List<PokemonApiResponse> results { get; set; }
    }

    public class PokemonInfo
    {
        public string id { get; set; }
        public string name { get; set; }
        // Otras propiedades que esperas recibir de la API, como habilidades, tipos, estadísticas, etc.
    }
    public class ResponseString
    {
        public string rsp { get; set; }
    }
  
}
