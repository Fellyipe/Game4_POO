using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamificacao3.Interfaces
{
    public interface IItemPedidoRepository
    {
        ItemPedido? GetById(int id);
        void Create(ItemPedido itemPedido);
        void Update(ItemPedido itemPedido);
        void Delete(int id);
        List<ItemPedido> GetByPedidoId(int pedidoId);
        IEnumerable<ItemPedido> ListAll();
    }
}