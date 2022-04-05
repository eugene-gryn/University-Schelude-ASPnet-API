using System.Collections.Generic;

namespace ScheduleLogic.Subject.Couples
{
    public class Subject : IEqualityComparer<Subject>
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

        public bool Equals(Subject x, Subject y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Name == y.Name && x.isPractice == y.isPractice;
        }

        public int GetHashCode(Subject obj)
        {
            unchecked
            {
                return ((obj.Name != null ? obj.Name.GetHashCode() : 0) * 397) ^ obj.isPractice.GetHashCode();
            }
        }
    }
}