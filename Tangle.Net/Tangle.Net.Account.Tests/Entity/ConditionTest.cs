namespace Tangle.Net.Account.Tests.Entity
{
  using System;

  using Microsoft.VisualStudio.TestTools.UnitTesting;

  using Tangle.Net.Account.Entity;
  using Tangle.Net.Entity;

  [TestClass]
  public class ConditionTest
  {
    [TestMethod]
    public void TestMagnetLinkCreation()
    {
      var condition = new Condition(
                        new Address("SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUYFZOTSAS9C"),
                        new DateTime(2019, 12, 12, 12, 12, 12)) { ExpectedAmount = 10, };

      Assert.AreEqual(
        "iota://SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUYFZOTSAS9C/?t=1576152732&m=false&am=10",
        condition.ToMagnetLink());
    }

    [TestMethod]
    public void TestMagnetLinkParsing()
    {
      var condition = Condition.FromMagnetLink(
        new Uri("iota://SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUYFZOTSAS9C/?t=1576152732&m=false&am=10"));

      Assert.AreEqual("SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUYFZOTSAS9C", condition.Address.Value + condition.Address.Checksum.Value);
      Assert.AreEqual(new DateTime(2019, 12, 12, 12, 12, 12), condition.TimeoutAt);
      Assert.IsFalse(condition.MultiUse);
      Assert.AreEqual(10, condition.ExpectedAmount);
    }

    [DataTestMethod]
    [DataRow("http://SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUYFZOTSAS9C/?t=1576152732&m=false&am=10")]
    [DataRow("iota://SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUY/?t=1576152732&m=false&am=10")]
    [DataRow("iota://SSFQNEKDAMMAJSTMLRPIHSKZMHQTMYITPPLUWLOPYKS9K9YDGJZKTNQHJVD9YGZFOVZKAZHDIDMFWJGUY/")]
    [ExpectedException(typeof(Exception))]
    public void TestInvalidMagnetLinkParsing(string magnetLink)
    {
      var condition = Condition.FromMagnetLink(new Uri(magnetLink));
    }
  }
}