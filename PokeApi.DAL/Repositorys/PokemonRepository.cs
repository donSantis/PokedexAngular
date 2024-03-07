using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PokeApi.DAL.DBContext;
using PokeApi.DAL.Repositorys.Contract;
using PokeApi.Model;

namespace PokeApi.DAL.Repositorys
{
    public class PokemonRepository : GenericRepository<Pokemon>, IPokemonRepository
    {
        private readonly PokedexdbContext _dbContext;

        public PokemonRepository(PokedexdbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<Pokemon> Register(Pokemon model)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    Pokemon pokemonFind = _dbContext.Pokemons.FirstOrDefault(p => p.IdPokemon == model.IdPokemon);
                    if (pokemonFind == null)
                    {
                        // No existe un Pokémon con el mismo IdPokemon, así que puedes agregar el nuevo Pokémon.
                        _dbContext.Pokemons.Add(model);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        // Puedes manejar el caso en el que ya existe un Pokémon con el mismo IdPokemon si es necesario.
                        // Por ejemplo, lanzar una excepción, realizar alguna acción especial, etc.
                        transaction.Rollback();
                    }

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                    // Manejar la excepción, ya sea lanzándola nuevamente o realizando alguna acción específica.
                }
            }
            return model;
        }

    }
}
