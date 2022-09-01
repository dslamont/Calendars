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
                calendar.Events = CreateEvents(feed);
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
                        VEvent vEvent = CreateEvent(ouEvent);
                        if (vEvent != null)
                        {
                            events.Add(vEvent);
                        }
                    }
                }
            }

            return events;
        }

        protected VEvent CreateEvent(OUEvent ouEvent)
        {
            VEvent vEvent = null;

            if (ouEvent != null)
            {
                vEvent = new VEvent();

                vEvent.Uid = CreateUID(ouEvent.Id);
                vEvent.DateTimeStamp = $"DTSTAMP:{CreateDateTimeString(ouEvent.StateDate)}";
                vEvent.Organiser = $"ORGANIZER;CN={ouEvent.OrganizerName}:MAILTO:{ouEvent.OrganizerEmail}";


                vEvent.StartTime = $"DTSTART;TZID=Europe/London:{CreateDateTimeString(ouEvent.StateDate)}";
                vEvent.EndTime = $"DTEND;TZID=Europe/London:{CreateDateTimeString(ouEvent.StateDate.AddMinutes(90))}";

                vEvent.Summary = $"SUMMARY:{ouEvent.Title}";
                string desc = CreateDescription(ouEvent);
                vEvent.Description = $"DESCRIPTION:{desc}";


                string htmlDesc = CreateDescriptionHTML(ouEvent);
                vEvent.DescriptionHTML = $"X-ALT-DESC;FMTTYPE=text/html:{htmlDesc}";

                vEvent.Location = $"LOCATION:[TODO: Location Goes Here]";
                vEvent.Status = "STATUS:CONFIRMED";
                vEvent.Sequence = $"SEQUENCE:{ouEvent.UpdateCount}";
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

        protected string CreateDescription(OUEvent ouEvent)
        {
            string desc = String.Empty;

            if (ouEvent != null)
            {
                //desc = $"<a src=\"{meeting.BookUrl}\">{meeting.Title} -  {meeting.Author}</a>";
                desc = $"{ouEvent.Title}";
            }

            return desc;
        }

        protected string CreateDescriptionHTML(OUEvent ouEvent)
        {
            StringBuilder html = new StringBuilder();

            if (ouEvent != null)
            {
                //desc = $"<a src=\"{meeting.BookUrl}\">{meeting.Title} -  {meeting.Author}</a>";
                html.Append("<!DOCTYPE HTML PUBLIC \" -//W3C//DTD HTML 3.2//EN\">");
                html.Append("<html>");
                html.Append("<body>");
                html.Append("<p>");

                html.Append("[TODO: Link Goes Here]");

                html.Append("</p>");

                html.Append("</body>");
                html.Append("</html>");
            }

            return html.ToString();
        }

        protected string CreateDateTimeString(DateTime dateTime)
        {
            string dateTimeString = String.Empty;

            //switch (calType)
            //{
            //    case BookGroupCalTypeEnum.WEBSITE:
            //        dateTimeString = dateTime.ToString("yyyyMMddTHHmmssZ");
            //        break;
            //    default:
            //        dateTimeString = dateTime.ToString("yyyyMMddTHHmmssZ");
            //        break;
            //}

            dateTimeString = dateTime.ToString("yyyyMMddTHHmmssZ");

            return dateTimeString;

        }
    }
}
