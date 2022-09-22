using Calendar;
using System;
using System.Collections.Generic;

namespace Bins
{
    public class BinDays
    {
        public static string CreateBlackBinDays()
        {
            string calendarText = String.Empty;

            List<VEvent> events = new List<VEvent>();

            DateTime currentDate = new DateTime(2021, 06, 4, 0, 0, 0);
            DateTime endDate = new DateTime(2022, 05, 27, 0, 0, 0);

            int loopIndex = 0;
            while (currentDate < endDate)
            {
                VEvent vEvent = CreateEvent(currentDate, true);
                events.Add(vEvent);

                currentDate = currentDate.AddDays(14);
                loopIndex++;
            }

            VCalendar calendar = new VCalendar();
            calendar.TimeZone = new VTimeZone();
            calendar.Events = events;
            calendarText = calendar.CreateCalendarText(String.Empty);
            
            return calendarText;
        }

        public static string CreateRecyclingBinDays()
        {
            string calendarText = String.Empty;

            List<VEvent> events = new List<VEvent>();

            DateTime currentDate = new DateTime(2021, 05, 28, 0, 0, 0);
            DateTime endDate = new DateTime(2022, 06, 03, 0, 0, 0);

            int loopIndex = 0;
            while (currentDate < endDate)
            {
                VEvent vEvent = CreateEvent(currentDate, false);
                events.Add(vEvent);

                currentDate = currentDate.AddDays(14);
                loopIndex++;
            }

            VCalendar calendar = new VCalendar();
            calendar.TimeZone = new VTimeZone();
            calendar.Events = events;
            calendarText = calendar.CreateCalendarText(String.Empty);

            return calendarText;
        }

        protected static VEvent CreateEvent(DateTime date, bool blackBins)
        {
            VEvent vEvent = null;

            vEvent = new VEvent();

            vEvent.Uid = CreateUID(date);
            vEvent.DateTimeStamp = $"DTSTAMP:{date.ToString("yyyyMMddTHHmmssZ")}";
            vEvent.Organiser = "ORGANIZER;CN=Don Lamont:MAILTO:don.lamont@e-pict.net";

            vEvent.StartTime = $"DTSTART;TZID=Europe/London:{date.ToString("yyyyMMdd")}";
            vEvent.EndTime = $"DTEND;TZID=Europe/London:{date.AddDays(1).ToString("yyyyMMdd")}";

            if (blackBins)
            {
                vEvent.Summary = "SUMMARY:Black Bins";
                vEvent.Description = "DESCRIPTION:Black Bins";
            }
            else
            {
                vEvent.Summary = "SUMMARY:Recycling Bins";
                vEvent.Description = "DESCRIPTION:Recycling Bins";
            }
            vEvent.Status = "STATUS:CONFIRMED";
            vEvent.Sequence = "SEQUENCE:1";
            vEvent.Transparency = "TRANSP:TRANSPARENT";
            vEvent.Categories = "CATEGORIES:Refuse,Recycling";
            vEvent.Class = "CLASS:PUBLIC";

            return vEvent;
        }

        protected static string CreateUID(DateTime dateTime)
        {
            string uid = String.Empty;

            if (dateTime != null)
            {
                uid = $"UID:bins_{dateTime.Year}{dateTime.Month}{dateTime.Day}_@e-pict.net";
            }

            return uid;
        }
    }
}
