﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IPokemonRepository : IGenericRepository<Pokemon>
    {
        Task<Pokemon> Register(Pokemon model);
    }
}
