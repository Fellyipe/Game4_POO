using Gamificacao3;
using Gamificacao3.Interfaces;

namespace Gamificacao3
{
    public class Menu
    {
        private readonly string connectionString;

        public Menu(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void CriarNovoPedido(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== CRIAR NOVO PEDIDO =====");
            Console.Write("Data do pedido: ");
            DateTime data = DateTime.Parse(Console.ReadLine());
            Console.Write("Cliente: ");
            var cliente = new Cliente(Console.ReadLine());
            Console.Write("Status do pedido (Pendente, Pago, Enviado, Entregue): ");
            string ?status = Console.ReadLine();

            gerenciamentoDePedidos.CriarPedido(data, cliente, status);

            Console.WriteLine("Pedido criado com sucesso!");
        }

        public void AdicionarItensAoPedido(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== ADICIONAR ITENS AO PEDIDO =====");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());
            Console.Write("ID do produto: ");
            int produtoId = int.Parse(Console.ReadLine());
            var produtoRepository = new ProdutoRepository(connectionString);
            var produto = produtoRepository.GetById(produtoId);
            Console.Write("Quantidade: ");
            int quantidade = int.Parse(Console.ReadLine());

            gerenciamentoDePedidos.AdicionarItemPedido(pedidoId, produtoId, quantidade, produto.Preco);
            Console.WriteLine("Item adicionado ao pedido com sucesso!");

        }

        public void AtualizarStatusPedido(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== ATUALIZAR STATUS DO PEDIDO =====");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());
            Console.Write("Novo status (Pendente, Pago, Enviado, Entregue): ");
            string novoStatus = Console.ReadLine();

            gerenciamentoDePedidos.AtualizarStatusPedido(pedidoId, novoStatus);

            Console.WriteLine("Status do pedido atualizado com sucesso!");
        }

        public void RemoverPedido(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== REMOVER PEDIDO =====");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());

            gerenciamentoDePedidos.RemoverPedido(pedidoId);

            Console.WriteLine("Pedido removido com sucesso!");
        }

        public void ListarPedidos(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== LISTAR PEDIDOS =====");
            Console.WriteLine("Escolha uma opção:");
            Console.WriteLine("1. Listar por cliente");
            Console.WriteLine("2. Listar por status");
            Console.WriteLine("3. Listar por data");
            Console.WriteLine("4. Voltar");

            string opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1":
                    Console.Write("Cliente: ");
                    var cliente = new Cliente(Console.ReadLine());
                    var pedidosPorCliente = gerenciamentoDePedidos.ListarPedidosPorCliente(cliente);
                    Console.WriteLine("Pedidos encontrados:");
                    foreach (var pedido in pedidosPorCliente)
                    {
                        Console.WriteLine("Cliente: " + pedido?.Cliente?.Nome + "; Data: " + pedido?.Data.ToString("dd/MM/yyyy") + "; Status: " + pedido?.Status);
                    }
                    break;
                case "2":
                    Console.Write("Status: ");
                    string status = Console.ReadLine();
                    var pedidosPorStatus = gerenciamentoDePedidos.ListarPedidosPorStatus(status);
                    Console.WriteLine("Pedidos encontrados:");
                    foreach (var pedido in pedidosPorStatus)
                    {
                        Console.WriteLine("Cliente: " + pedido.Cliente.Nome + "; Data: " + pedido.Data.ToString("dd/MM/yyyy") + "; Status: " + pedido.Status);
                    }
                    break;
                case "3":
                    Console.Write("Digite a data formato (DD/MM/AAAA): ");
                    DateTime data = DateTime.Parse(Console.ReadLine());
                    var pedidosPorData = gerenciamentoDePedidos.ListarPedidosPorData(data);
                    Console.WriteLine("Pedidos encontrados:");
                    foreach (var pedido in pedidosPorData)
                    {
                        Console.WriteLine("Cliente: " + pedido.Cliente.Nome + "; Data: " + pedido.Data.ToString("dd/MM/yyyy") + "; Status: " + pedido.Status);
                    }
                    break;
                case "4":
                    break;
                default:
                    Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                    break;
            }
        }

        public void CalcularValorTotalPedido(GerenciamentoDePedidos gerenciamentoDePedidos)
        {
            Console.WriteLine("===== CALCULAR VALOR TOTAL DO PEDIDO =====");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());

            decimal valorTotal = gerenciamentoDePedidos.CalcularValorTotalPedido(pedidoId);

            Console.WriteLine($"Valor total do pedido: R${valorTotal:F2}");
        }

        public void AcessoAdministrador(GerenciamentoDePedidos gerenciamentoDePedidos, ProdutoRepository produtoRepository, PedidoRepository pedidoRepository, ItemPedidoRepository itemPedidoRepository)
        {
            bool sair = false;

            while (!sair)
            {
                Console.WriteLine("===== ACESSO ADMINISTRADOR =====");
                Console.WriteLine("1. Criar novo produto");
                Console.WriteLine("2. Ler um produto");
                Console.WriteLine("3. Atualizar produto");
                Console.WriteLine("4. Remover produto");
                Console.WriteLine("5. Listar todos os produtos");
                Console.WriteLine("6. Criar novo pedido");
                Console.WriteLine("7. Ler um pedido");
                Console.WriteLine("8. Atualizar pedido");
                Console.WriteLine("9. Remover pedido");
                Console.WriteLine("10. Listar todos os pedidos");
                Console.WriteLine("11. Criar novo item pedido");
                Console.WriteLine("12. Ler um item pedido");
                Console.WriteLine("13. Atualizar item pedido");
                Console.WriteLine("14. Remover item pedido");
                Console.WriteLine("15. Listar todos os itens pedido");
                Console.WriteLine("0. Voltar");
                Console.WriteLine("===============================");
                Console.Write("Escolha uma opção: ");

                string opcao = Console.ReadLine();
                Console.WriteLine();

                switch (opcao)
                {
                    case "1":
                        CriarNovoProduto(produtoRepository);
                        break;
                    case "2":
                        LerProduto(produtoRepository);
                        break;
                    case "3":
                        AtualizarProduto(produtoRepository);
                        break;
                    case "4":
                        RemoverProduto(produtoRepository);
                        break;
                    case "5":
                        ListarTodosProdutos(produtoRepository);
                        break;
                    case "6":
                        CriarNovoPedido(pedidoRepository);
                        break;
                    case "7":
                        LerPedido(pedidoRepository);
                        break;
                    case "8":
                        AtualizarPedido(pedidoRepository);
                        break;
                    case "9":
                        RemoverPedido(pedidoRepository);
                        break;
                    case "10":
                        ListarTodosPedidos(pedidoRepository);
                        break;
                    case "11":
                        CriarNovoItemPedido(itemPedidoRepository);
                        break;
                    case "12":
                        LerItemPedido(itemPedidoRepository);
                        break;
                    case "13":
                        AtualizarItemPedido(itemPedidoRepository);
                        break;
                    case "14":
                        RemoverItemPedido(itemPedidoRepository);
                        break;
                    case "15":
                        ListarTodosItemPedidos(itemPedidoRepository);
                        break;
                    case "0":
                        sair = true;
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                        break;
                }

                Console.WriteLine();
            }
        }

        public void CriarNovoProduto(ProdutoRepository produtoRepository)
        {
            Console.WriteLine("===== CRIAR NOVO PRODUTO =====");
            Console.Write("Nome do produto: ");
            string nome = Console.ReadLine();
            Console.Write("Descrição do produto: ");
            string descricao = Console.ReadLine();
            Console.Write("Preço do produto: ");
            decimal preco = decimal.Parse(Console.ReadLine());
            Console.Write("Quantidade em estoque: ");
            int quantidade = int.Parse(Console.ReadLine());

            Produto produto = new Produto(0, nome, descricao, preco, quantidade);
            produtoRepository.Create(produto);

            Console.WriteLine("Produto criado com sucesso!");
        }

        public void LerProduto(ProdutoRepository produtoRepository)
        {
            Console.WriteLine("===== LER PRODUTO =====");
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());
            var produto = produtoRepository.GetById(id);

            if (produto != null)
            {
                Console.WriteLine("ID: " + produto.Id + "; Nome: " + produto.Nome + "; Descrição: " + produto.Descricao + "; Preço: " + produto.Preco + "; Quantidade em estoque: " + produto.QuantidadeEmEstoque);
            }
            else
            {
                Console.WriteLine("Nenhum produto encontrado!");
            }
        }

        public void AtualizarProduto(ProdutoRepository produtoRepository)
        {
            Console.WriteLine("===== ATUALIZAR PRODUTO =====");
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());

            Produto produto = produtoRepository.GetById(id);
            if (produto != null)
            {
                Console.Write("Novo nome do produto: ");
                string nome = Console.ReadLine();
                Console.Write("Nova descrição do produto: ");
                string descricao = Console.ReadLine();
                Console.Write("Novo preço do produto: ");
                decimal preco = decimal.Parse(Console.ReadLine());
                Console.Write("Nova quantidade em estoque: ");
                int quantidade = int.Parse(Console.ReadLine());

                produto.Nome = nome;
                produto.Descricao = descricao;
                produto.Preco = preco;
                produto.QuantidadeEmEstoque = quantidade;

                produtoRepository.Update(produto);

                Console.WriteLine("Produto atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Produto não encontrado!");
            }
        }

        public void RemoverProduto(ProdutoRepository produtoRepository)
        {
            Console.WriteLine("===== REMOVER PRODUTO =====");
            Console.Write("ID do produto: ");
            int id = int.Parse(Console.ReadLine());

            produtoRepository.Delete(id);

            Console.WriteLine("Produto removido com sucesso!");
        }

        public void ListarTodosProdutos(ProdutoRepository produtoRepository)
        {
            Console.WriteLine("===== LISTAR TODOS OS PRODUTOS =====");
            var produtos = produtoRepository.ListAll();

            if (produtos.Any())
            {
                foreach (var produto in produtos)
                {
                    Console.WriteLine("ID: " + produto.Id + "; Nome: " + produto.Nome + "; Descrição: " + produto.Descricao + "; Preço: " + produto.Preco + "; Quantidade em estoque: " + produto.QuantidadeEmEstoque);
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto encontrado!");
            }
        }
        public void CriarNovoPedido(PedidoRepository pedidoRepository)
        {
            Console.WriteLine("===== CRIAR NOVO PEDIDO =====");
            Console.Write("Data do pedido: ");
            DateTime data = DateTime.Parse(Console.ReadLine());
            Console.Write("Cliente: ");
            var cliente = new Cliente(Console.ReadLine());
            Console.Write("Status do pedido: ");
            string ?status = Console.ReadLine();

            Pedido pedido = new Pedido(0, data, cliente, status);
            pedidoRepository.Create(pedido);

            Console.WriteLine("Pedido criado com sucesso!");
        }

        public void LerPedido(PedidoRepository pedidoRepository)
        {
            Console.WriteLine("===== LER PEDIDO =====");
            Console.Write("ID do pedido: ");
            int id = int.Parse(Console.ReadLine());
            var pedido = pedidoRepository.GetById(id);

            if (pedido != null)
            {
                Console.WriteLine("ID: " + pedido.Id + "; Data: " + pedido.Data.ToString("dd/MM/yyyy") + "; Cliente: " + pedido?.Cliente?.Nome + "; Status: " + pedido?.Status);
            }
            else
            {
                Console.WriteLine("Nenhum pedido encontrado!");
            }
        }

        public void AtualizarPedido(PedidoRepository pedidoRepository)
        {
            Console.WriteLine("===== ATUALIZAR PEDIDO =====");
            Console.Write("ID do pedido: ");
            int id = int.Parse(Console.ReadLine());

            Pedido pedido = pedidoRepository.GetById(id);
            if (pedido != null)
            {
                Console.Write("Nova data do pedido: ");
                DateTime data = DateTime.Parse(Console.ReadLine());
                Console.Write("Novo cliente do pedido: ");
                var cliente = new Cliente(Console.ReadLine());
                Console.Write("Novo status do pedido: ");
                string ?status = Console.ReadLine();

                pedido.Data = data;
                pedido.Cliente = cliente;
                pedido.Status = status;

                pedidoRepository.Update(pedido);

                Console.WriteLine("Pedido atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Pedido não encontrado!");
            }
        }

        public void RemoverPedido(PedidoRepository pedidoRepository)
        {
            Console.WriteLine("===== REMOVER PEDIDO =====");
            Console.Write("ID do pedido: ");
            int id = int.Parse(Console.ReadLine());

            pedidoRepository.Delete(id);

            Console.WriteLine("Pedido removido com sucesso!");
        }

        public void ListarTodosPedidos(PedidoRepository pedidoRepository)
        {
            Console.WriteLine("===== LISTAR TODOS OS PEDIDOS =====");
            var pedidos = pedidoRepository.ListAll();

            if (pedidos.Any())
            {
                foreach (var pedido in pedidos)
                {
                    Console.WriteLine("ID: " + pedido.Id + "; Data: " + pedido.Data.ToString("dd/MM/yyyy") + "; Cliente: " + pedido.Cliente.Nome + "; Status: " + pedido.Status);
                }
            }
            else
            {
                Console.WriteLine("Nenhum pedido encontrado!");
            }
        }
        public void CriarNovoItemPedido(ItemPedidoRepository itemPedidoRepository)
        {
            Console.WriteLine("===== CRIAR NOVO ITEM PEDIDO =====");
            Console.Write("ID do pedido: ");
            int pedidoId = int.Parse(Console.ReadLine());
            Console.Write("ID do produto: ");
            int produtoId = int.Parse(Console.ReadLine());
            /*Console.Write("Preço unitário do produto: ");
            decimal precoUnitario = decimal.Parse(Console.ReadLine());*/
            Console.Write("Quantidade: ");
            int quantidade = int.Parse(Console.ReadLine());
            var pedidoRepository = new PedidoRepository(connectionString);
            var produtoRepository = new ProdutoRepository(connectionString);
            var pedido = pedidoRepository.GetById(pedidoId);
            var produto = produtoRepository.GetById(produtoId);
            
            ItemPedido itemPedido = new ItemPedido(0, produto, quantidade, produto.Preco, pedido);
            itemPedidoRepository.Create(itemPedido);

            Console.WriteLine("Item pedido criado com sucesso!");
        }

        public void LerItemPedido(ItemPedidoRepository itemPedidoRepository)
        {
            Console.WriteLine("===== LER ITEM PEDIDO =====");
            Console.Write("ID do item pedido: ");
            int id = int.Parse(Console.ReadLine());
            var itemPedido = itemPedidoRepository.GetById(id);

            if (itemPedido != null)
            {
                Console.WriteLine("ID: " + itemPedido.Id + "; ID do produto: " + itemPedido.Produto.Id + "; Quantidade: " + itemPedido.Quantidade + "; Preço unitário: " + itemPedido.PrecoUnitario + "; ID do pedido: " + itemPedido.Pedido.Id);
            }
            else
            {
                Console.WriteLine("Nenhum item pedido encontrado!");
            }
        }

        public void AtualizarItemPedido(ItemPedidoRepository itemPedidoRepository)
        {
            Console.WriteLine("===== ATUALIZAR ITEM PEDIDO =====");
            Console.Write("ID do item pedido: ");
            int id = int.Parse(Console.ReadLine());

            ItemPedido itemPedido = itemPedidoRepository.GetById(id);
            if (itemPedido != null)
            {
                Console.Write("Novo ID do pedido: ");
                int pedidoId = int.Parse(Console.ReadLine());
                Console.Write("Novo ID do produto: ");
                int produtoId = int.Parse(Console.ReadLine());
                /*Console.Write("Preço unitário do produto: ");
                decimal precoUnitario = decimal.Parse(Console.ReadLine());*/
                Console.Write("Nova Quantidade: ");
                int quantidade = int.Parse(Console.ReadLine());

                var pedidoRepository = new PedidoRepository(connectionString);
                var produtoRepository = new ProdutoRepository(connectionString);
                var pedido = pedidoRepository.GetById(pedidoId);
                var produto = produtoRepository.GetById(produtoId);

                itemPedido.Pedido = pedido;
                itemPedido.Quantidade = quantidade;
                itemPedido.PrecoUnitario = produto.Preco;
                itemPedido.Produto = produto;

                itemPedidoRepository.Update(itemPedido);

                Console.WriteLine("Item pedido atualizado com sucesso!");
            }
            else
            {
                Console.WriteLine("Item pedido não encontrado!");
            }
        }

        public void RemoverItemPedido(ItemPedidoRepository itemPedidoRepository)
        {
            Console.WriteLine("===== REMOVER ITEM PEDIDO =====");
            Console.Write("ID do item pedido: ");
            int id = int.Parse(Console.ReadLine());

            itemPedidoRepository.Delete(id);

            Console.WriteLine("Item pedido removido com sucesso!");
        }

        public void ListarTodosItemPedidos(ItemPedidoRepository itemPedidoRepository)
        {
            Console.WriteLine("===== LISTAR TODOS OS ITEM PEDIDOS =====");
            var itemPedidos = itemPedidoRepository.ListAll();

            if (itemPedidos.Any())
            {
                foreach (var itemPedido in itemPedidos)
                {
                    Console.WriteLine("ID: " + itemPedido.Id + "; ID do produto: " + itemPedido.Produto.Id + "; Quantidade: " + itemPedido.Quantidade + "; Preço unitário: " + itemPedido.PrecoUnitario + "; ID do pedido: " + itemPedido.Pedido.Id);
                }
            }
            else
            {
                Console.WriteLine("Nenhum produto encontrado!");
            }
        }

    }
}