namespace ScheduleLogic.Subject
{
    public class SubjectWeek
    {
        public Day[] days;
        public SheldueTiming timing;
        public string WeekName;


        public SubjectWeek()
        {
            WeekName = "";
            timing = new SheldueTiming(0);

            // Init days
            days = new Day[7];
            for (var i = 0; i < days.Length; i++) days[i] = new Day();
        }

        public SubjectWeek(string weekName, int CoupleCount)
        {
            WeekName = weekName;
            timing = new SheldueTiming(CoupleCount);

            // Init days
            days = new Day[7];
            for (var i = 0; i < days.Length; i++) days[i] = new Day();
        }

        public SubjectWeek(in SubjectWeek other)
        {
            for (var i = 0; i < other.days.Length; i++) days[i] = new Day(other.days[i]);
            timing = new SheldueTiming(other.timing);
        }
    }
}