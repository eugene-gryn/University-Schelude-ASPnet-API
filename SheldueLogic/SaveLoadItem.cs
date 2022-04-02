using System.IO;
using Newtonsoft.Json;

namespace SheldueLogic
{
    public interface ILoader
    {
        Sheldue LoadObj(string file);
        void SaverObj(string file, object obj);
    }

    public class JsonSaveLoader : ILoader
    {
        public Sheldue LoadObj(string filename)
        {
            using (var writer = new StreamReader(filename))
            {
                var str = writer.ReadToEnd();
                return JsonConvert.DeserializeObject<Sheldue>(str);
            }
        }

        public void SaverObj(string filename, object obj)
        {
            using (var writer = new StreamWriter(filename))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}