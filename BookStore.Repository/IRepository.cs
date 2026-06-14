using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStore.Model;

namespace BookStore.Repository
{
    public interface IRepository<T> where T : BaseModel
    {
        Task<IEnumerable<T>> GetAllAsync(string Include="" , int pageNumber = 1 , int pageSize = 10 );
        Task<T> Get(Expression<Func<T, bool>> predicate , string Include="" );
        Task Add(T item);
        void Update(T item);
        Task Delete(int id);
    }
}
