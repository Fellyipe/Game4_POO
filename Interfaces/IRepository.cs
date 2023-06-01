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
        void Create(T entidade);
        void Update(T entidade);
        void Delete(int id);
        List<T> ListAll();
    }
}