using System.Collections.Concurrent;
using LibraryManagement;

void Section(string title)
{
    Console.WriteLine();
    LibraryUtilities.PrintSeparator('=');
    Console.WriteLine(title);
    LibraryUtilities.PrintSeparator('=');
}

// ---------------------------------------------------------------------------
// 1. Abstract class: LibraryItem -> Book, Magazine  (polymorphism)
// ---------------------------------------------------------------------------
Section("1. Abstract class + polymorphism");

LibraryItem item = new Book(1, "Clean Code", 2008, "Robert C. Martin", 464);
item.DisplayInfo();      // runs Book.DisplayInfo (late binding)
item.PrintBasicInfo();   // runs the shared LibraryItem method

var items = new List<LibraryItem>
{
    item,
    new Magazine(2, "National Geographic", 2024, 5)
};
foreach (LibraryItem libraryItem in items)
{
    libraryItem.DisplayInfo(); // each object runs ITS OWN version
}

// var x = new LibraryItem(...);  // compile error CS0144: cannot create an instance of an abstract class

// ---------------------------------------------------------------------------
// 2. IComparable<Book>: books.Sort()
// ---------------------------------------------------------------------------
Section("2. IComparable<Book> - sorting by year, then title");

var books = new List<Book>
{
    new Book(1, "Clean Code", 2008, "Robert C. Martin", 464),
    new Book(2, "The Pragmatic Programmer", 1999, "Andrew Hunt & David Thomas", 352),
    new Book(3, "Design Patterns", 1994, "Erich Gamma et al.", 395),
    new Book(4, "C# in Depth", 2019, "Jon Skeet", 528),
    new Book(5, "Refactoring", 1999, "Martin Fowler", 448) // same year as book 2 -> tests the title tie-break
};

Console.WriteLine("Before sorting:");
books.ForEach(b => Console.WriteLine($"  {b.Year} - {b.Title}"));

books.Sort(); // no comparer needed: Book implements IComparable<Book>

Console.WriteLine("After sorting:");
books.ForEach(b => Console.WriteLine($"  {b.Year} - {b.Title}"));

// ---------------------------------------------------------------------------
// 3. ICloneable
// ---------------------------------------------------------------------------
Section("3. ICloneable");

var originalBook = new Book(1, "Clean Code", 2008, "Robert C. Martin", 464);
var clonedBook = (Book)originalBook.Clone(); // Clone() returns object -> cast
clonedBook.Title += " - Copy";

Console.WriteLine($"Original: {originalBook.Title}");
Console.WriteLine($"Clone   : {clonedBook.Title}");
Console.WriteLine($"Same object? {ReferenceEquals(originalBook, clonedBook)}");

// Contrast: copying the REFERENCE
Book alias = originalBook;               // NOT a copy: two names for the same object
alias.Title = "Changed through alias";
Console.WriteLine($"After changing the alias -> original title: {originalBook.Title}");
alias.Title = "Clean Code";              // restore

// ---------------------------------------------------------------------------
// 4. Static class
// ---------------------------------------------------------------------------
Section("4. Static class - LibraryUtilities");

Console.WriteLine($"Late fee for 5 days  : {LibraryUtilities.CalculateLateFee(5)}");
Console.WriteLine($"Late fee for 3 days @ 4.5: {LibraryUtilities.CalculateLateFee(3, 4.5m)}");
Console.WriteLine($"Book code            : {LibraryUtilities.GenerateBookCode("Clean Code", 2008)}");
LibraryUtilities.PrintSeparator('*', 30);
// var u = new LibraryUtilities();  // compile error CS0723: cannot declare a variable of static type

// ---------------------------------------------------------------------------
// 5. Sealed class
// ---------------------------------------------------------------------------
Section("5. Sealed class - LibraryCard");

var card = new LibraryCard("LC-1001", "Mona Hassan", DateTime.Today.AddYears(1));
card.DisplayCard();
// public class PremiumLibraryCard : LibraryCard { }
// ^ invalid: CS0509 "cannot derive from sealed type". 'sealed' means this class is final,
//   so the compiler refuses any class that tries to inherit from it.

// ---------------------------------------------------------------------------
// 6. Partial class
// ---------------------------------------------------------------------------
Section("6. Partial class - Book is split over 3 files");

Console.WriteLine("Book.cs (properties, constructor, IComparable, ICloneable)");
Console.WriteLine("Book.Display.cs (DisplayInfo, PrintBookDetails)");
Console.WriteLine("Book.Operators.cs (>, <, +)");
originalBook.PrintBookDetails(); // defined in Book.Display.cs
Console.WriteLine($"Runtime type: {originalBook.GetType().Name} (ONE class, not three)");

// ---------------------------------------------------------------------------
// 7. Operator overloading
// ---------------------------------------------------------------------------
Section("7. Operator overloading");

Book book1 = new(1, "Clean Code", 2008, "Robert C. Martin", 464);
Book book2 = new(2, "Design Patterns", 1994, "Erich Gamma et al.", 395);

if (book1 > book2)
{
    Console.WriteLine("Book 1 has more pages.");
}
if (book1 < book2)
{
    Console.WriteLine("Book 2 has more pages.");
}

Book combined = book1 + book2;
Console.WriteLine($"Combined: {combined.Title} | {combined.Author} | {combined.Year} | " +
                  $"pages = {combined.PageCount} (= {book1.PageCount} + {book2.PageCount})");

// ---------------------------------------------------------------------------
// 8. Singleton
// ---------------------------------------------------------------------------
Section("8. Singleton - LibraryConfiguration");

var config1 = LibraryConfiguration.Instance;
var config2 = LibraryConfiguration.Instance;
Console.WriteLine($"ReferenceEquals(config1, config2) = {ReferenceEquals(config1, config2)}");

config1.LibraryName = "Alexandria Library";
Console.WriteLine($"Changed through config1, read through config2: {config2.LibraryName}");

// many threads asking at the same moment
var hashes = new ConcurrentBag<int>();
Parallel.For(0, 50, _ => hashes.Add(LibraryConfiguration.Instance.GetHashCode()));
Console.WriteLine($"50 parallel requests -> distinct instances: {hashes.Distinct().Count()}");

// using the configuration values with the static utilities
decimal fee = LibraryUtilities.CalculateLateFee(5, config1.LateFeePerDay);
Console.WriteLine($"{config1.LibraryName}: 5 days late x {config1.LateFeePerDay} = {fee}");
