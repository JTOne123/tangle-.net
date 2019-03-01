namespace Tangle.Net.Account.Tests.Services
{
  using System;
  using System.Collections.Generic;
  using System.Runtime.Remoting.Channels;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Account.Services;
  using Tangle.Net.Entity;

  [TestClass]
  public class AccountTest
  {
    [TestMethod]
    public void TestDepositRequestAllocation()
    {
      var accountSettings = AccountSettings.GetDefault(Seed.Random(), new InMemoryIotaRepository());
      var account = new Account("123456789", accountSettings, new AddressGeneratorStub());

      var condition = account.AllocateDepositRequest(new DepositRequest(DateTime.UtcNow, 10, true));
      var state = accountSettings.Store.LoadAccount("123456789");

      // Fresh accounts use index 1 for the first deposit
      Assert.AreEqual(1, condition.Address.KeyIndex);
      Assert.AreEqual(1, state.LastUsedKeyIndex);
      Assert.AreEqual(1, state.DepositRequests.Count);
      Assert.AreEqual(10, state.DepositRequests[0].ExpectedAmount);
      Assert.AreEqual(2, state.DepositRequests[0].SecurityLevel);
    }

    [TestMethod]
    public void TestSendPopulatesStoreAndEmitsEvents()
    {
      var accountSettings = AccountSettings.GetDefault(Seed.Random(), new InMemoryIotaRepository());
      accountSettings.InputSelector = new InputSelectorStub();

      // Initialize account and set index to input selection max index
      accountSettings.Store.LoadAccount("123456789");
      accountSettings.Store.WriteIndex("123456789", 2);

      var attachToTangleEmitted = false;
      var doingInputSelectionEmitted = false;
      var gettingTransactionsToApproveEmitted = false;
      var sentTransferEmitted = false;
      var prepareTransferEmitted = false;

      var account = new Account("123456789", accountSettings, new AddressGeneratorStub());
      EventSource.AttachingToTangle += (sender, args) => { attachToTangleEmitted = true; };
      EventSource.DoingInputSelection += (sender, args) => { doingInputSelectionEmitted = true; };
      EventSource.GettingTransactionsToApprove += (sender, args) => { gettingTransactionsToApproveEmitted = true; };
      EventSource.SentTransfer += (sender, args) => { sentTransferEmitted = true; };
      EventSource.PrepareTransfer += (sender, args) => { prepareTransferEmitted = true; };

      var bundle = account.Send(new List<Transfer> { new Transfer { Address = new Address(Seed.Random().Value), ValueToTransfer = 111 } });
      var state = accountSettings.Store.LoadAccount("123456789");

      Assert.AreEqual(1, state.PendingTransfers.Count);
      Assert.AreEqual(1, state.DepositRequests.Count);
      Assert.AreEqual(111, state.DepositRequests[0].ExpectedAmount);
      Assert.AreEqual(bundle.TailTransaction.Hash.Value, state.PendingTransfers[0].Tail.Value);

      Assert.IsTrue(attachToTangleEmitted);
      Assert.IsTrue(doingInputSelectionEmitted);
      Assert.IsTrue(gettingTransactionsToApproveEmitted);
      Assert.IsTrue(sentTransferEmitted);
      Assert.IsTrue(prepareTransferEmitted);
    }
  }
}