using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BookGroup
{
    public class Convertor
    {
        public string CreateCalendar(Schedule schedule)
        {
            StringBuilder sb = new StringBuilder();
            
            if(schedule!=null)
            { 
            StringWriter writer = new StringWriter(sb);
            
            writer.Write("This will be a calendar");
            writer.Flush();
            writer.Close();
            }

            return sb.ToString();
        }
    }
}
