using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data.Common;

namespace ToDoAppData
{
    public class DatabaseInitilizer
    {
        public static void InitilizeDatabase(string connectionstring)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    connection.Open();
                    string script = File.ReadAllText("Scripts\\CreateDatabase.sql");
                    DbCommand command = connection.CreateCommand();
                    command.CommandText = script;
                    command.ExecuteNonQuery();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}

