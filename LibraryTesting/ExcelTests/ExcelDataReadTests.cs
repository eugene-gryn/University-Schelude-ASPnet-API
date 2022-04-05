using NUnit.Framework;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;

namespace LibraryTesting.ExcelTests
{
    public class ExcelDataReadTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ExcelReadCouplesCount()
        {
            var reader = new ExcelScheludeConverter();

            var list = reader.GetSubjectWeek("NAU schelude","Schelude.xlsx");

            Assert.True(list.Couples.Count > 0);
        }
    }
}