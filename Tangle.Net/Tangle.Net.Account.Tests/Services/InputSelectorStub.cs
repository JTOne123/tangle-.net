namespace Tangle.Net.Account.Tests.Services
{
  using System.Collections.Generic;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Services;
  using Tangle.Net.Cryptography;
  using Tangle.Net.Entity;

  public class InputSelectorStub : IInputSelector
  {
    /// <inheritdoc />
    public InputSelection SelectInputs(IAccount account, long transferValue, bool balanceCheck)
    {
      return new InputSelection(
        new List<Address>
          {
            new Address(Hash.Empty.Value) { Balance = 111, KeyIndex = 1, SecurityLevel = 2, PrivateKey = new PrivateKeyStub() },
            new Address(Hash.Empty.Value) { Balance = 111, KeyIndex = 2, SecurityLevel = 2, PrivateKey = new PrivateKeyStub() }
          });
    }
  }
}