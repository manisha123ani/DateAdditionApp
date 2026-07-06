using System;
using Xunit;
using DateAdditionApp;

namespace DateAdditionApp.Tests
{
    public class DateCalculatorTests
    {
        // ---------- AddDays: core requirement example ----------

        [Fact]
        public void AddDays_GivenAssignmentExample_ReturnsExpectedDate()
        {
            // 31/01/2016 + 1 day => 01/02/2016 (from the assignment spec)
            var result = DateCalculator.AddDays(31, 1, 2016, 1);

            Assert.Equal((1, 2, 2016), result);
        }

        // ---------- Month rollover ----------

        [Theory]
        [InlineData(28, 2, 2023, 1, 1, 3, 2023)]   // non-leap Feb -> March
        [InlineData(30, 4, 2024, 1, 1, 5, 2024)]   // 30-day month rollover
        [InlineData(31, 12, 2023, 1, 1, 1, 2024)]  // year rollover
        public void AddDays_MonthAndYearRollover_WorksCorrectly(
            int day, int month, int year, int daysToAdd,
            int expDay, int expMonth, int expYear)
        {
            var result = DateCalculator.AddDays(day, month, year, daysToAdd);

            Assert.Equal((expDay, expMonth, expYear), result);
        }

        // ---------- Leap year handling ----------

        [Fact]
        public void AddDays_LeapYearFebruary_Has29Days()
        {
            // 28/02/2024 + 1 day => 29/02/2024 (2024 is a leap year)
            var result = DateCalculator.AddDays(28, 2, 2024, 1);

            Assert.Equal((29, 2, 2024), result);
        }

        [Fact]
        public void AddDays_NonLeapYearFebruary_RollsOverAt28()
        {
            // 28/02/2023 + 1 day => 01/03/2023 (2023 is not a leap year)
            var result = DateCalculator.AddDays(28, 2, 2023, 1);

            Assert.Equal((1, 3, 2023), result);
        }

        [Theory]
        [InlineData(2000, true)]   // divisible by 400 -> leap
        [InlineData(1900, false)]  // divisible by 100 but not 400 -> not leap
        [InlineData(2024, true)]   // divisible by 4, not by 100 -> leap
        [InlineData(2023, false)]  // not divisible by 4 -> not leap
        public void IsLeapYear_ReturnsExpectedResult(int year, bool expected)
        {
            Assert.Equal(expected, DateCalculator.IsLeapYear(year));
        }

        // ---------- Adding zero / large day counts ----------

        [Fact]
        public void AddDays_ZeroDays_ReturnsSameDate()
        {
            var result = DateCalculator.AddDays(15, 6, 2024, 0);

            Assert.Equal((15, 6, 2024), result);
        }

        [Fact]
        public void AddDays_MultipleYearRollover_WorksCorrectly()
        {
            // 01/01/2024 + 366 days (2024 is a leap year) => 01/01/2025
            var result = DateCalculator.AddDays(1, 1, 2024, 366);

            Assert.Equal((1, 1, 2025), result);
        }

        // ---------- Negative days (subtraction) ----------

        [Fact]
        public void AddDays_NegativeDays_SubtractsCorrectly()
        {
            // 01/03/2023 - 1 day => 28/02/2023 (non-leap year)
            var result = DateCalculator.AddDays(1, 3, 2023, -1);

            Assert.Equal((28, 2, 2023), result);
        }

        [Fact]
        public void AddDays_NegativeDaysAcrossYear_SubtractsCorrectly()
        {
            // 01/01/2024 - 1 day => 31/12/2023
            var result = DateCalculator.AddDays(1, 1, 2024, -1);

            Assert.Equal((31, 12, 2023), result);
        }

        // ---------- Invalid input ----------

        [Fact]
        public void AddDays_InvalidStartingDate_ThrowsArgumentException()
        {
            // 31/02/2023 does not exist
            Assert.Throws<ArgumentException>(() => DateCalculator.AddDays(31, 2, 2023, 1));
        }

        // ---------- GetDaysInMonth ----------

        [Theory]
        [InlineData(1, 2024, 31)]
        [InlineData(2, 2024, 29)]  // leap year
        [InlineData(2, 2023, 28)]  // non-leap year
        [InlineData(4, 2024, 30)]
        [InlineData(12, 2024, 31)]
        public void GetDaysInMonth_ReturnsCorrectDayCount(int month, int year, int expectedDays)
        {
            Assert.Equal(expectedDays, DateCalculator.GetDaysInMonth(month, year));
        }

        [Fact]
        public void GetDaysInMonth_InvalidMonth_ThrowsArgumentOutOfRangeException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => DateCalculator.GetDaysInMonth(13, 2024));
        }

        // ---------- IsValidDate ----------

        [Theory]
        [InlineData(29, 2, 2024, true)]   // leap year Feb 29 exists
        [InlineData(29, 2, 2023, false)]  // non-leap year Feb 29 doesn't exist
        [InlineData(31, 4, 2024, false)]  // April has only 30 days
        [InlineData(1, 13, 2024, false)]  // invalid month
        [InlineData(15, 6, 2024, true)]   // normal valid date
        public void IsValidDate_ReturnsExpectedResult(int day, int month, int year, bool expected)
        {
            Assert.Equal(expected, DateCalculator.IsValidDate(day, month, year));
        }

        // ---------- TryParseDate ----------

        [Fact]
        public void TryParseDate_ValidInput_ReturnsTrueAndCorrectValues()
        {
            bool success = DateCalculator.TryParseDate("31/01/2016", out int day, out int month, out int year);

            Assert.True(success);
            Assert.Equal((31, 1, 2016), (day, month, year));
        }

        [Theory]
        [InlineData("31-01-2016")]  // wrong separator
        [InlineData("31/02/2023")]  // invalid date (Feb has 28 days in 2023)
        [InlineData("abc/01/2016")] // non-numeric
        [InlineData("")]            // empty
        [InlineData(null)]          // null
        public void TryParseDate_InvalidInput_ReturnsFalse(string? input)
        {
            bool success = DateCalculator.TryParseDate(input!, out _, out _, out _);

            Assert.False(success);
        }
    }
}
