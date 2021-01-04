﻿using Calendar;
using System;
using System.Collections.Generic;

namespace BookGroup
{
    public class BookGroupConvertor
    {
        public string CreateCalendar(Schedule schedule)
        {
            string calendarText = string.Empty;

            if (schedule != null)
            {
                VCalendar calendar = new VCalendar();
                calendar.TimeZone = new VTimeZone();
                calendar.Events = CreateEvents(schedule.Meetings);
                calendarText = calendar.CreateCalendarText();
            }

            return calendarText;
        }

        protected List<VEvent> CreateEvents(List<Meeting> meetings)
        {
            List<VEvent> events = new List<VEvent>();

            if (meetings != null)
            {
                foreach (Meeting meeting in meetings)
                {
                    VEvent vEvent = CreateEvent(meeting);
                    if (vEvent != null)
                    {
                        events.Add(vEvent);
                    }
                }
            }

            return events;
        }

        protected VEvent CreateEvent(Meeting meeting)
        {
            VEvent vEvent = null;

            if (meeting != null)
            {
                vEvent = new VEvent();

                vEvent.Uid = CreateUID(meeting.StateDate);
                vEvent.DateTimeStamp = $"DTSTAMP:{meeting.StateDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")}";
                vEvent.Organiser = $"ORGANIZER;CN={meeting.OrganizerName}:MAILTO:{meeting.OrganizerEmail}";

                vEvent.StartTime = $"DTSTART;TZID=Europe/London:{meeting.StateDate.ToUniversalTime().ToString("yyyyMMddTHHmmss")}";
                vEvent.EndTime = $"DTEND;TZID=Europe/London:{meeting.StateDate.AddMinutes(90).ToUniversalTime().ToString("yyyyMMddTHHmmss")}";

                vEvent.Summary = $"SUMMARY:{meeting.Title} - {meeting.Author}";
                string desc = CreateDescription(meeting);
                vEvent.Description = $"DESCRIPTION:{desc}";
                vEvent.Status = "STATUS:CONFIRMED";
                vEvent.Sequence = $"SEQUENCE:{meeting.UpdateCount}";
                vEvent.Transparency = "TRANSP:TRANSPARENT";
                vEvent.Categories = "CATEGORIES:Book Group";
                vEvent.Class = "CLASS:PUBLIC";

            }

            return vEvent;
        }

        protected string CreateUID(DateTime dateTime)
        {
            string uid = String.Empty;

            if (dateTime != null)
            {
                uid = $"UID:ferryhill_{dateTime.Year}{dateTime.Month}{dateTime.Day}_@e-pict.net";
            }

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

    }
}
