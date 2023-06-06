using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamificacao4.Interfaces
{
    public interface IRepository<T>
    {
        T GetById<T>(int id);
        void Create<T>(T entidade);
        void Update<T>(T entidade);
        void Delete<T>(int id);
        IEnumerable<T> ListAll<T>();
    }
}