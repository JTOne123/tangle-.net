namespace Tangle.Net.Account.Services.Memory
{
  using Tangle.Net.Entity;

  public class InMemorySeedProvider : ISeedProvider
  {
    public InMemorySeedProvider(Seed seed)
    {
      this.Seed = seed;
    }

    /// <inheritdoc />
    public Seed Seed { get; }
  }
}