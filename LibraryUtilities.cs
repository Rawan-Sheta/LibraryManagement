namespace LibraryManagement;

// Part 4: static class. No instances, only static members, nothing to remember between calls.
public static class LibraryUtilities
{
    private const decimal DefaultFeePerDay = 2.0m;

    // Late fee = overdue days x fee per day (0 if the book is not overdue)
    public static decimal CalculateLateFee(int overdueDays)
    {
        return CalculateLateFee(overdueDays, DefaultFeePerDay);
    }

    public static decimal CalculateLateFee(int overdueDays, decimal feePerDay)
    {
        if (overdueDays <= 0) return 0m;
        return overdueDays * feePerDay;
    }

    // Example: ("Clean Code", 2008) -> "CLE-2008"
    public static string GenerateBookCode(string title, int year)
    {
        string letters = new string(title.Where(char.IsLetter).Take(3).ToArray()).ToUpper();
        letters = letters.PadRight(3, 'X'); // titles with fewer than 3 letters
        return $"{letters}-{year}";
    }

    public static void PrintSeparator(char symbol = '-', int length = 50)
    {
        Console.WriteLine(new string(symbol, length));
    }
}
