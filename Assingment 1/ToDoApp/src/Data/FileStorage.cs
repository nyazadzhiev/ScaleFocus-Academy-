using System;
using System.IO;
using System.Text.Json;

namespace ToDoAppData
{
    public class FileStorage
    {
        public class FileDatabase
        {
            public void Write<T>(string fileName, T data)
            {
                string serializedData = JsonSerializer.Serialize(data);

                File.WriteAllText(fileName, serializedData);

            }

            public T Read<T>(string fileName) where T : class
            {
                if (File.Exists(fileName))
                {
                    string serializedData = File.ReadAllText(fileName);
                    return JsonSerializer.Deserialize<T>(serializedData);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
