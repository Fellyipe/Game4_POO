using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamificacao4.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        void Create(Produto produto);
        void Update(Produto produto);
        void Delete(int id);
        Produto? GetById(int id);
        IEnumerable<Produto> ListAll();
    }
}