using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;
using BookStore.Model;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly BookStoreContext context;
        DbSet<T> Set;
        public Repository(BookStoreContext _context)
        {
            context = _context;
            Set = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAllAsync(string Include = "", int pageNumber = 1, int pageSize = 10)
        {
            int NoOfSkip = (pageNumber - 1) * pageSize;
            var query = Set.Where(o => o.IsDeleted == false);

            if (Include == "")
                return await query.Skip(NoOfSkip).Take(pageSize).ToListAsync();
            else
                return await query.Include(Include).Skip(NoOfSkip).Take(pageSize).ToListAsync();
        }
        public async Task<T> Get(Expression<Func<T, bool>> predicate, string Include = "")
        {
            var query = Set.Where(o => o.IsDeleted == false);
            if (Include == "")
                return await query.FirstOrDefaultAsync(predicate);
            else
                return await query.Include(Include).FirstOrDefaultAsync(predicate);
        }
        public async Task Add(T item)
        {
            Set.Attach(item);
            await Set.AddAsync(item);
        }
        public void Update(T item)
        {
            Set.Attach(item);
            Set.Update(item);
        }
        public async Task Delete(int id)
        {
            T obj = await Get(o => o.Id == id);
            Set.Attach(obj);
            obj.IsDeleted = true;
        }
    }
}
