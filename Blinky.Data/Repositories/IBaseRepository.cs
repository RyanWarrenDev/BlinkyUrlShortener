using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blinky.Data.Repositories
{
    public interface IBaseRepository<T>
    {
        T Insert(T data);

        IEnumerable<T> All();

        T FindById(int id);

        void Update(T entity);

        bool Delete(int id);
    }
}
