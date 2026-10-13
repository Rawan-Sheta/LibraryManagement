namespace LibraryManagement;

// Part 6 - FILE 3 of 3: operators (Part 7).
public partial class Book
{
    // Operators are static methods. > and < MUST be overloaded as a pair.
    public static bool operator >(Book left, Book right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.PageCount > right.PageCount;
    }

    public static bool operator <(Book left, Book right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);
        return left.PageCount < right.PageCount;
    }

    // DESIGN DECISION for '+' (documented):
    // The result is a NEW Book (the operands are not modified) that represents the "combined volume":
    //   - PageCount : left.PageCount + right.PageCount   (required by the assignment)
    //   - Title     : "<left title> + <right title>"
    //   - Author    : "<left author> & <right author>"
    //   - Year      : the LATER of the two years (the combined edition cannot be older than its parts)
    //   - Id        : 0, meaning "not registered in the library yet" (an Id must be unique, so we
    //                 must not copy either operand's Id)
    public static Book operator +(Book left, Book right)
    {
        ArgumentNullException.ThrowIfNull(left);
        ArgumentNullException.ThrowIfNull(right);

        return new Book(
            id: 0,
            title: $"{left.Title} + {right.Title}",
            year: Math.Max(left.Year, right.Year),
            author: $"{left.Author} & {right.Author}",
            pageCount: left.PageCount + right.PageCount);
    }
}
