using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace SheldueLogic.SheldueObj
{
    public interface ISheldueConverter
    {
        List<SubjectWeek> GetSubjectWeek(string filename);
    }

    public class ExcelSheldueConverter : ISheldueConverter
    {
        private const int TIME_ROW = 1;

        public ExcelSheldueConverter()
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Gets data from real Excel document and put it into massive
        /// </summary>
        /// <param name="filename">File of the Excel data</param>
        /// <returns>Getet info in usable format</returns>
        private List<List<string>> ExactData(string filename)
        {
            List<List<string>> file = new List<List<string>>();

            using (FileStream stream = File.Open(filename, FileMode.Open, FileAccess.Read))
            {
                using (IExcelDataReader reader = ExcelReaderFactory.CreateReader(stream))
                {
                    do
                    {
                        while (reader.Read())
                        {
                            file.Add(new List<string>());
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                object line = reader.GetValue(i);

                                if (line != null)
                                {
                                    file[file.Count - 1].Add(line.ToString());
                                }
                                else
                                {
                                    file[file.Count - 1].Add("");
                                }
                            }
                        }
                    } while (reader.NextResult());
                }
            }
            return file;
        }

        private int GetCouplesCount(List<List<string>> table)
        {
            int COLUMN_OF_COUPLES = 0;

            int RealCount = 0;
            int result;
            for (int row = 2; row < table.Count; row++)
            {
                if (int.TryParse(table[row][COLUMN_OF_COUPLES], out result))
                {
                    RealCount++;
                }
                else
                {
                    break;
                }
            }
            return RealCount;
        }

        private int GetDaysCount(List<List<string>> table)
        {
            int ROW_OF_DAYS = 1;


            int RealCount = 0;
            for (int days = 2; days < table[ROW_OF_DAYS].Count; days++)
            {
                if (!string.IsNullOrEmpty(table[ROW_OF_DAYS][days]))
                {
                    RealCount++;
                }
            }
            return RealCount;
        }

        /// <summary>
        /// Cuts name subject (remove * on begin), return practice or not
        /// </summary>
        /// <param name="subjectName">String of subject name</param>
        /// <returns>Is that subject is pratice</returns>
        private bool isSubjectPractise(ref string subjectName)
        {
            Regex practiceView = new Regex(@"\*(\w+)");

            if (practiceView.IsMatch(subjectName))
            {
                subjectName = subjectName.Remove(0, 1);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Parses begin default time of couple
        /// </summary>
        /// <param name="timeRow">string from parse time</param>
        /// <returns>Time begin couple</returns>
        private TimeSpan TableCoupleTimeToTimeBEGIN(string timeRow)
        {
            Regex time = new Regex(@"((\d{1}|\d{2})(\:|\.)(\d{2}))-((\d{1}|\d{2})(\:|\.)(\d{2}))");

            Match match;
            match = time.Match(timeRow);

            return new TimeSpan(int.Parse(match.Groups[2].Value), int.Parse(match.Groups[4].Value), 0);
        }

        /// <summary>
        /// Parses end default time of couple
        /// </summary>
        /// <param name="timeRow">string from parse time</param>
        /// <returns>Time end couple</returns>
        private TimeSpan TableCoupleTimeToTimeEND(string timeRow)
        {
            Regex time = new Regex(@"((\d{1}|\d{2})(\:|\.)(\d{2}))-((\d{1}|\d{2})(\:|\.)(\d{2}))");

            Match match;
            match = time.Match(timeRow);

            return new TimeSpan(int.Parse(match.Groups[6].Value), int.Parse(match.Groups[8].Value), 0);
        }

        /// <summary>
        /// Parse times of every couple
        /// </summary>
        /// <param name="table">parse file</param>
        /// <param name="CountCouples">count couples to parse</param>
        /// <returns>Timed class of the sheldue</returns>
        private SheldueTiming ParseSheldueTimings(List<List<string>> table, int CountCouples)
        {
            SheldueTiming timings = new SheldueTiming(CountCouples);

            for (int couple = 0; couple < CountCouples; couple++)
            {
                string timeStr = table[couple + 2][TIME_ROW];

                timings.times[couple] = new CoupleDefaultTime(
                    TableCoupleTimeToTimeBEGIN(timeStr),
                    TableCoupleTimeToTimeEND(timeStr)
                    );
            }

            return timings;
        }

        /// <summary>
        /// Method cuts from table one week
        /// </summary>
        /// <param table="Table of the Excel document. Page with Sheldue"></param>
        /// <returns>Object of week sheldue</returns>
        public SubjectWeek GetOneWeek(ref List<List<string>> table)
        {
            // Week Info
            int CountOfCouples, CountOfDays;

            // Define Week Info
            CountOfCouples = GetCouplesCount(table);
            CountOfDays = GetDaysCount(table);

            // Init week
            SubjectWeek valueWeek = new SubjectWeek(table[0][0], CountOfCouples)
            {
                timing = ParseSheldueTimings(table, CountOfCouples)
            };

            // Constructing week
            for (int couple = 0; couple < CountOfCouples; couple++)
            {
                for (int day = 0; day < CountOfDays; day++)
                {
                    // <!!--<>--!!> Добавление уже существующих пар, вместо конструкции новой пары

                    if (!string.IsNullOrEmpty(table[couple + 2][day + 2]))
                    {
                        string SubName = table[couple + 2][day + 2];
                        bool isPractice = isSubjectPractise(ref SubName);

                        valueWeek.days[day].Couples[couple] = new Couple(
                                valueWeek.timing.times[couple].starts,
                                valueWeek.timing.times[couple].ends,
                                new Subject(SubName, isPractice));
                    }
                }
            }

            // Delete week from table
            table.RemoveRange(0, CountOfCouples + 2);

            return valueWeek;
        }

        /// <summary>
        /// Parse subject weeks
        /// </summary>
        /// <param name="filename">Name of parse file</param>
        /// <returns>List of SubjectWeeks from parsed file</returns>
        public List<SubjectWeek> GetSubjectWeek(string filename)
        {
            List<List<string>> table = ExactData(filename);
            List<SubjectWeek> weeks = new List<SubjectWeek>();

            while (table.Count > 0)
            {
                weeks.Add(GetOneWeek(ref table));
            }

            return weeks;
        }

    }
}
