namespace Tangle.Net.Account.Services
{
  using System;

  public class UtcTimeSource : ITimeSource
  {
    /// <inheritdoc />
    public DateTime Time => DateTime.UtcNow;
  }
}