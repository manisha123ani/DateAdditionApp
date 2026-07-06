using System;

namespace DateAdditionApp
{
    public static class DateCalculator
    {
        private static readonly int[] DaysInMonthNonLeap =
        {
            31,28,31,30,31,30,31,31,30,31,30,31
        };

        private static readonly int[] DaysInMonthLeap =
        {
            31,29,31,30,31,30,31,31,30,31,30,31
        };

        // Cumulative days before each month
        private static readonly int[] MonthOffsetNonLeap =
        {
            0,31,59,90,120,151,181,212,243,273,304,334
        };

        private static readonly int[] MonthOffsetLeap =
        {
            0,31,60,91,121,152,182,213,244,274,305,335
        };

        public static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0)
                   || (year % 400 == 0);
        }

        public static int GetDaysInMonth(int month, int year)
        {
            return IsLeapYear(year)
                ? DaysInMonthLeap[month - 1]
                : DaysInMonthNonLeap[month - 1];
        }

        public static bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12)
                return false;

            return day >= 1 && day <= GetDaysInMonth(month, year);
        }

        // Date -> Day Number
        private static long DateToDayNumber(int day, int month, int year)
        {
            long years = year - 1;

            long totalDays =
                years * 365 +
                years / 4 -
                years / 100 +
                years / 400;

            totalDays += IsLeapYear(year)
                ? MonthOffsetLeap[month - 1]
                : MonthOffsetNonLeap[month - 1];

            totalDays += day;

            return totalDays;
        }

        // Day Number -> Date
        private static (int day, int month, int year) DayNumberToDate(long dayNumber)
        {
            int year = (int)(dayNumber / 365.2425) + 1;

            long firstDayOfYear = DateToDayNumber(1, 1, year);

            while (firstDayOfYear > dayNumber)
            {
                year--;
                firstDayOfYear = DateToDayNumber(1, 1, year);
            }

            while (true)
            {
                long nextYear = DateToDayNumber(1, 1, year + 1);

                if (nextYear > dayNumber)
                    break;

                year++;
                firstDayOfYear = nextYear;
            }

            int dayOfYear = (int)(dayNumber - firstDayOfYear + 1);

            int[] months = IsLeapYear(year)
                ? DaysInMonthLeap
                : DaysInMonthNonLeap;

            int month = 1;

            while (dayOfYear > months[month - 1])
            {
                dayOfYear -= months[month - 1];
                month++;
            }

            return (dayOfYear, month, year);
        }

        public static (int day, int month, int year) AddDays(
            int day,
            int month,
            int year,
            int daysToAdd)
        {
            if (!IsValidDate(day, month, year))
                throw new ArgumentException("Invalid Date");

            long dayNumber = DateToDayNumber(day, month, year);

            dayNumber += daysToAdd;

            if (dayNumber <= 0)
                throw new ArgumentException("Date out of range");

            return DayNumberToDate(dayNumber);
        }

        public static bool TryParseDate(
            string input,
            out int day,
            out int month,
            out int year)
        {
            day = month = year = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            string[] parts = input.Split('/');

            if (parts.Length != 3)
                return false;

            return int.TryParse(parts[0], out day)
                && int.TryParse(parts[1], out month)
                && int.TryParse(parts[2], out year)
                && IsValidDate(day, month, year);
        }
    }
}