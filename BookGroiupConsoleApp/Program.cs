// See https://aka.ms/new-console-template for more information
using BookGroup;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

Console.WriteLine("Creating Book Group Calendars");

string fileName = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\book_group.json";
using FileStream openStream = File.OpenRead(fileName);
Schedule? bookGroupSchedule = await JsonSerializer.DeserializeAsync<Schedule>(openStream);
if (bookGroupSchedule is not null)
{
    BookGroupConvertor convertor = new BookGroupConvertor();

    //Create a book group website calendar file
    string webCalFilePath = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\book_group_web.ics";
    string websiteCal = convertor.CreateCalendar(bookGroupSchedule, BookGroupCalTypeEnum.WEBSITE);
    await File.WriteAllTextAsync(webCalFilePath, websiteCal);

    //Create a website calendar file
    string calFilePath = "C:\\Users\\donla\\Dropbox\\Projects\\Development\\Calendars\\book_group.ics";
    string cal = convertor.CreateCalendar(bookGroupSchedule, BookGroupCalTypeEnum.WEBSITE);
    await File.WriteAllTextAsync(calFilePath, cal);
}
