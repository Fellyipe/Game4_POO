using Gamificacao3;
using Gamificacao3.Interfaces;
using MySql.Data;
using MySql.Data.MySqlClient;

public class ProdutoRepository : IProdutoRepository
{
    private readonly string connectionString;

    public ProdutoRepository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Create(Produto produto)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "INSERT INTO tb_produto (Nome, Descricao, Preco, QuantidadeEmEstoque) " +
                        "VALUES (@Nome, @Descricao, @Preco, @QuantidadeEmEstoque); SELECT LAST_INSERT_ID();";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nome", produto.Nome);
                command.Parameters.AddWithValue("@Descricao", produto.Descricao);
                command.Parameters.AddWithValue("@Preco", produto.Preco);
                command.Parameters.AddWithValue("@QuantidadeEmEstoque", produto.QuantidadeEmEstoque);

                produto.Id = Convert.ToInt32(command.ExecuteScalar());
                //command.ExecuteNonQuery();
            }
        }
    }

    public void Update(Produto produto)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "UPDATE tb_produto SET Nome = @Nome, Descricao = @Descricao, " +
                        "Preco = @Preco, QuantidadeEmEstoque = @QuantidadeEmEstoque " +
                        "WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Nome", produto.Nome);
                command.Parameters.AddWithValue("@Descricao", produto.Descricao);
                command.Parameters.AddWithValue("@Preco", produto.Preco);
                command.Parameters.AddWithValue("@QuantidadeEmEstoque", produto.QuantidadeEmEstoque);
                command.Parameters.AddWithValue("@Id", produto.Id);

                command.ExecuteNonQuery();
            }
        }
    }

    public void Delete(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "DELETE FROM tb_produto WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();
            }
        }
    }

    public Produto? GetById(int id)
    {
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_produto WHERE Id = @Id";
            using (var command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var produtoId = reader.GetInt32(0);
                        var nome = reader.GetString(1);
                        var descricao = reader.GetString(2);
                        var preco = reader.GetDecimal(3);
                        var quantidadeEmEstoque = reader.GetInt32(4);

                        return new Produto(produtoId, nome, descricao, preco, quantidadeEmEstoque);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }

    public IEnumerable<Produto> ListAll()
    {
        var produtos = new List<Produto>();

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var query = "SELECT * FROM tb_produto";
            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var produtoId = reader.GetInt32(0);
                        var nome = reader.GetString(1);
                        var descricao = reader.GetString(2);
                        var preco = reader.GetDecimal(3);
                        var quantidadeEmEstoque = reader.GetInt32(4);

                        var produto = new Produto(produtoId, nome, descricao, preco, quantidadeEmEstoque);
                        produtos.Add(produto);
                    }
                }
            }
        }

        return produtos;
    }
}
