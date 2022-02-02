using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SheldueLogic.SheldueObj
{
    public struct Subject
    {
        public string Name;
        public string GoogleMeetUrl;
        public bool isPractice;

        public Subject(string name, bool isPractice, string googleMeetUrl = null)
        {
            Name = name;
            GoogleMeetUrl = googleMeetUrl;
            this.isPractice = isPractice;
        }
    }
}
