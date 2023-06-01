using Gamificacao3;
using Gamificacao3.Interfaces;
using MySql.Data;
using MySql.Data.MySqlClient;

public class PedidoRepository : IPedidoRepository
{
    private readonly string connectionString;

    public PedidoRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public Pedido? GetById(int id)
    {
        var connection = new MySqlConnection(connectionString);
        
        connection.Open();
        var query = "SELECT * FROM tb_pedido WHERE Id = @Id";
        var command = new MySqlCommand(query, connection);
            
        command.Parameters.AddWithValue("@Id", id);
        var reader = command.ExecuteReader();
                
        if (reader.Read())
        {
            var pedidoId = reader.GetInt32(0);
            var Data = reader.GetDateTime(1);
            var cliente = new Cliente(reader.GetString(2));
            var status = reader.GetString(3);

            return new Pedido(pedidoId, Data, cliente, status);
        }
                
            
        return null;
    }

    public void Create(Pedido pedido)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO tb_pedido (Data, Cliente, Status) " +
                        "VALUES (@Data, @Cliente, @Status); SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Data", pedido?.Data);
                command.Parameters.AddWithValue("@Cliente", pedido?.Cliente?.Nome);
                command.Parameters.AddWithValue("@Status", pedido?.Status);
                if (pedido != null)
                {
                    pedido.Id = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }
    }

    public void Update(Pedido pedido)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE tb_pedido SET Data = @Data, Cliente = @Cliente, Status = @Status WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Data", pedido?.Data);
                command.Parameters.AddWithValue("@Cliente", pedido?.Cliente?.Nome);
                command.Parameters.AddWithValue("@Status", pedido?.Status);
                command.Parameters.AddWithValue("@Id", pedido?.Id);

                command.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM tb_pedido WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);
                command.ExecuteNonQuery();
            }
        }
    }

    public List<Pedido> GetByCliente(Cliente cliente)
    {
        var pedidos = new List<Pedido>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_pedido WHERE Cliente = @Cliente";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Cliente", cliente.Nome);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedidoId = reader.GetInt32(0);
                        var Data = reader.GetDateTime(1);
                        var status = reader.GetString(3);

                        pedidos.Add(new Pedido(pedidoId, Data, cliente, status));
                    }
                }
            }
        }
        return pedidos;
    }

    public List<Pedido> GetByStatus(string status)
    {
        var pedidos = new List<Pedido>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_pedido WHERE Status = @Status";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Status", status);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedidoId = reader.GetInt32(0);
                        var Data = reader.GetDateTime(1);
                        var cliente = new Cliente(reader.GetString(2));

                        pedidos.Add(new Pedido(pedidoId, Data, cliente, status));
                    }
                }
            }
        }
        return pedidos;
    }

    public List<Pedido> GetByData(DateTime data)
    {
        var pedidos = new List<Pedido>();
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_pedido WHERE Data = @Data";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Data", data);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedidoId = reader.GetInt32(0);
                        var cliente = new Cliente(reader.GetString(2));
                        var status = reader.GetString(3);

                        pedidos.Add(new Pedido(pedidoId, data, cliente, status));
                    }
                }
            }
        }
        return pedidos;
    }
    
    public IEnumerable<Pedido> ListAll()
    {
        var pedidos = new List<Pedido>();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_pedido";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var pedidoId = reader.GetInt32(0);
                        var Data = reader.GetDateTime(1);
                        var cliente = new Cliente(reader.GetString(2));
                        var status = reader.GetString(3);

                        var pedido = new Pedido(pedidoId, Data, cliente, status);
                        pedidos.Add(pedido);
                    }
                }
            }
        }

        return pedidos;
    }



}
