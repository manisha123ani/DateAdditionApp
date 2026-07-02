# Date Addition App

Console app that adds a given number of days to a `dd/mm/yyyy` date — **without using any date/time library** (no `DateTime`, no third-party date packages). All date math (leap years, month rollover, year rollover) is done manually.

## How to Run in Visual Studio

1. Open **Visual Studio** → `File` → `Open` → `Project/Solution`.
2. Select `DateAdditionApp.csproj`.
3. Press `F5` (or `Ctrl+F5` to run without debugging).
4. In the console window, enter:
   - Date in `dd/mm/yyyy` format (e.g. `31/01/2016`)
   - Number of days to add (e.g. `1`)
5. Output:
   ```
   New Date: 01/02/2016
   ```

## How to Run from Command Line (alternative)

```bash
cd DateAdditionApp
dotnet run
```

Requires [.NET 8 SDK](https://dotnet.microsoft.com/download) installed.

## Running Unit Tests

The solution includes a separate `DateAdditionApp.Tests` project (xUnit) covering `DateCalculator`: assignment example, month/year rollover, leap years, negative days, zero days, invalid dates, and parsing.

**In Visual Studio:**
1. Open `Test Explorer` (`Test` → `Test Explorer`).
2. Click `Run All Tests`.

**From command line:**
```bash
cd DateAdditionApp.Tests
dotnet test
```

## Notes

- Handles leap years correctly (`(year % 4 == 0 && year % 100 != 0) || year % 400 == 0`).
- Supports both adding days (positive) and subtracting days (negative number).
- Includes basic input validation (invalid format, non-existent dates like `31/02/2016`).
- Target framework: `.NET 8.0` — change `<TargetFramework>` in the `.csproj` if you're on a different version (e.g. `net6.0`).
