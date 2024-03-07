using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace PokeApi.DAL.Repositorys.Contract
{
    public interface IGenericRepository<TModel> where TModel : class
    {
        Task<TModel> GetModel(Expression<Func<TModel, bool>> filter);
        Task<TModel> CreateModel(TModel model);
        Task<bool> EditModel(TModel model);
        Task<bool> DeleteModel(TModel model);
        Task<IQueryable<TModel>> Consulta(Expression<Func<TModel, bool>> filter = null);  

    }
}
