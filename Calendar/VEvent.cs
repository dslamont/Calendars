using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Calendar
{
    public class VEvent
    {
        public string Uid { get; set; }
        public string DateTimeStamp { get; set; }
        public string Organiser { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Sequence { get; set; }

        public string Status { get; set; }
        public string Transparency { get; set; }

        public string Categories { get; set; }
        public string Class { get; set; }

        public void OutputText(TextWriter writer)
        {
            if (writer != null)
            {
                writer.WriteLine("BEGIN:VEVENT");

                writer.WriteLine(Uid);
                writer.WriteLine(DateTimeStamp);
                writer.WriteLine(Organiser);
                writer.WriteLine(StartTime);
                writer.WriteLine(EndTime);
                writer.WriteLine(Summary);
                writer.WriteLine(Description);
                writer.WriteLine(Status);
                writer.WriteLine(Sequence);
                writer.WriteLine(Transparency);
                writer.WriteLine(Categories);
                writer.WriteLine(Class);

                writer.WriteLine("END:VEVENT");

            }
        }
    }
}
