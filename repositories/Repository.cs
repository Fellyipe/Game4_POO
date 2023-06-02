using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using MySql.Data;
using MySql.Data.MySqlClient;

public class Repository<T> where T : class
{
    private readonly string connectionString;

    public Repository(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public void Create(T entity)
    {
        string tableName = typeof(T).Name.ToLower();
        string columns = GetColumnNames();
        string values = GetColumnValues(entity);

        string query = $"INSERT INTO tb_{tableName} ({columns}) VALUES ({values});";

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
    }

    public void Update(T entity)
    {
        string tableName = typeof(T).Name.ToLower();
        string keyColumn = GetPrimaryKeyColumnName();
        string keyColumnValue = GetPrimaryKeyColumnValue(entity);

        string updateColumns = GetUpdateColumnNamesAndValues(entity);

        string query = $"UPDATE tb_{tableName} SET {updateColumns} WHERE Id = {entity.Id};";

        using (var connection = new MySqlConnection(connectionString))
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

        using (var connection = new MySqlConnection(connectionString))
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

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        T entity = CreateEntityFromReader(reader);
                        return entity;
                    }
                }
            }
        }
        return null;
    }

    public IEnumerable<T> GetAll()
    {
        string tableName = typeof(T).Name.ToLower();

        string query = $"SELECT * FROM tb_{tableName};";

        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();

            using (var command = new MySqlCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    List<T> entities = new List<T>();

                    while (reader.Read())
                    {
                        T entity = CreateEntityFromReader(reader);
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

    private string GetColumnNames()
    {
        var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() == null);
        return string.Join(", ", properties.Select(p => p.Name));
    }

    private string GetColumnValues(T entity)
    {
        var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() == null);
        return string.Join(", ", properties.Select(p => $"'{p.GetValue(entity)}'"));
    }

    private string GetUpdateColumnNamesAndValues(T entity)
    {
        var properties = typeof(T).GetProperties().Where(p => p.GetCustomAttribute<KeyAttribute>() == null);
        return string.Join(", ", properties.Select(p => $"{p.Name} = '{p.GetValue(entity)}'"));
    }

    // private string GetPrimaryKeyColumnValue(T entity)
    // {
    //     var property = typeof(T).GetProperties().FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null);
    //     return property?.GetValue(entity)?.ToString();
    // }

    private T CreateEntityFromReader(MySqlDataReader reader)
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
