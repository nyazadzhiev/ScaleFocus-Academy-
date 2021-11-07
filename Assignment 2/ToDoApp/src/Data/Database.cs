using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Data.SqlClient;
using System.Data;
using ToDoAppEntities;
using Task = ToDoAppEntities.Task;

namespace ToDoAppData
{
    public class Database
    {
        public string _sqlConnectionString { get; }

        public Database(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        public SqlCommand CreateCommand(SqlConnection connection, string sql)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        public void AddParameter(SqlCommand command, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            command.Parameters.Add(parameterName, parameterType).Value = parameterValue;
        }



        


    }
}
