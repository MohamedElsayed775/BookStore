using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.Data;

namespace BookStore.UnitOfWork
{
    public class UnitOfWorks
    {
        private readonly BookStoreContext context;

        public UnitOfWorks(BookStoreContext _Context)
        {
            context = _Context;
            BeginTransaction();
        }

        public void BeginTransaction()
        {
            if (context.Database.CurrentTransaction is null)
            {
                context.Database.BeginTransaction();
            }
        }
        public void save()
        {
            try
            {
                context.SaveChanges();
            }
            catch(Exception ex)  
            {
                context.Database.CurrentTransaction.Rollback();
            }
        }
        public void SaveAndCommit()
        {
            try
            {
                context.SaveChanges();
                context.Database.CurrentTransaction.Commit();
            }
            catch
            {
                context.Database.CurrentTransaction.Rollback();
            }            
        }
    }
}
