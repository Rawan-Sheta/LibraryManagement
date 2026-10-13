namespace LibraryManagement;

public class Magazine : LibraryItem
{
    public int IssueNumber { get; set; }

    public Magazine(int id, string title, int year, int issueNumber)
        : base(id, title, year)
    {
        IssueNumber = issueNumber;
    }

    public override void DisplayInfo()
    {
        Console.WriteLine($"[Magazine] \"{Title}\" - Issue #{IssueNumber} ({Year})");
    }
}
