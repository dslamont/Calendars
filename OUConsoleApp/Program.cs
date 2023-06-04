// See https://aka.ms/new-console-template for more information
using OpenUniversity;
using System.Text.Json;

Console.WriteLine("Creating OU Feeds!");

OUConvertor convertor = new OUConvertor();

//Create a M248 Calendar
string m248FileName = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\M248.json";
using FileStream openStream = File.OpenRead(m248FileName);
OUFeed? m248Schedule = await JsonSerializer.DeserializeAsync<OUFeed>(openStream);
if (m248Schedule is not null)
{
    //Create a calendar feed
    string m248CalFilePath = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\M248.ics";
    string m248calendar = convertor.CreateCalendar(m248Schedule);
    await File.WriteAllTextAsync(m248CalFilePath, m248calendar);
}

//Create a M269 Calendar
string m269FileName = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\M269.json";
using FileStream m269Stream = File.OpenRead(m269FileName);
OUFeed? m269Schedule = await JsonSerializer.DeserializeAsync<OUFeed>(m269Stream);
if (m269Schedule is not null)
{
    //Create a calendar feed
    string m269CalFilePath = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\M269.ics";
    string m269calendar = convertor.CreateCalendar(m269Schedule);
    await File.WriteAllTextAsync(m269CalFilePath, m269calendar);
}
