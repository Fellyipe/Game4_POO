using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using MySql.Data;
using MySql.Data.MySqlClient;
using Gamificacao4;
using Gamificacao4.Interfaces;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

public class Repository<T> : IRepository<T>
{
    private readonly string _connectionString;

    public Repository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Create<T>(T entity)
    {
        /*string tableName = typeof(T).Name.ToLower();
        var properties = typeof(T).GetProperties().Where(p => !p.Name.Equals("Id"));
        string columns = string.Join(", ", properties.Select(p => p.Name));
        string values = string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));
        Console.WriteLine(properties);

        string query = $"INSERT INTO tb_{tableName} ({columns}) VALUES ({values});";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }*/
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            var queryBuilder = new StringBuilder($"INSERT INTO tb_{typeof(T).Name} (");
            var valuesBuilder = new StringBuilder("VALUES (");
            var parameters = new List<MySqlParameter>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "Id" || property.GetValue(entity) == null)
                    continue;

                string columnName = property.Name;
                object value = property.GetValue(entity);
                if (columnName == "Produto")
                {
                    columnName = "tb_produtoId";
                    value = ((Gamificacao4.Produto)value).Id;
                }
                if (columnName == "Pedido")
                {
                    columnName = "tb_pedidoId";
                    value = ((Gamificacao4.Pedido)value).Id;
                }
                queryBuilder.Append($"{columnName}, ");
                valuesBuilder.Append($"@{columnName}, ");

                var parameter = new MySqlParameter($"@{columnName}", value);
                parameters.Add(parameter);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            valuesBuilder.Remove(valuesBuilder.Length - 2, 2);

            queryBuilder.Append(") ");
            valuesBuilder.Append(")");

            var query = queryBuilder.ToString() + valuesBuilder.ToString();
            var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());
            command.ExecuteNonQuery();
            long id = command.LastInsertedId;
            Console.WriteLine("Id do objeto criado: " + id);
        }
    }   
        /*using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                var queryBuilder = new StringBuilder($"INSERT INTO tb_{typeof(T).Name} (");
                var valuesBuilder = new StringBuilder("VALUES (");
                var parameters = new List<MySqlParameter>();

                var properties = typeof(T).GetProperties();

                foreach (var property in properties)
                {
                    if (property.Name == "Id" || property.GetValue(entity) == null)
                        continue;

                    string columnName = property.Name.ToLower();
                    object value = property.GetValue(entity);

                    queryBuilder.Append($"{columnName}, ");
                    valuesBuilder.Append($"@{columnName}, ");

                    var parameter = new MySqlParameter($"@{columnName}", value);
                    parameters.Add(parameter);
                }

                queryBuilder.Remove(queryBuilder.Length - 2, 2);
                valuesBuilder.Remove(valuesBuilder.Length - 2, 2);

                queryBuilder.Append(") ");
                valuesBuilder.Append(")");

                var query = queryBuilder.ToString() + valuesBuilder.ToString();
                var command = new MySqlCommand(query, connection);
                command.Parameters.AddRange(parameters.ToArray());
                command.ExecuteNonQuery();
            }
                */

    public void Update<T>(T entity)
    {
        /*string tableName = typeof(T).Name.ToLower();

        var properties = typeof(T).GetProperties().Where(p => !p.Name.Equals("Id"));
        var id = typeof(T).GetProperty("Id").GetValue(entity);

        string query = $"UPDATE tb_{tableName} SET {string.Join(", ", properties.Select(p => $"{p.Name} = '{p.GetValue(entity)}'"))} WHERE Id = {id};";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }*/
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string tableName = typeof(T).Name.ToLower();
            var queryBuilder = new StringBuilder($"UPDATE tb_{tableName} SET ");
            var parameters = new List<MySqlParameter>();

            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                if (property.Name == "Id" || property.GetValue(entity) == null)
                    continue;

                string columnName = property.Name;
                object value = property.GetValue(entity);
                if (columnName == "Produto")
                {
                    columnName = "tb_produtoId";
                    value = ((Gamificacao4.Produto)value).Id;
                }
                if (columnName == "Pedido")
                {
                    columnName = "tb_pedidoId";
                    value = ((Gamificacao4.Pedido)value).Id;
                }
                queryBuilder.Append($"{columnName} = @{columnName}, ");

                var parameter = new MySqlParameter($"@{columnName}", value);
                parameters.Add(parameter);
            }

            queryBuilder.Remove(queryBuilder.Length - 2, 2);
            queryBuilder.Append(" WHERE Id = @Id;");

            var query = queryBuilder.ToString();

            var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());
            command.Parameters.AddWithValue("@Id", properties.First(p => p.Name == "Id").GetValue(entity));
            command.ExecuteNonQuery();
        }
    }

    public void Delete<T>(int id)
    {
        string tableName = typeof(T).Name.ToLower();

        string query = $"DELETE FROM tb_{tableName} WHERE Id = {id};";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public T GetById<T>(int id)
    {
        /*string tableName = typeof(T).Name.ToLower();

        string query = $"SELECT * FROM tb_{tableName} WHERE Id = {id};";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        T entity = Activator.CreateInstance<T>();

                        foreach (var property in typeof(T).GetProperties())
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(entity, reader[property.Name]);
                                Console.WriteLine(property);
                            }
                        }
                        return entity;
                    }
                }
            }    
        }
        return default(T);*/
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string tableName = typeof(T).Name.ToLower();
            string query = $"SELECT * FROM tb_{tableName} WHERE Id = {id};";
            var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Id", id);

            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    var entity = Activator.CreateInstance<T>();
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.Name == "Id")
                        {
                            property.SetValue(entity, id);
                            continue;
                        }
                        if (property.Name == "Produto")
                        {
                            var produtoId = reader.GetInt32("tb_produtoId");
                            var produto = GetById<Produto>(produtoId);
                            property.SetValue(entity, produto);
                            continue;
                        }
                        if (property.Name == "Pedido")
                        {
                            var pedidoId = reader.GetInt32("tb_pedidoId");
                            var pedido = GetById<Pedido>(pedidoId);
                            property.SetValue(entity, pedido);
                            continue;
                        }

                        var value = reader[property.Name];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(entity, value);
                        }
                    }

                    return entity;
                }
            }
        }
        return default;
    }

    public IEnumerable<T> ListAll<T>()
    {
        /*string tableName = typeof(T).Name.ToLower();

        string query = $"SELECT * FROM tb_{tableName};";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    var entities = new List<T>();

                    while (reader.Read())
                    {
                        var entity = Activator.CreateInstance<T>();
                        var properties = typeof(T).GetProperties();
                        foreach (var property in properties)
                        {
                            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
                            {
                                property.SetValue(entity, reader[property.Name]);
                            }
                        }
                        entities.Add(entity);
                    }

                    return entities;
                }
            }
        }*/
        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();
            string tableName = typeof(T).Name.ToLower();
            string query = $"SELECT * FROM tb_{tableName};";
            var command = new MySqlCommand(query, connection);

            var entities = new List<T>();

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var entity = Activator.CreateInstance<T>();
                    var properties = typeof(T).GetProperties();

                    foreach (var property in properties)
                    {
                        if (property.Name == "Produto")
                        {
                            var id = reader.GetInt32("tb_produtoId");
                            var produto = GetById<Produto>(id);
                            property.SetValue(entity, produto);
                            continue;
                        }
                        if (property.Name == "Pedido")
                        {
                            var id = reader.GetInt32("tb_pedidoId");
                            var pedido = GetById<Pedido>(id);
                            property.SetValue(entity, pedido);
                            continue;
                        }

                        var value = reader[property.Name];
                        if (value != DBNull.Value)
                        {
                            property.SetValue(entity, value);
                        }
                    }

                    entities.Add(entity);
                }
            }

            return entities;
        }
    }

    // private string GetTableName()
    // {
    //     var attribute = typeof(T).GetCustomAttribute<TableNameAttribute>();
    //     return attribute?.Name ?? typeof(T).Name;
    // }

    // private string GetPrimaryKeyColumnName()
    // {
    //     var property = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
    //     return property?.Name;
    // }

    // private string GetPrimaryKeyColumnValue(T entity)
    // {
    //     var property = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
    //     return property?.GetValue(entity)?.ToString();
    // }
    /*    private T CreateEntityFromReader(MySqlDataReader reader)
    {
        T entity = Activator.CreateInstance<T>();

        foreach (var property in typeof(T).GetProperties())
        {
            if (!reader.IsDBNull(reader.GetOrdinal(property.Name)))
            {
                property.SetValue(entity, reader[property.Name]);
            }
        }

        return entity;
    }
    */
}