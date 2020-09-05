using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;

namespace Emergence.Transform
{
    public class SqlImporter<T> : IImporter<T> where T : new()
    {
        private readonly string _connectionString;
        private readonly string _commandText;

        public SqlImporter(string connectionString, string commandText)
        {
            _connectionString = connectionString;
            _commandText = commandText;
        }

        public async IAsyncEnumerable<T> Import()
        {
            using (var connection = GetConnection())
            using (var command = GetCommand(connection, _commandText, CommandType.Text))
            using (var reader = command.ExecuteReader())
            {
                while (await reader.ReadAsync())
                {
                    var result = new T();

                    for (var inc = 0; inc < reader.FieldCount; inc++)
                    {
                        var type = result.GetType();
                        var name = reader.GetName(inc);
                        if (name != null)
                        {
                            var prop = type.GetProperty(name);
                            prop.SetValue(result, Convert.ChangeType(reader.GetValue(inc), prop.PropertyType), null);
                        }
                    }

                    yield return result;
                }
            }
        }

#pragma warning disable CA2100 // Review SQL queries for security vulnerabilities
        protected DbCommand GetCommand(DbConnection connection, string commandText, CommandType commandType) => new SqlCommand(commandText, connection as SqlConnection)
        {
            CommandType = commandType
        };
#pragma warning restore CA2100 // Review SQL queries for security vulnerabilities

        private SqlConnection GetConnection()
        {
            var connection = new SqlConnection(_connectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        private static Dictionary<int, string> GetColumnsForDynamicQuery(DataTable schema)
        {
            var rowList = schema?.Rows.Cast<DataRow>().ToList();
            var dynamicColumnDic = new Dictionary<int, string>();

            if (rowList != null)
            {
                foreach (var dr in rowList)
                {
                    var ordinalAsInt = int.Parse(dr["ColumnOrdinal"].ToString());
                    var columnName = dr["ColumnName"]?.ToString().Replace(" ", string.Empty);

                    if (columnName == null)
                    {
                        throw new Exception("There was a probelm retrieving column.");
                    }

                    if (dynamicColumnDic.ContainsValue(columnName))
                    {
                        throw new Exception($"Duplicate column name detected: {columnName}");
                    }

                    dynamicColumnDic.Add(ordinalAsInt, columnName);
                }
            }

            return dynamicColumnDic;
        }
    }
}
