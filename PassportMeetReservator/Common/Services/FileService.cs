using System.IO;

using Newtonsoft.Json;

namespace Common.Services
{
    public class FileService
    {
        public T LoadData<T>(string filename) where T : new()
        {
            if (!File.Exists(filename))
                return new T();

            using (StreamReader str = new StreamReader(filename))
            {
                string json = str.ReadToEnd();

                if (string.IsNullOrEmpty(json))
                    return new T();

                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public void SaveData(string filename, object data)
        {
            using (StreamWriter strw = File.CreateText(filename))
                strw.Write(JsonConvert.SerializeObject(data));
        }
    }
}
