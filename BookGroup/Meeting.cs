using System;

namespace BookGroup
{
    public class Meeting
    {
        public DateTime StateDate { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string BookUrl { get; set; }
        public string CoverImageUrl { get; set; }
        public string OrganizerName { get; set; }
        public string OrganizerEmail { get; set; }
        public string Location { get; set; }
        public int UpdateCount { get; set; }
    }
}
