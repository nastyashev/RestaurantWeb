using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantSupportWebApp.Interfaces
{
    public interface IRepository<TModel> where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();

        void Add(TModel model);

        void Delete(TModel model);

        void Update(TModel model);
    }
}
