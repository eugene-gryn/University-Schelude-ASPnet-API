
using ConsoleTestApp;
using System.Text.Json;

string filename = "jsonFile.json";
var User = JsonGenerator.UserGenerate();

string jsonstr = JsonSerializer.Serialize(User);
File.WriteAllText(filename, jsonstr);
Console.WriteLine(File.ReadAllText(filename));

