using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenUniversity
{
    public class OUFeed
    {
        private List<OUEvent> _events;

        public OUModule Module { get; set; }

        public List<OUEvent> Events 
        {
            get 
            {
                //Ensure the storage bin has been created
                if(_events == null)
                {
                    _events = new List<OUEvent>();
                }

                return _events;
            } 
            
            set
            {
                _events = value;
            }
        }
    }
}
