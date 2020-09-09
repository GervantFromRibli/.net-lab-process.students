using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IRepository<T>
        where T : class
    {
        void Create(T item);

        T FindById(int id);

        List<T> GetAll();

        void Remove(T item);

        void Update(T item);
    }
}
