namespace Tangle.Net.Account.Services.Plugins
{
  public class BundlePromoter : IAccountPlugin
  {
    /// <inheritdoc />
    public string Name => "BundlePromoter";

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