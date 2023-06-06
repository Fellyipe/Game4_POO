using Gamificacao4;
using Gamificacao4.Interfaces;
    
namespace Gamificacao4
{
    public class GerenciamentoDePedidos
    {
        private readonly IPedidoRepository pedidoRepository;
        private readonly IItemPedidoRepository itemPedidoRepository;

        public GerenciamentoDePedidos(IPedidoRepository pedidoRepository, IItemPedidoRepository itemPedidoRepository)
        {
            this.pedidoRepository = pedidoRepository;
            this.itemPedidoRepository = itemPedidoRepository;
        }

        public Pedido CriarPedido(DateTime data, string cliente, string status)
        {
            var pedido = new Pedido{Id = 0, Data = data, Cliente = cliente, Status = status};
            pedidoRepository.Create(pedido);
            Console.WriteLine("Id do pedido: " + pedido.Id);
            return pedido;
        }

        public void AdicionarItemPedido(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
            var pedido = pedidoRepository.GetById(pedidoId);
            Console.WriteLine("pedidoClienteNome: " + pedido?.Cliente + "; produtoId: " + produtoId);
            var produtoRepository = new ProdutoRepository("server=localhost;database=poo_game4;user=root;password=;");
            var produto = produtoRepository.GetById(produtoId);

            if (pedido != null && produto != null)
            {
                var itemPedido = new ItemPedido{Id = 0, Produto = produto, Quantidade = quantidade, PrecoUnitario = precoUnitario, Pedido = pedido};
                itemPedidoRepository.Create(itemPedido);
            }
        }

        public void AtualizarStatusPedido(int pedidoId, string novoStatus)
        {
            var pedido = pedidoRepository.GetById(pedidoId);
            if (pedido != null)
            {
                pedido.Status = novoStatus;
                pedidoRepository.Update(pedido);
            }
        }

        public void RemoverPedido(int pedidoId)
        {
            var pedido = pedidoRepository.GetById(pedidoId);
            if (pedido != null)
            {
                pedidoRepository.Delete(pedidoId);
            }
        }

        public List<Pedido> ListarPedidosPorCliente(string cliente)
        {
            return pedidoRepository.GetByCliente(cliente);
        }

        public List<Pedido> ListarPedidosPorStatus(string status)
        {
            return pedidoRepository.GetByStatus(status);
        }

        public List<Pedido> ListarPedidosPorData(DateTime data)
        {
            return pedidoRepository.GetByData(data);
        }

        public decimal CalcularValorTotalPedido(int pedidoId)
        {
            var pedido = pedidoRepository.GetById(pedidoId);
            var itemPedidoRepository = new ItemPedidoRepository("server=localhost;database=poo_game4;user=root;password=;");
            var itensPedido = itemPedidoRepository.GetByPedidoId(pedidoId);
            if (pedido != null)
            {
                decimal valorTotal = 0;
                foreach (var itemPedido in itensPedido)
                {
                    valorTotal += itemPedido.Quantidade * itemPedido.PrecoUnitario;
                }
                return valorTotal;
            }
            return 0;
        }
    }
}