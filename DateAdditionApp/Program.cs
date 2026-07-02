using System;

namespace DateAdditionApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Date Addition (No built-in date libraries used) ===");
            Console.WriteLine();

            Console.Write("Enter date (dd/mm/yyyy): ");
            string? input = Console.ReadLine();

            Console.Write("Enter number of days to add: ");
            string? daysInput = Console.ReadLine();

            if (!DateCalculator.TryParseDate(input ?? string.Empty, out int day, out int month, out int year))
            {
                Console.WriteLine("Invalid date. Please use dd/mm/yyyy format and a real date.");
                return;
            }

            if (!int.TryParse(daysInput, out int daysToAdd))
            {
                Console.WriteLine("Invalid number of days.");
                return;
            }

            var (newDay, newMonth, newYear) = DateCalculator.AddDays(day, month, year, daysToAdd);

            Console.WriteLine();
            Console.WriteLine($"New Date: {newDay:D2}/{newMonth:D2}/{newYear}");
        }
    }
}
