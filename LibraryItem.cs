namespace LibraryManagement;

// Part 1: abstract base class. It describes what EVERY library item has/does,
// but it is too general to exist on its own ("an item" is not a real thing on a shelf).
public abstract class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }

    // protected: only derived classes (Book, Magazine) can call it, through ': base(...)'
    protected LibraryItem(int id, string title, int year)
    {
        Id = id;
        Title = title;
        Year = year;
    }

    // ABSTRACT: no body here. Every concrete item MUST provide its own version.
    public abstract void DisplayInfo();

    // CONCRETE: has a body. All items share it as-is.
    public void PrintBasicInfo()
    {
        Console.WriteLine($"ID: {Id} | Title: {Title} | Year: {Year}");
    }
}
