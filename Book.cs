namespace LibraryManagement;

// Part 6 - FILE 1 of 3: properties, constructor, IComparable<Book> and ICloneable.
// The base class and the interfaces only need to be listed on ONE of the parts.
public partial class Book : LibraryItem, IComparable<Book>, ICloneable
{
    public string Author { get; set; }
    public int PageCount { get; set; }

    public Book(int id, string title, int year, string author, int pageCount)
        : base(id, title, year)
    {
        Author = author;
        PageCount = pageCount;
    }

    // Part 2: IComparable<Book>
    // negative -> this book comes BEFORE other
    // zero     -> same position (equal for sorting)
    // positive -> this book comes AFTER other
    public int CompareTo(Book? other)
    {
        if (other is null) return 1; // any real object is "greater" than null

        int byYear = Year.CompareTo(other.Year);
        if (byYear != 0) return byYear;

        // same year -> compare by title
        return string.Compare(Title, other.Title, StringComparison.Ordinal);
    }

    // Part 3: ICloneable
    // MemberwiseClone creates a NEW Book object and copies every field into it (shallow copy).
    // It is safe here because all our fields are value types or strings (strings are immutable).
    public object Clone()
    {
        return MemberwiseClone();
    }
}
