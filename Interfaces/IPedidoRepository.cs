using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamificacao4.Interfaces
{
    public interface IPedidoRepository : IRepository<Pedido>
    {
        Pedido? GetById(int id);
        void Create(Pedido pedido);
        void Update(Pedido pedido);
        void Delete(int id);
        List<Pedido> GetByCliente(string cliente);
        List<Pedido> GetByStatus(string status);
        List<Pedido> GetByData(DateTime data);
        IEnumerable<Pedido> ListAll();
    }
}
