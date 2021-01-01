using System.IO;

namespace Calendar
{
    public class VTimeZone
    {
        public void OutputText(TextWriter writer)
        {
            if (writer != null)
            {
                writer.WriteLine("BEGIN:VTIMEZONE");

                writer.WriteLine("TZID:Europe/London");
                writer.WriteLine("TZURL:http://tzurl.org/zoneinfo-outlook/Europe/London");
                writer.WriteLine("X-LIC-LOCATION:Europe/London");

                OutputDayLightText(writer);
                OutputStandardText(writer);

                writer.WriteLine("END:VTIMEZONE");
            }
        }

        protected void OutputDayLightText(TextWriter writer)
        {
            if (writer != null)
            {
                writer.WriteLine("BEGIN:DAYLIGHT");

                writer.WriteLine("TZOFFSETFROM:+0000");
                writer.WriteLine("TZOFFSETTO:+0100");
                writer.WriteLine("TZNAME:BST");
                writer.WriteLine("DTSTART:19700329T010000");
                writer.WriteLine("RRULE:FREQ=YEARLY;BYMONTH=3;BYDAY=-1SU");
          
                writer.WriteLine("END:DAYLIGHT");
            }
        }

        protected void OutputStandardText(TextWriter writer)
        {
            if (writer != null)
            {
                writer.WriteLine("BEGIN:STANDARD");

                writer.WriteLine("TZOFFSETFROM:+0100");
                writer.WriteLine("TZOFFSETTO:+0100");
                writer.WriteLine("TZNAME:GMT");
                writer.WriteLine("DTSTART:19701025T020000");
                writer.WriteLine("RRULE:FREQ=YEARLY;BYMONTH=10;BYDAY=-1SU");

                writer.WriteLine("END:STANDARD");
            }
        }
    }
}
