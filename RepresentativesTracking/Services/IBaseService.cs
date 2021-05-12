using System.Collections.Generic;
using System.Threading.Tasks;
namespace Services
{
    public interface IBaseService<T, TId>
    {
        Task<T> Create(T t);
        Task<T> Modify(TId id, T t);
        Task<T> Delete(TId id);
        Task<T> FindById(TId id);
        Task<IEnumerable<T>> All(int PageNumber, int Count);
    }
}
