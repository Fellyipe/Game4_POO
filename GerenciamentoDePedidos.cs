using Gamificacao3;
using Gamificacao3.Interfaces;
    
namespace Gamificacao3
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

        public Pedido CriarPedido(DateTime Data, Cliente cliente, string status)
        {
            var pedido = new Pedido(0, Data, cliente, status);
            pedidoRepository.Create(pedido);
            return pedido;
        }

        public void AdicionarItemPedido(int pedidoId, int produtoId, int quantidade, decimal precoUnitario)
        {
            var pedido = pedidoRepository.GetById(pedidoId);
            Console.WriteLine("pedidoClienteNome: " + pedido?.Cliente?.Nome + "; produtoId: " + produtoId);
            var produtoRepository = new ProdutoRepository("server=localhost;database=poo_game3;user=root;password=;");
            var produto = produtoRepository.GetById(produtoId);

            if (pedido != null && produto != null)
            {
                var itemPedido = new ItemPedido(0, produto, quantidade, precoUnitario, pedido);
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

        public List<Pedido> ListarPedidosPorCliente(Cliente cliente)
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
            var itemPedidoRepository = new ItemPedidoRepository("server=localhost;database=poo_game3;user=root;password=;");
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