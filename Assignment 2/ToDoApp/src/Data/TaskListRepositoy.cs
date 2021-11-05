using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using ToDoAppEntities;

namespace ToDoAppData
{
    public class TaskListRepositoy
    {
        private readonly Database database;

        public TaskListRepositoy(Database _database)
        {
            database = _database;
        }

        public bool CreateTaskList(TaskList list)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[TaskLists] ([Title], [CreatorId], [ModifierId], [LastEdited], [CreatedAt])" +
                        "VALUES (@Title, @CreatorId, @ModifierId, @LastEdited, @CreatedAt)";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, list.Title);
                        database.AddParameter(command, "@CreatorId", SqlDbType.Int, list.CreatorId);
                        database.AddParameter(command, "@ModifierId", SqlDbType.Int, list.ModifierId);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, list.LastEdited);
                        database.AddParameter(command, "@CreatedAt", SqlDbType.DateTime, list.CreatedAt);

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

        public List<TaskList> GetTaskLists(int id)
        {
            List<TaskList> lists = new List<TaskList>();

            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = database.CreateCommand(connection, "SELECT * FROM [dbo].[Tasklists] WHERE [CreatorId] = @Id"))
                    {
                        database.AddParameter(command, "@Id", SqlDbType.Int, id);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TaskList taskList = new TaskList();
                                    taskList.Id = reader.GetInt32(0);
                                    taskList.Title = reader.GetString(1);
                                    taskList.CreatedAt = reader.GetDateTime(2);
                                    taskList.LastEdited = reader.GetDateTime(3);
                                    taskList.CreatorId = reader.GetInt32(4);
                                    taskList.ModifierId = reader.GetInt32(5);

                                    lists.Add(taskList);
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

            return lists;
        }

        public TaskList GetTaskList(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = database.CreateCommand(connection, "SELECT * FROM [dbo].[TaskLists] WHERE [Id] = @TaskListId"))
                    {
                        database.AddParameter(command, "@TaskListId", SqlDbType.Int, id);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TaskList taskList = new TaskList();
                                    taskList.Id = reader.GetInt32(0);
                                    taskList.Title = reader.GetString(1);
                                    taskList.CreatedAt = reader.GetDateTime(2);
                                    taskList.LastEdited = reader.GetDateTime(3);
                                    taskList.CreatorId = reader.GetInt32(4);
                                    taskList.ModifierId = reader.GetInt32(5);

                                    return taskList;
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

            return null;
        }

        public TaskList GetTaskList(string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = database.CreateCommand(connection, "SELECT * FROM [dbo].[TaskLists] WHERE [Title] = @Title"))
                    {
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, title);

                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TaskList taskList = new TaskList();
                                    taskList.Id = reader.GetInt32(0);
                                    taskList.Title = reader.GetString(1);
                                    taskList.CreatedAt = reader.GetDateTime(2);
                                    taskList.LastEdited = reader.GetDateTime(3);
                                    taskList.CreatorId = reader.GetInt32(4);
                                    taskList.ModifierId = reader.GetInt32(5);

                                    return taskList;
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

            return null;
        }

        public bool EditTaskList(int id, string newTitle)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[TaskLists] SET [Title] = @Title, [LastEdited] = @LastEdited WHERE [Id] = @Id";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        DateTime now = DateTime.Now;

                        database.AddParameter(command, "@Id", SqlDbType.Int, id);
                        database.AddParameter(command, "@Title", SqlDbType.VarChar, newTitle);
                        database.AddParameter(command, "@LastEdited", SqlDbType.DateTime, now);

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

        public bool DeleteTaskList(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "DELETE FROM [dbo].[TaskLists] WHERE [Id] = @Id";

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

        public bool ShareTaskList(int userId, int listId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[UsersSharedLists] ([UserId], [ListId])" +
                        "VALUES (@UserId, ListId)";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        database.AddParameter(command, @"UserId", SqlDbType.Int, userId);
                        database.AddParameter(command, @"UsListIderId", SqlDbType.Int, listId);

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

        public List<TaskList> GetSharedLists()
        {
            List<TaskList> lists = new List<TaskList>();

            try
            {
                using (SqlConnection connection = new SqlConnection(database._sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM TaskLists INNER JOIN UsersSharedLists ON [Id] = [ListId]";

                    using (SqlCommand command = database.CreateCommand(connection, sqlQuery))
                    {
                        using (SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    TaskList taskList = new TaskList();
                                    taskList.Id = reader.GetInt32(0);
                                    taskList.Title = reader.GetString(1);
                                    taskList.CreatedAt = reader.GetDateTime(2);
                                    taskList.LastEdited = reader.GetDateTime(3);
                                    taskList.CreatorId = reader.GetInt32(4);
                                    taskList.ModifierId = reader.GetInt32(5);

                                    lists.Add(taskList);
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

            return lists;
        }
    }
}
