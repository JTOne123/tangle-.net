namespace Tangle.Net.Account.Services.Plugins
{
  public class TransactionPoller : IAccountPlugin
  {
    /// <inheritdoc />
    public string Name => "TransactionPoller";

    /// <inheritdoc />
    public void Shutdown()
    {
    }

    /// <inheritdoc />
    public void Start(IAccount account)
    {
    }
  }
}