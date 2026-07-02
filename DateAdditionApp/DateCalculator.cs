using System;

namespace DateAdditionApp
{
    /// <summary>
    /// Contains all date-math logic, built from scratch without using
    /// DateTime or any date/time library. Kept separate from Program.cs
    /// so it can be unit tested independently of console I/O.
    /// </summary>
    public static class DateCalculator
    {
        /// <summary>
        /// Adds (or subtracts, if negative) the given number of days to a date,
        /// one day at a time.
        /// </summary>
        public static (int day, int month, int year) AddDays(int day, int month, int year, int daysToAdd)
        {
            if (!IsValidDate(day, month, year))
            {
                throw new ArgumentException("The date entered does not exist.");
            }

            if (daysToAdd >= 0)
            {
                for (int i = 0; i < daysToAdd; i++)
                {
                    day++;

                    int maxDaysInMonth = GetDaysInMonth(month, year);

                    if (day > maxDaysInMonth)
                    {
                        day = 1;
                        month++;

                        if (month > 12)
                        {
                            month = 1;
                            year++;
                        }
                    }
                }
            }
            else
            {
                int daysToSubtract = -daysToAdd;

                for (int i = 0; i < daysToSubtract; i++)
                {
                    day--;

                    if (day < 1)
                    {
                        month--;

                        if (month < 1)
                        {
                            month = 12;
                            year--;
                        }

                        day = GetDaysInMonth(month, year);
                    }
                }
            }

            return (day, month, year);
        }

        public static bool IsLeapYear(int year)
        {
            return (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);
        }

        public static int GetDaysInMonth(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
            }

            int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

            if (month == 2 && IsLeapYear(year))
            {
                return 29;
            }

            return daysInMonth[month - 1];
        }

        public static bool IsValidDate(int day, int month, int year)
        {
            if (year < 1 || month < 1 || month > 12)
            {
                return false;
            }

            int maxDay = GetDaysInMonth(month, year);
            return day >= 1 && day <= maxDay;
        }

        /// <summary>
        /// Parses a "dd/mm/yyyy" string. Returns false if the format or
        /// the date itself is invalid.
        /// </summary>
        public static bool TryParseDate(string input, out int day, out int month, out int year)
        {
            day = month = year = 0;

            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string[] parts = input.Split('/');

            if (parts.Length != 3
                || !int.TryParse(parts[0], out day)
                || !int.TryParse(parts[1], out month)
                || !int.TryParse(parts[2], out year))
            {
                return false;
            }

            return IsValidDate(day, month, year);
        }
    }
}
