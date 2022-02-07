

using Newtonsoft.Json;
using System.IO;

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
            using (StreamReader writer = new StreamReader(filename))
            {
                string str = writer.ReadToEnd();
                return JsonConvert.DeserializeObject<Sheldue>(str);
            }
        }

        public void SaverObj(string filename, object obj)
        {
            using (StreamWriter writer = new StreamWriter(filename))
            {
                writer.Write(JsonConvert.SerializeObject(obj));
            }
        }
    }
}
