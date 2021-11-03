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
        private readonly string _sqlConnectionString;

        public Database(string connectionString)
        {
            _sqlConnectionString = connectionString;
        }

        private SqlCommand CreateCommand(SqlConnection connection, string sql)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        private void AddParameter(SqlCommand command, string parameterName, SqlDbType parameterType, object parameterValue)
        {
            command.Parameters.Add(parameterName, parameterType).Value = parameterValue;
        }

        public bool CreateUser(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[Users] ([FirstName], [LastName], [Username], [Password], [IsAdmin], [CreatedAt]," +
                        " [LastEdited], [CreatorId], [ModifierId])" +
                        "VALUES (@FirstName, @LastName, @Username, @Password, @IsAdmin," +
                        "@CreatedAt, @LastEdited, @CreatorId, @ModifierId)";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@FirstName", SqlDbType.NVarChar, user.FirstName);
                        AddParameter(command, "@LastName", SqlDbType.NVarChar, user.LastName);
                        AddParameter(command, "@Username", SqlDbType.NVarChar, user.Username);
                        AddParameter(command, "@Password", SqlDbType.NVarChar, user.Password);
                        AddParameter(command, "@IsAdmin", SqlDbType.Bit, user.IsAdmin);
                        AddParameter(command, "@CreatedAt", SqlDbType.DateTime, user.CreatedAt);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, user.LastEdited);
                        AddParameter(command, "@CreatorId", SqlDbType.Int, user.CreatorId);
                        AddParameter(command, "@ModifierId", SqlDbType.Int, user.ModifierId);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool DeleteUser(string username)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlString = "DELETE FROM Users WHERE Username = @Username";

                    using(SqlCommand command = CreateCommand(connection, sqlString))
                    {
                        AddParameter(command, "@Username", SqlDbType.VarChar, username);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using(SqlCommand command = CreateCommand(connection, "SELECT * FROM [dbo].[Users]"))
                    {
                        using(SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);

                        using(SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public User GetUser(string username)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Users] WHERE [Username] = @Username";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Username", SqlDbType.VarChar, username);

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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Users] SET FirstName = @FirstName,  LastName = @LastName," +
                        "Username = @Username, Password = @Password, LastEdited = @LastEdited WHERE Username = @toEdit";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@toEdit", SqlDbType.VarChar, username);
                        AddParameter(command, "@Username", SqlDbType.VarChar, newUsername);
                        AddParameter(command, "@FirstName", SqlDbType.VarChar, newFirstName);
                        AddParameter(command, "@LastName", SqlDbType.VarChar, newLastName);
                        AddParameter(command, "@Password", SqlDbType.VarChar, newPassword);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, dateTime);

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

        public bool CreateTaskList(TaskList list)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[TaskLists] ([Title], [CreatorId], [ModifierId], [LastEdited], [CreatedAt])" +
                        "VALUES (@Title, @CreatorId, @ModifierId, @LastEdited, @CreatedAt)";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Title", SqlDbType.VarChar, list.Title);
                        AddParameter(command, "@CreatorId", SqlDbType.Int, list.CreatorId);
                        AddParameter(command, "@ModifierId", SqlDbType.Int, list.ModifierId);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, list.LastEdited);
                        AddParameter(command, "@CreatedAt", SqlDbType.DateTime, list.CreatedAt);

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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using(SqlCommand command = CreateCommand(connection, "SELECT * FROM [dbo].[Tasklists] WHERE [CreatorId] = @Id"))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);

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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using(SqlCommand command = CreateCommand(connection, "SELECT * FROM [dbo].[TaskLists] WHERE [Id] = @TaskListId"))
                    {
                        AddParameter(command, "@TaskListId", SqlDbType.Int, id);
                        
                        using(SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    using (SqlCommand command = CreateCommand(connection, "SELECT * FROM [dbo].[TaskLists] WHERE [Title] = @Title"))
                    {
                        AddParameter(command, "@Title", SqlDbType.VarChar, title);

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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[TaskLists] SET [Title] = @Title, [LastEdited] = @LastEdited WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        DateTime now = DateTime.Now;

                        AddParameter(command, "@Id", SqlDbType.Int, id);
                        AddParameter(command, "@Title", SqlDbType.VarChar, newTitle);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, now);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool DeleteTaskList(int id)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "DELETE FROM [dbo].[TaskLists] WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool ShareTaskList(int userId, int listId)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[UsersSharedLists] ([UserId], [ListId])" +
                        "VALUES (@UserId, ListId)";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, @"UserId", SqlDbType.Int, userId);
                        AddParameter(command, @"UsListIderId", SqlDbType.Int, listId);

                        object result = command.ExecuteNonQuery();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM TaskLists INNER JOIN UsersSharedLists ON [Id] = [ListId]";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return lists;
        }

        public bool CreateTask(Task task)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO [dbo].[Tasks] ([Title], [Description], [IsCompleted], [CreatorId], [ModifierId]," +
                        "[LastEdited], [CreatedAt], [ListId])" +
                        "VALUES (@Title, @Description, @IsCompleted, @CreatorId, @ModifierId, @LastEdited, @CreatedAt, @ListId)";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Title", SqlDbType.VarChar, task.Title);
                        AddParameter(command, "@Description", SqlDbType.VarChar, task.Description);
                        AddParameter(command, "@IsCompleted", SqlDbType.Bit, task.IsComplete);
                        AddParameter(command, "@CreatorId", SqlDbType.Int, task.CreatorId);
                        AddParameter(command, "@ModifierId", SqlDbType.Int, task.ModifierId);
                        AddParameter(command, "@ListId", SqlDbType.Int, task.ListId);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, task.LastEdited);
                        AddParameter(command, "@CreatedAt", SqlDbType.DateTime, task.CreatedAt);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public Task GetTask(int id)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Tasks] WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);

                        using(SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection))
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return null;
        }

        public Task GetTask(string title)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM [dbo].[Tasks] WHERE [Title] = @Title";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Title", SqlDbType.VarChar, title);

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
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM Tasks WHERE [ListId] = @ListId";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {

                        AddParameter(command, "@ListId", SqlDbType.Int, listId);

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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return tasks;
        }

        public bool DeleteTask(int id)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "DELETE [dbo].[Tasks] WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool EditTask(int id, string newTitle, string newDescription, bool newIsComlpeted)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Tasks] SET [Title] = @Title, [Description] = @Description, [IsCompleted] = @IsCompleted, [LastEdited] = @LastEdited " +
                        "WHERE [Id] = @Id";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);
                        AddParameter(command, "@Title", SqlDbType.VarChar, newTitle);
                        AddParameter(command, "@Description", SqlDbType.VarChar, newDescription);
                        AddParameter(command, "@IsCompleted", SqlDbType.Bit, newIsComlpeted);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, DateTime.Now);

                        object result = command.ExecuteScalar();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return false;
        }

        public bool CompleteTask(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "UPDATE [dbo].[Tasks] SET [IsCompleted] = @IsCompleted, [LastEdited] = @LastEdited " +
                        "WHERE [Id] = @Id";

                    using (SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@Id", SqlDbType.Int, id);
                        AddParameter(command, "@IsCompleted", SqlDbType.Bit, true);
                        AddParameter(command, "@LastEdited", SqlDbType.DateTime, DateTime.Now);

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
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "INSERT INTO UsersAssignedTasks ([UserId], [TaskId]) VALUES (@UserId, TaskId)";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@UserId", SqlDbType.Int, userId);
                        AddParameter(command, "@TaskId", SqlDbType.Int, taskId);

                        object result = command.ExecuteNonQuery();

                        return result != null;
                    }
                }
            }
            catch(Exception e)
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
                using (SqlConnection connection = new SqlConnection(_sqlConnectionString))
                {
                    connection.Open();

                    string sqlQuery = "SELECT * FROM Tasks INNER JOIN UsersAssignedTasks ON [UserId] = [Id] AND [UserID] = @UserId";

                    using(SqlCommand command = CreateCommand(connection, sqlQuery))
                    {
                        AddParameter(command, "@UserId", SqlDbType.Int, userId);

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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return tasks;
        }
    }
}
