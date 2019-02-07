namespace Tangle.Net.Account.Entity
{
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using Tangle.Net.Entity;

  public class InputSelection
  {
    public InputSelection(List<Address> inputAddresses)
    {
      if (inputAddresses.Any(i => i.KeyIndex == 0))
      {
        throw new ArgumentException("Invalid key index '0' detected");
      }

      this.InputAddresses = inputAddresses;
    }

    public List<Address> InputAddresses { get; set; }

    public long InputValueSum => this.InputAddresses.Sum(i => i.Balance);

    public List<int> KeyIndices => this.InputAddresses.Select(i => i.KeyIndex).ToList();
  }
}