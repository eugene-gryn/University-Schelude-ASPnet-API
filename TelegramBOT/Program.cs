using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TelegramBOT
{
    public class Program
    {
        private const int starts_encoding = 96;

        public static string StringConvertor(string line)
        {
            string modifiedLine = line.ToLower();
            StringBuilder endString = new StringBuilder();

            foreach (var letter in modifiedLine)
            {
                if (char.IsLetter(letter))
                {
                    endString.Append((letter - starts_encoding) + " ");
                }
            }

            return endString.ToString();
        }

        public static string StringDictionaryConverter(string line)
        {
            Dictionary<char, int> letterNumbers = new Dictionary<char, int>();
            StringBuilder retValue = new StringBuilder();

            int j = 1;
            for (var i = 'a'; i < 'z'; i++, j++)
            {
                letterNumbers.Add(i, j);
            }

            foreach (var letter in line.ToLower())
            {
                if (char.IsLetter(letter))
                {
                    retValue.Append(letterNumbers[letter] + " ");
                }
            }

            return retValue.ToString();
        }

        public static void Main(string[] args)
        {
            var line = "The sunset sets at twelve o' clock";
            Console.WriteLine(TelegramBOT.Program.StringConvertor(line));
            Console.WriteLine(TelegramBOT.Program.StringDictionaryConverter(line));
        }
    }
}