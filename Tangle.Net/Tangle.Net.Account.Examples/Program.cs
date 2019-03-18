namespace Tangle.Net.Account.Examples
{
  using System;

  internal class Program
  {
    internal static void Main(string[] args)
    {
      var example = new AccountFlow();
      example.Execute();
      Console.ReadKey();
    }
  }
}