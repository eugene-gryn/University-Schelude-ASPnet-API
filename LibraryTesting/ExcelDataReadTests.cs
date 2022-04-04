using NUnit.Framework;
using ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor;

namespace LibraryTesting
{
    public class ExcelDataReadTests
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void ExcelRead()
        {
            var reader = new ExcelSheldueConverter();

            var list = reader.GetSubjectWeek("//src/Schelude.xlsx");

            Assert.True(list.Capacity > 0);
        }
    }
}