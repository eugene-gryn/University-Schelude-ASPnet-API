using NUnit.Framework;
using ScheduleLogic.Subject.FileCoupleConvertors;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;

namespace LibraryTesting.ExcelTests
{
    public class ExcelScanTest
    {
        private const string FILENAME = "Schelude.xlsx";
        private const string NAME = "NAU schelude";

        const int WEEK_COUNT = 2;

        const int REPEAT_DUBLICATE_COUNT = 4;


        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ExcelScanFromFile()
        {
            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek(NAME, FILENAME);

            Assert.True(couples.Couples.Count > 0);
        }

        [Test]
        public void SameObjectsSubject()
        {
            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek(NAME, FILENAME);

            var monday1 = couples.FindByDate(ScheduleLogic.ScheduleEngine.FirstDayOnWeek());
            var monday2 = couples.FindByDate(ScheduleLogic.ScheduleEngine.FirstDayOnWeek().AddDays(7));

            Assert.AreSame(monday1[0].CoupleSubject, monday2[0].CoupleSubject);
        }

        [Test]
        public void DublicatorCouplesAfterScanning()
        {
            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek(NAME, FILENAME);

            Continuetor continuetor = new Continuetor(WEEK_COUNT);
            continuetor.Duplicate(couples, REPEAT_DUBLICATE_COUNT);

            var monday1 = couples.FindByDate(ScheduleLogic.ScheduleEngine.FirstDayOnWeek());
            var monday2 = couples.FindByDate(ScheduleLogic.ScheduleEngine.FirstDayOnWeek().AddDays(14));

            Assert.True(monday1[0].Begin.TimeOfDay == monday2[0].Begin.TimeOfDay);
        }

        [Test]
        public void DuplictorAfterScanningCountCorrectly()
        {

            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek("NAU schelude", "Schelude.xlsx");

            var countBefore = couples.Couples.Count;

            Continuetor continuetor = new Continuetor(WEEK_COUNT);
            continuetor.Duplicate(couples, REPEAT_DUBLICATE_COUNT);

            var countAfter = couples.Couples.Count;


            Assert.True(countAfter/countBefore == REPEAT_DUBLICATE_COUNT);
        }
    }
}
