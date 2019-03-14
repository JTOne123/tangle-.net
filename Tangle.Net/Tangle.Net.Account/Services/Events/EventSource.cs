namespace Tangle.Net.Account.Services
{
  using System;

  public static class EventSource
  {
    public static event EventHandler AttachingToTangle;

    public static event EventHandler DoingInputSelection;

    public static event EventHandler GettingTransactionsToApprove;

    public static event EventHandler PrepareTransfer;

    public static event EventHandler SentTransfer;

    public static event EventHandler BundleConfirmed;

    public enum EvenType
    {
      PrepareTransfer,

      GettingTransactionsToApprove,

      AttachingToTangle,

      SentTransfer,

      DoingInputSelection,

      BundleConfirmed
    }

    public static void Invoke(EvenType type, object sender, EventArgs args)
    {
      switch (type)
      {
        case EvenType.PrepareTransfer:
          PrepareTransfer?.Invoke(sender, args);
          break;
        case EvenType.GettingTransactionsToApprove:
          GettingTransactionsToApprove?.Invoke(sender, args);
          break;
        case EvenType.AttachingToTangle:
          AttachingToTangle?.Invoke(sender, args);
          break;
        case EvenType.SentTransfer:
          SentTransfer?.Invoke(sender, args);
          break;
        case EvenType.DoingInputSelection:
          DoingInputSelection?.Invoke(sender, args);
          break;
        case EvenType.BundleConfirmed:
          BundleConfirmed?.Invoke(sender, args);
          break;
      }
    }
  }
}