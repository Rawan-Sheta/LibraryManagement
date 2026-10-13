namespace LibraryManagement;

// Part 6 - FILE 2 of 3: display methods.
public partial class Book
{
    // Provides the body for the abstract method declared in LibraryItem.
    public override void DisplayInfo()
    {
        Console.WriteLine($"[Book] \"{Title}\" by {Author} ({Year}) - {PageCount} pages");
    }

    public void PrintBookDetails()
    {
        PrintBasicInfo(); // concrete method inherited from LibraryItem
        Console.WriteLine($"Author: {Author} | Pages: {PageCount}");
    }
}
