using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamificacao3.Interfaces
{
    public interface IPedidoRepository
    {
        Pedido? GetById(int id);
        void Create(Pedido pedido);
        void Update(Pedido pedido);
        void Delete(int id);
        List<Pedido> GetByCliente(Cliente cliente);
        List<Pedido> GetByStatus(string status);
        List<Pedido> GetByData(DateTime data);
        IEnumerable<Pedido> ListAll();
    }
}
