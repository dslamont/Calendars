using System;

namespace OpenUniversity
{
    public class OUEvent
    {
        public string Id { get; set; }
        public DateTime StateDate { get; set; }
        public string Title { get; set; }
        public string OrganizerName { get; set; }
        public string OrganizerEmail { get; set; }
        public int UpdateCount { get; set; }
    }
}
