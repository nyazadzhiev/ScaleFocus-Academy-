using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppData
{
    public class TaskRepository
    {
        private readonly Database database;

        public TaskRepository(Database _database)
        {
            database = _database;
        }

        public bool CreateTask(Task task)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[Tasks] ([Title], [Description], [IsCompleted], [CreatorId], [ModifierId]," +
                        "[LastEdited], [CreatedAt], [ListId])" +
                        "VALUES (@Title, @Description, @IsCompleted, @CreatorId, @ModifierId, @LastEdited, @CreatedAt, @ListId)";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, task.Title);
                        database.AddParameter(command, "@Description", SqlDbType.VarChar, task.Description);
                        database.AddParameter(command, "@IsCompleted", SqlDbType.Bit, task.IsComplete);
                        database.AddParameter(command, "@CreatorId", SqlDbType.Int, task.CreatorId);
                        database.AddParameter(command, "@ModifierId", SqlDbType.Int, task.ModifierId);
                        database.AddParameter(command, "@ListId", SqlDbType.Int, task.ListId);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, task.LastEdited);
                        database.AddParameter(command, "@CreatedAt", SqlDbType.DateTime, task.CreatedAt);

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

        public Task GetTask(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Tasks] WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Task task = new Task();
                                task.Id = reader.GetInt32(0);
                                task.Title = reader.GetString(1);
                                task.Description = reader.GetString(2);
                                task.IsComplete = reader.GetBoolean(3);
                                task.CreatedAt = reader.GetDateTime(4);
                                task.LastEdited = reader.GetDateTime(5);
                                task.CreatorId = reader.GetInt32(6);
                                task.ModifierId = reader.GetInt32(7);
                                task.ListId = reader.GetInt32(8);

                                return task;
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

        public Task GetTask(string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Tasks] WHERE [Title] = @Title";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, title);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                reader.Read();
                                Task task = new Task();
                                task.Id = reader.GetInt32(0);
                                task.Title = reader.GetString(1);
                                task.Description = reader.GetString(2);
                                task.IsComplete = reader.GetBoolean(3);
                                task.CreatedAt = reader.GetDateTime(4);
                                task.LastEdited = reader.GetDateTime(5);
                                task.CreatorId = reader.GetInt32(6);
                                task.ModifierId = reader.GetInt32(7);
                                task.ListId = reader.GetInt32(8);

                                return task;
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

        public List<Task> GetTasks(int listId)
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM Tasks WHERE [ListId] = @ListId";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {

                        database.AddParameter(command, "@ListId", SqlDbType.Int, listId);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Task task = new Task();
                                    task.Id = reader.GetInt32(0);
                                    task.Title = reader.GetString(1);
                                    task.Description = reader.GetString(2);
                                    task.IsComplete = reader.GetBoolean(3);
                                    task.CreatedAt = reader.GetDateTime(4);
                                    task.LastEdited = reader.GetDateTime(5);
                                    task.CreatorId = reader.GetInt32(6);
                                    task.ModifierId = reader.GetInt32(7);
                                    task.ListId = reader.GetInt32(8);

                                    tasks.Add(task);
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

            return tasks;
        }

        public bool DeleteTask(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "DELETE [dbo].[Tasks] WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);

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

        public bool EditTask(int id, string newTitle, string newDescription, bool newIsComlpeted)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Tasks] SET [Title] = @Title, [Description] = @Description, [IsCompleted] = @IsCompleted, [LastEdited] = @LastEdited " +
                        "WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, newTitle);
                        database.AddParameter(command, "@Description", SqlDbType.VarChar, newDescription);
                        database.AddParameter(command, "@IsCompleted", SqlDbType.Bit, newIsComlpeted);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, DateTime.Now);

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

        public bool CompleteTask(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Tasks] SET [IsCompleted] = @IsCompleted, [LastEdited] = @LastEdited " +
                        "WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);
                        database.AddParameter(command, "@IsCompleted", SqlDbType.Bit, true);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, DateTime.Now);

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

        public bool AssignTask(int userId, int taskId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO UsersAssignedTasks ([UserId], [TaskId]) VALUES (@UserId, TaskId)";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        database.AddParameter(command, "@TaskId", SqlDbType.Int, taskId);

                        object result = command.ExecuteNonQuery();

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

        public List<Task> GetAssignedTasks(int userId)
        {
            List<Task> tasks = new List<Task>();

            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM Tasks INNER JOIN UsersAssignedTasks ON [UserId] = [Id] AND [UserID] = @UserId";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@UserId", SqlDbType.Int, userId);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Task task = new Task();
                                    task.Id = reader.GetInt32(0);
                                    task.Title = reader.GetString(1);
                                    task.Description = reader.GetString(2);
                                    task.IsComplete = reader.GetBoolean(3);
                                    task.CreatedAt = reader.GetDateTime(4);
                                    task.LastEdited = reader.GetDateTime(5);
                                    task.CreatorId = reader.GetInt32(6);
                                    task.ModifierId = reader.GetInt32(7);
                                    task.ListId = reader.GetInt32(8);

                                    tasks.Add(task);
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

            return tasks;
        }
    }
}
