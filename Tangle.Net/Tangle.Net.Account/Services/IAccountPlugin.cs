namespace Tangle.Net.Account.Services
{
  public interface IAccountPlugin
  {
    string Name { get; }

    void Shutdown();

    void Start(IAccount account);
  }
}