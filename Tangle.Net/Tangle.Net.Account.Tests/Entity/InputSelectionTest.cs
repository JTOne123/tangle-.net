namespace Tangle.Net.Account.Tests.Entity
{
  using System;
  using System.Collections.Generic;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  [TestClass]
  public class InputSelectionTest
  {
    [TestMethod]
    public void TestPropertyAggregation()
    {
      var selection = new InputSelection(
        new List<Address>
          {
            new Address(Hash.Empty.Value) { Balance = 111, KeyIndex = 1 }, new Address(Hash.Empty.Value) { Balance = 111, KeyIndex = 2 }
          });

      Assert.AreEqual(222, selection.InputValueSum);
      Assert.AreEqual(2, selection.KeyIndices.Count);
      Assert.AreEqual(1, selection.KeyIndices[0]);
      Assert.AreEqual(2, selection.KeyIndices[1]);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TestAddressKeyIndexIsMissingShouldThrowException()
    {
      var selection = new InputSelection(
        new List<Address>
          {
            new Address(Hash.Empty.Value) { Balance = 111 }
          });
    }
  }
}