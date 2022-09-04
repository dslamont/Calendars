// See https://aka.ms/new-console-template for more information
using OpenUniversity;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Hello, OU Feed!");

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

string fileName = "M248.json";
using FileStream createStream = File.Create(fileName);
await JsonSerializer.SerializeAsync(createStream, feed);
await createStream.DisposeAsync();

//Create the ics calendar file
OUConvertor convertor = new OUConvertor();
string calendar = convertor.CreateCalendar(feed);

//Save the ics file
await File.WriteAllTextAsync("M248.ics", calendar);