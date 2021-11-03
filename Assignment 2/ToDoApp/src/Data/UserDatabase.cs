using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppData
{
    public class UserDatabase
    {
        private readonly Database database;

        public UserDatabase(Database _database)
        {
            database = _database;
        }

        public bool CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Username], [Password], [IsAdmin], [CreatedAt]," +
                        " [LastEdited], [CreatorId], [ModifierId])" +
                        "VALUES (@FirstName, @LastName, @Username, @Password, @IsAdmin," +
                        "@CreatedAt, @LastEdited, @CreatorId, @ModifierId)";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@FirstName", SqlDbType.NVarChar, user.FirstName);
                        database.AddParameter(command, "@LastName", SqlDbType.NVarChar, user.LastName);
                        database.AddParameter(command, "@Username", SqlDbType.NVarChar, user.Username);
                        database.AddParameter(command, "@Password", SqlDbType.NVarChar, user.Password);
                        database.AddParameter(command, "@IsAdmin", SqlDbType.Bit, user.IsAdmin);
                        database.AddParameter(command, "@CreatedAt", SqlDbType.DateTime, user.CreatedAt);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, user.LastEdited);
                        database.AddParameter(command, "@CreatorId", SqlDbType.Int, user.CreatorId);
                        database.AddParameter(command, "@ModifierId", SqlDbType.Int, user.ModifierId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool DeleteUser(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlString = "DELETE FROM Users WHERE Username = @Username";

                    using (SqlCommand command = database.CreateCommand(connection, sqlString))
                    {
                        database.AddParameter(command, "@Username", SqlDbType.VarChar, username);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public List<User> GetUsers()
        {
            List<User> users = new List<User>();

            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = database.CreateCommand(connection, "SELECT * FROM [dbo].[Users]"))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    User user = new User();
                                    user.Id = reader.GetInt32(0);
                                    user.FirstName = reader.GetString(1);
                                    user.LastName = reader.GetString(2);
                                    user.Username = reader.GetString(3);
                                    user.Password = reader.GetString(4);
                                    user.IsAdmin = reader.GetBoolean(5);
                                    user.CreatedAt = reader.GetDateTime(6);
                                    user.LastEdited = reader.GetDateTime(7);
                                    user.CreatorId = reader.GetInt32(8);
                                    user.ModifierId = reader.GetInt32(9);

                                    users.Add(user);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return users;
        }

        public User GetUser(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                User user = new User();
                                user.Id = reader.GetInt32(0);
                                user.FirstName = reader.GetString(1);
                                user.LastName = reader.GetString(2);
                                user.Username = reader.GetString(3);
                                user.Password = reader.GetString(4);
                                user.IsAdmin = reader.GetBoolean(5);
                                user.CreatedAt = reader.GetDateTime(6);
                                user.LastEdited = reader.GetDateTime(7);
                                user.CreatorId = reader.GetInt32(8);
                                user.ModifierId = reader.GetInt32(9);

                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public User GetUser(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE [Username] = @Username";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Username", SqlDbType.VarChar, username);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                User user = new User();
                                user.Id = reader.GetInt32(0);
                                user.FirstName = reader.GetString(1);
                                user.LastName = reader.GetString(2);
                                user.Username = reader.GetString(3);
                                user.Password = reader.GetString(4);
                                user.IsAdmin = reader.GetBoolean(5);
                                user.CreatedAt = reader.GetDateTime(6);
                                user.LastEdited = reader.GetDateTime(7);
                                user.CreatorId = reader.GetInt32(8);
                                user.ModifierId = reader.GetInt32(9);

                                return user;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public bool EditUser(string username, string newUsername, string newPassword, string newFirstName, string newLastName, DateTime dateTime)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Users] SET FirstName = @FirstName,  LastName = @LastName," +
                        "Username = @Username, Password = @Password, LastEdited = @LastEdited WHERE Username = @toEdit";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@toEdit", SqlDbType.VarChar, username);
                        database.AddParameter(command, "@Username", SqlDbType.VarChar, newUsername);
                        database.AddParameter(command, "@FirstName", SqlDbType.VarChar, newFirstName);
                        database.AddParameter(command, "@LastName", SqlDbType.VarChar, newLastName);
                        database.AddParameter(command, "@Password", SqlDbType.VarChar, newPassword);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, dateTime);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }
    }
}
