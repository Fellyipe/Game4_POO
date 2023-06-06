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

public class Repository<T> : IRepository<T>
{
    private readonly string _connectionString;

    public Repository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void Create(T entity)
    {
        string tableName = typeof(T).Name.ToLower();
        var properties = typeof(T).GetProperties().Where(p => !p.Name.Equals("Id"));
        string columns = string.Join(", ", properties.Select(p => p.Name));
        string values = string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));

        string query = $"INSERT INTO {tableName} ({columns}) VALUES ({values});";

        using (var connection = new MySqlConnection(_connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
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

    public void Update(T entity)
    {
        string tableName = typeof(T).Name.ToLower();

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
        }
    }

    public void Delete(int id)
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

    public T GetById(int id)
    {
        string tableName = typeof(T).Name.ToLower();

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
                            }
                        }
                        return entity;
                    }
                }
            }    
        }
        return default(T);
    }

    public IEnumerable<T> ListAll()
    {
        string tableName = typeof(T).Name.ToLower();

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