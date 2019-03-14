namespace Tangle.Net.Account.Services.Events
{
  using System;

  using Tangle.Net.Entity;

  public class BundleConfirmedEventArgs : EventArgs
  {
    public BundleConfirmedEventArgs(Bundle bundle)
    {
      this.Bundle = bundle;
    }

    public Bundle Bundle { get; }
  }
}