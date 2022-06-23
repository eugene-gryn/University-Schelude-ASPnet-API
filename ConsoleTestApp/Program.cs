
using ConsoleTestApp;
using System.Text.Json;

ScheduleDataSetGenerator generator = new ScheduleDataSetGenerator();

var users = generator.RUsersAdd(30);

users.ForEach(user => Console.WriteLine(user.Name));
