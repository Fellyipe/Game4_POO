using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using MySql.Data;
using MySql.Data.MySqlClient;

// Implementação do repositório utilizando ADO.NET

/*
Adaptar a Gamificação 3 para uso de Genérics e Reflexão
*/


/* ------------- ALUNOS -------------------
ÍSIS YASMIM
GUILHERME FAVERO
FELIPE BUENO
*/
using Gamificacao4;
using Gamificacao4.Interfaces;


namespace Gamificacao4
{
    class Program
    {
        public static void Main(string[] args)
        {
            string connectionString = "server=localhost;database=poo_game4;user=root;password=;";
            
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("Conexão com o banco de dados estabelecida com sucesso.");

                string query = "SELECT SYSDATE() AS SYSDATE";
                MySqlCommand comando = new MySqlCommand(query, connection);

                using (MySqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        DateTime sysDate = reader.GetDateTime("SYSDATE");
                        Console.WriteLine(sysDate);
                    }
                }

                var pedidoRepository = new PedidoRepository(connectionString);
                var itemPedidoRepository = new ItemPedidoRepository(connectionString);
                var produtoRepository = new ProdutoRepository(connectionString);
                var repositoryPedido = new Repository<Pedido>(connectionString);
                var repositoryItemPedido = new Repository<ItemPedido>(connectionString);
                var gerenciamentoDePedidos = new GerenciamentoDePedidos(pedidoRepository, itemPedidoRepository);
                var menu = new Menu(connectionString);
                
                bool sair = false;

                while (!sair)
                {
                    Console.WriteLine("====== MENU ======");
                    Console.WriteLine("1. Criar um novo pedido");
                    Console.WriteLine("2. Adicionar itens a um pedido");
                    Console.WriteLine("3. Atualizar o status de um pedido");
                    Console.WriteLine("4. Remover um pedido");
                    Console.WriteLine("5. Listar pedidos por cliente, status ou data");
                    Console.WriteLine("6. Calcular o valor total de um pedido");
                    Console.WriteLine("7. Acesso administrador (CRUD de produtos, pedidos e item pedidos)");
                    Console.WriteLine("8. Listar todos os produtos");
                    Console.WriteLine("0. Sair");
                    Console.WriteLine("==================");
                    Console.Write("Escolha uma opção: ");

                    string opcao = Console.ReadLine();
                    Console.WriteLine();

                    switch (opcao)
                    {
                        case "1":
                            menu.CriarNovoPedido(gerenciamentoDePedidos);
                            break;
                        case "2":
                            menu.AdicionarItensAoPedido(gerenciamentoDePedidos);
                            break;
                        case "3":
                            menu.AtualizarStatusPedido(gerenciamentoDePedidos);
                            break;
                        case "4":
                            menu.RemoverPedido(gerenciamentoDePedidos);
                            break;
                        case "5":
                            menu.ListarPedidos(gerenciamentoDePedidos);
                            break;
                        case "6":
                            menu.CalcularValorTotalPedido(gerenciamentoDePedidos);
                            break;
                        case "7":
                            menu.AcessoAdministrador(gerenciamentoDePedidos, produtoRepository, pedidoRepository, itemPedidoRepository);
                            break;
                        case "8":
                            menu.ListarTodosProdutos(produtoRepository);
                            Console.WriteLine("Aperte qualquer tecla para continuar");
                            Console.ReadKey();
                            break;
                        case "0":
                            sair = true;
                            break;
                        default:
                            Console.WriteLine("Opção inválida. Por favor, escolha uma opção válida.");
                            Console.WriteLine("Aperte qualquer tecla para continuar");
                            Console.ReadKey();
                            break;
                    }
                    Console.WriteLine();
                }
                connection.Close();
                Console.WriteLine("Conexão com o banco de dados encerrada.");                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar ao banco de dados: " + ex.Message);
            }
        }
    }
}