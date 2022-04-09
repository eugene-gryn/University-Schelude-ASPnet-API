using System.IO;
using Newtonsoft.Json;

namespace ScheduleLogic.StateHolding
{
    public class JsonSaveLoader : ILoader
    {
        public ScheduleEngine LoadObj(string filename)
        {
            using (var writer = new StreamReader(filename))
            {
                var str = writer.ReadToEnd();
                return JsonConvert.DeserializeObject<ScheduleEngine>(str);
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