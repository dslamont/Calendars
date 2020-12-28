using System;
using System.Collections.Generic;
using System.Text;

namespace BookGroup
{
    public class Schedule
    {
        protected List<Meeting> _meetings;

        public List<Meeting> Meetings 
        {
            get
            { 
                if(_meetings==null)
                {
                    _meetings = new List<Meeting>();
                }

                return _meetings;
            }

            set
            {
                _meetings = value;
            } 
        }
    }
}
