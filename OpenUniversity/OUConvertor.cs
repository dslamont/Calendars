using Calendar;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenUniversity
{
    public class OUConvertor
    {
        public string CreateCalendar(OUFeed feed)
        {
            string calendarText = string.Empty;

            if (feed != null)
            {
                VCalendar calendar = new VCalendar();
                calendar.TimeZone = new VTimeZone();
                calendar.Events = CreateEvents(feed.Meetings, calType);
                calendarText = calendar.CreateCalendarText();
            }

            return calendarText;
        }

        protected List<VEvent> CreateEvents(OUFeed feed)
        {
            List<VEvent> events = new List<VEvent>();

            if (feed != null)
            {
                if (feed.Events != null)
                {
                    foreach (OUEvent ouEvent in feed.Events)
                    {
                        VEvent vEvent = CreateEvent(ouEvent, calType);
                        if (vEvent != null)
                        {
                            events.Add(vEvent);
                        }
                    }
                }
            }

            return events;
        }

        protected VEvent CreateEvent(Meeting meeting, BookGroupCalTypeEnum calType)
        {
            VEvent vEvent = null;

            if (meeting != null)
            {
                vEvent = new VEvent();

                vEvent.Uid = CreateUID(meeting.Id);
                vEvent.DateTimeStamp = $"DTSTAMP:{CreateDateTimeString(meeting.StateDate, calType)}";
                vEvent.Organiser = $"ORGANIZER;CN={meeting.OrganizerName}:MAILTO:{meeting.OrganizerEmail}";


                vEvent.StartTime = $"DTSTART;TZID=Europe/London:{CreateDateTimeString(meeting.StateDate, calType)}";
                vEvent.EndTime = $"DTEND;TZID=Europe/London:{CreateDateTimeString(meeting.StateDate.AddMinutes(90), calType)}";

                vEvent.Summary = $"SUMMARY:{meeting.Title} - {meeting.Author}";
                string desc = CreateDescription(meeting);
                vEvent.Description = $"DESCRIPTION:{desc}";


                string htmlDesc = CreateDescriptionHTML(meeting);
                vEvent.DescriptionHTML = $"X-ALT-DESC;FMTTYPE=text/html:{htmlDesc}";

                vEvent.Location = $"LOCATION:{meeting.Location}";
                vEvent.Status = "STATUS:CONFIRMED";
                vEvent.Sequence = $"SEQUENCE:{meeting.UpdateCount}";
                vEvent.Transparency = "TRANSP:TRANSPARENT";
                vEvent.Categories = "CATEGORIES:Book Group";
                vEvent.Class = "CLASS:PUBLIC";

            }

            return vEvent;
        }

        protected string CreateUID(string id)
        {
            string uid = $"UID:ferryhill_{id}_@e-pict.net";

            return uid;
        }

        protected string CreateDescription(Meeting meeting)
        {
            string desc = String.Empty;

            if (meeting != null)
            {
                //desc = $"<a src=\"{meeting.BookUrl}\">{meeting.Title} -  {meeting.Author}</a>";
                desc = $"{meeting.Title} -  {meeting.Author}";
            }

            return desc;
        }

        protected string CreateDescriptionHTML(Meeting meeting)
        {
            StringBuilder html = new StringBuilder();

            if (meeting != null)
            {
                //desc = $"<a src=\"{meeting.BookUrl}\">{meeting.Title} -  {meeting.Author}</a>";
                html.Append("<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 3.2//EN\">");
                html.Append("<html>");
                html.Append("<body>");
                html.Append("<p>");

                html.Append($"<a href=\"{meeting.BookUrl}\">");

                html.Append($"<img src=\"{meeting.CoverImageUrl}\"/>");

                html.Append("</a>");

                html.Append("</p>");

                html.Append("</body>");
                html.Append("</html>");
            }

            return html.ToString();
        }

        protected string CreateDateTimeString(DateTime dateTime, BookGroupCalTypeEnum calType)
        {
            string dateTimeString = String.Empty;

            switch (calType)
            {
                case BookGroupCalTypeEnum.WEBSITE:
                    dateTimeString = dateTime.ToString("yyyyMMddTHHmmssZ");
                    break;
                default:
                    dateTimeString = dateTime.ToString("yyyyMMddTHHmmssZ");
                    break;
            }

            return dateTimeString;

        }
    }
}
