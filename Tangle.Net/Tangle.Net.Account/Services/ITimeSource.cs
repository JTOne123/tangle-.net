namespace Tangle.Net.Account.Services
{
  using System;

  public interface ITimeSource
  {
    DateTime Time { get; }
  }
}