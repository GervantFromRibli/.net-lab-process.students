using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public interface ISqlRepository<T> 
        where T : class
    {
        void FillRepositoryWithSqlData();

        void SaveChanges(string type, T item);
    }
}
