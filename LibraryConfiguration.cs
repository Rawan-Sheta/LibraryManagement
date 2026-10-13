namespace LibraryManagement;

// Part 8: thread-safe Singleton.
public sealed class LibraryConfiguration
{
    // Lazy<T> with ExecutionAndPublication: the factory lambda runs ONCE, even if many threads
    // ask for the value at the same time (Lazy uses a lock internally; the other threads wait,
    // then all receive the same object). The instance is created only when first needed.
    private static readonly Lazy<LibraryConfiguration> _lazyInstance =
        new Lazy<LibraryConfiguration>(
            () => new LibraryConfiguration(),
            LazyThreadSafetyMode.ExecutionAndPublication);

    // Private constructor: no code outside this class can write 'new LibraryConfiguration()'.
    private LibraryConfiguration()
    {
        LibraryName = "City Central Library";
        MaximumBorrowDays = 14;
        LateFeePerDay = 2.5m;
    }

    // The single, globally accessible instance.
    public static LibraryConfiguration Instance => _lazyInstance.Value;

    public string LibraryName { get; set; }
    public int MaximumBorrowDays { get; set; }
    public decimal LateFeePerDay { get; set; }
}
