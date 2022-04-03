namespace ScheduleLogic.Subject
{
    public class Subject
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