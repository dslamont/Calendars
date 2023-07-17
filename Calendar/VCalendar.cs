using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calendar
{
    public class VCalendar
    {
        private List<VEvent> _events;

        public VTimeZone TimeZone { get; set; }

        public List<VEvent> Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new List<VEvent>();
                }

                return _events;
            }

            set
            {
                _events = value;
            }
        }


        public string CreateCalendarText(string calId)
        {
            StringBuilder sb = new StringBuilder();

            StringWriter writer = new StringWriter(sb);

            writer.WriteLine("BEGIN:VCALENDAR");
            writer.WriteLine("VERSION:2.0");
            writer.WriteLine("PRODID:-//e-pict.net/calendars//NONSGML v1.0//EN");
            writer.WriteLine($"X-WR-RELCALID:{calId}");

            if (TimeZone != null)
            {
                TimeZone.OutputText(writer);
            }

            if(Events!=null)
            {
                foreach(VEvent vEvent in Events)
                {
                    vEvent.OutputText(writer);
                }
            }

            writer.WriteLine("END:VCALENDAR");
            writer.Flush();
            writer.Close();

            return sb.ToString();
        }

    }
}