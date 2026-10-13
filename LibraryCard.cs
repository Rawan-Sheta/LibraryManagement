namespace LibraryManagement;

// Part 5: sealed class -> nobody can inherit from it.
public sealed class LibraryCard
{
    public string CardNumber { get; }
    public string MemberName { get; }
    public DateTime ExpirationDate { get; }

    public LibraryCard(string cardNumber, string memberName, DateTime expirationDate)
    {
        CardNumber = cardNumber;
        MemberName = memberName;
        ExpirationDate = expirationDate;
    }

    public bool IsExpired => ExpirationDate < DateTime.Today;

    public void DisplayCard()
    {
        Console.WriteLine($"Card #{CardNumber} | Member: {MemberName} | " +
                          $"Expires: {ExpirationDate:yyyy-MM-dd} | Expired: {IsExpired}");
    }
}

// This would NOT compile (error CS0509: cannot derive from sealed type 'LibraryCard'):
//
//   public class PremiumLibraryCard : LibraryCard
//   {
//   }
//
// 'sealed' tells the compiler this class is final: no subclass may exist.
