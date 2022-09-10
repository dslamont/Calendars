// See https://aka.ms/new-console-template for more information
using OpenUniversity;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, OU Feed!");


OUConvertor convertor = new OUConvertor();

string m248FileName = "M248.json";
OUFeed m248Feed = CreateM248Feed();
using FileStream m248Stream = File.Create(m248FileName);
await JsonSerializer.SerializeAsync(m248Stream, m248Feed);
await m248Stream.DisposeAsync();

//Create the ics calendar file
string m248Calendar = convertor.CreateCalendar(m248Feed);

//Save the ics file
await File.WriteAllTextAsync("M248.ics", m248Calendar);



string fileName = "M269.json";
OUFeed m269Feed = CreateM269Feed();
using FileStream createStream = File.Create(fileName);
await JsonSerializer.SerializeAsync(createStream, m269Feed);
await createStream.DisposeAsync();

//Create the ics calendar file
string calendar = convertor.CreateCalendar(m269Feed);

//Save the ics file
await File.WriteAllTextAsync("M269.ics", calendar);


OUFeed CreateM248Feed()
{
    OUFeed feed = new OUFeed();
    OUModule moduleDetails = new OUModule();
    moduleDetails.Code = "M248";
    moduleDetails.Title = "Analysing Data";
    feed.Module = moduleDetails;

    OUEvent ouEvent = new OUEvent();
    ouEvent.Id = "20220906";
    ouEvent.StateDate = DateTime.Parse("2022-09-06T00:00:00");
    ouEvent.Title = "Website Opens";
    ouEvent.OrganizerName = "Don Lamont";
    ouEvent.OrganizerEmail = "don.lamont@e-pict.net";
    ouEvent.UpdateCount = 1;

    feed.Events.Add(ouEvent);

    OUEvent ouEvent2 = new OUEvent();
    ouEvent2.Id = "20220910";
    ouEvent2.StateDate = DateTime.Parse("2022-09-10T00:00:00");
    ouEvent2.Title = "Study Materials Arrive";
    ouEvent2.OrganizerName = "Don Lamont";
    ouEvent2.OrganizerEmail = "don.lamont@e-pict.net";
    ouEvent2.UpdateCount = 1;

    feed.Events.Add(ouEvent2);

    return feed;
}

OUFeed CreateM269Feed()
{
    OUFeed feed = new OUFeed();
    OUModule moduleDetails = new OUModule();
    moduleDetails.Code = "M269";
    moduleDetails.Title = "Algorithms, data structures and computability";
    feed.Module = moduleDetails;

    OUEvent ouEvent = new OUEvent();
    ouEvent.Id = "20220906";
    ouEvent.StateDate = DateTime.Parse("2022-09-06T00:00:00");
    ouEvent.Title = "Website Opens";
    ouEvent.OrganizerName = "Don Lamont";
    ouEvent.OrganizerEmail = "don.lamont@e-pict.net";
    ouEvent.UpdateCount = 1;

    feed.Events.Add(ouEvent);

    return feed;
}