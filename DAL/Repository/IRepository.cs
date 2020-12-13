using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DAL.Repository
{
   public  interface IRepository<T> where T : class
    {
        IQueryable<T> FindAll();

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate);
    }
}
