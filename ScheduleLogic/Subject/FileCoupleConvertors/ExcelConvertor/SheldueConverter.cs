using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using ScheduleLogic.Subject.Couples;
using ExcelDataReader;

namespace ScheduleLogic.Subject.FileCoupleConvertors.ExcelConvertor
{
    public class ExcelScheludeConverter : IConverter
    {
        private const int TIME_ROW = 1;
        private const int COLUMN_OF_COUPLES = 0;
        private const int ROW_OF_DAYS = 1;
        private const int ROW_FIRST_DAY = 2;
        private const int ROW_FIRST_COUPLE = 2;

        private const int GROUP_BEGIN_TIME_MIN = 4;
        private const int GROUP_BEGIN_TIME_HS = 2;

        private const int GROUP_END_TIME_MIN = 8;
        private const int GROUP_END_TIME_HS = 6;
        
        private const int SCHELUDE_WEEKDAY_OFFSET = 1;

        private readonly Regex practiceView = new Regex(@"\*(\w+)");
        private readonly Regex time = new Regex(@"((\d{1}|\d{2})(\:|\.)(\d{2}))-((\d{1}|\d{2})(\:|\.)(\d{2}))");


        public ExcelScheludeConverter()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public CoupleManager GetSubjectWeek(string name, string filename)
        {
            var couples = new CoupleManager(name, new List<Couple>());

            var table = ExactData(filename);

            while (table.Count > 0)
            {
                var weekCouples = GetOneWeek(ref table);
                couples.Merge(weekCouples);
            }

            return couples;
        }

        /// <summary>
        ///     Gets data from real Excel document and put it into massive
        /// </summary>
        /// <param name="filename">File of the Excel data</param>
        /// <returns>Getet info in usable format</returns>
        private List<List<string>> ExactData(string filename)
        {
            var file = new List<List<string>>();

            using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            file.Add(new List<string>());
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                var line = reader.GetValue(i);

                                if (line != null)
                                    file[file.Count - 1].Add(line.ToString());
                                else
                                    file[file.Count - 1].Add("");
                            }
                        }
                    } while (reader.NextResult());
                }
            }

            return file;
        }

        private int GetCouplesCount(List<List<string>> table)
        {
            var realCount = 0;
            int result;
            for (var row = 2; row < table.Count; row++)
                if (int.TryParse(table[row][COLUMN_OF_COUPLES], out result))
                    realCount++;
                else
                    break;
            return realCount;
        }

        private int GetDaysCount(List<List<string>> table)
        {
            var realCount = 0;
            for (var days = ROW_FIRST_DAY; days < table[ROW_OF_DAYS].Count; days++)
                if (!string.IsNullOrEmpty(table[ROW_OF_DAYS][days]))
                    realCount++;
            return realCount;
        }

        /// <summary>
        ///     Cuts name subject (remove * on begin), return practice or not
        /// </summary>
        /// <param name="subjectName">String of subject name</param>
        /// <returns>Is that subject is pratice</returns>
        private bool IsPracticeCuts(ref string subjectName)
        {
            if (practiceView.IsMatch(subjectName))
            {
                subjectName = subjectName.Remove(0, 1);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Parses begin default time of couple
        /// </summary>
        /// <param name="timeRow">string from parse time</param>
        /// <returns>Time begin couple</returns>
        private TimeSpan TableCoupleTimeToTimeBEGIN(string timeRow)
        {
            var match = time.Match(timeRow);

            return new TimeSpan(int.Parse(match.Groups[GROUP_BEGIN_TIME_HS].Value),
                int.Parse(match.Groups[GROUP_BEGIN_TIME_MIN].Value), 0);
        }

        /// <summary>
        ///     Parses end default time of couple
        /// </summary>
        /// <param name="timeRow">string from parse time</param>
        /// <returns>Time end couple</returns>
        private TimeSpan TableCoupleTimeToTimeEND(string timeRow)
        {
            var match = time.Match(timeRow);

            return new TimeSpan(int.Parse(match.Groups[GROUP_END_TIME_HS].Value),
                int.Parse(match.Groups[GROUP_END_TIME_MIN].Value), 0);
        }

        /// <summary>
        ///     Parse times of every couple
        /// </summary>
        /// <param name="table">parse file</param>
        /// <param name="CountCouples">count couples to parse</param>
        /// <returns>Timed class of the sheldue</returns>
        private ScheduleTiming ParseScheludeTimings(List<List<string>> table, int CountCouples)
        {
            var timings = new ScheduleTiming(CountCouples);

            for (var couple = 0; couple < CountCouples; couple++)
            {
                var timeStr = table[couple + 2][TIME_ROW];

                timings.times[couple] = new CoupleDefaultTime(
                    TableCoupleTimeToTimeBEGIN(timeStr),
                    TableCoupleTimeToTimeEND(timeStr)
                );
            }

            return timings;
        }

        private DateTime firstDayOnWeek()
        {
            // Day of week
            int offsetDay = (0 - (int)DateTime.Now.DayOfWeek + SCHELUDE_WEEKDAY_OFFSET);
            offsetDay += (Schedule.CurrentWeek() == 0) ? 0 : -7;

            return DateTime.Now.Date.AddDays(offsetDay);
        }

        /// <summary>
        ///     Method cuts from table one week
        /// </summary>
        /// <param table="Table of the Excel document. Page with Sheldue"></param>
        /// <returns>Object of week sheldue</returns>
        private CoupleManager GetOneWeek(ref List<List<string>> table)
        {
            // Define Week Info
            var countOfCouples = GetCouplesCount(table);
            var countOfDays = GetDaysCount(table);
            var couples = new List<Couple>();

            // Init week
            var oneWeek = new CoupleManager(table[0][0], couples)
            {
                Timing = ParseScheludeTimings(table, countOfCouples)
            };

            // Constructing week
            for (var day = 0; day < countOfDays; day++)
            {
                for (var couple = 0; couple < countOfCouples; couple++)
                {

                    // <!!--<>--!!> Добавление уже существующих пар, вместо конструкции новой пары

                    if (!string.IsNullOrEmpty(table[couple + ROW_FIRST_COUPLE][day + ROW_FIRST_DAY]))
                    {
                        var subName = table[couple + ROW_FIRST_COUPLE][day + ROW_FIRST_DAY];
                        var isPractice = IsPracticeCuts(ref subName);

                        var weekDay = firstDayOnWeek();
                        weekDay = weekDay.AddDays(day).Date;

                        var begin = weekDay.AddTicks(oneWeek.Timing.times[couple].starts.Ticks);
                        var end = weekDay.AddTicks(oneWeek.Timing.times[couple].ends.Ticks);

                        var creationCouple = new Couple(begin, end,
                            Couples.SubjectSource.GetSubject(subName, isPractice));

                        oneWeek.Couples.Add(creationCouple);
                    }
                }

            }


            // Delete week from table
            table.RemoveRange(0, countOfCouples + ROW_FIRST_COUPLE);

            return oneWeek;
        }
    }
}