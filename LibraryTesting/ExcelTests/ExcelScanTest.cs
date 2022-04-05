using NUnit.Framework;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;

namespace LibraryTesting.ExcelTests
{
    public class ExcelScanTest
    {

        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void ExcelScanFromFile()
        {
            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek("NAU schelude", "Schelude.xlsx");

            Assert.True(couples.Couples.Count > 0);
        }

        [Test]
        public void SameObjectsSubject()
        {
            var reader = new ExcelScheludeConverter();

            var couples = reader.GetSubjectWeek("NAU schelude", "Schelude.xlsx");

            var monday1 = couples.FindByDate(ScheduleLogic.Schedule.FirstDayOnWeek());
            var monday2 = couples.FindByDate(ScheduleLogic.Schedule.FirstDayOnWeek().AddDays(7));

            Assert.AreSame(monday1[0].CoupleSubject, monday2[0].CoupleSubject);
        }
    }
}
