using Database;
using System.Collections.Generic;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Test
{
    [TestClass]
    public class BidTest
    {
        private UserController userCtrl;
        private ItemController itemCtrl;
        private BidController bidCtrl;
        private User testSeller;
        private Item testItem;

        [TestInitialize]
        public void TestInitialize()
        {
            userCtrl = new UserController();
            itemCtrl = new ItemController();
            bidCtrl = new BidController();

            userCtrl = new UserController();
            itemCtrl = new ItemController();

            testSeller = new User
            {
                UserName = "TestSeller",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestSellerEmail@gmail.com",
                Admin = false
            };

            userCtrl.CreateUser(testSeller);
            testSeller = userCtrl.Login("TestSeller", "TestPassword");

            int itemId = itemCtrl.CreateItem(
                "TestItem",
                "This is a test item",
                10,
                1,
                testSeller.LoginToken.SecureToken,
                3,
                "");
            testItem = itemCtrl.GetItemById(itemId);
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
            TestHelper.DeleteRows("Item", "name");
            TestHelper.DeleteRows("Bids", "buyerName");
        }

        [TestMethod]
        public void Bid_ExpectedScenario()
        {
            User bidder = new User
            {
                UserName = "TestBidder",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(bidder);

            bidder = userCtrl.Login("TestBidder", "TestPassword");

            bool success = bidCtrl.Bid(testItem.Id, 40, bidder.LoginToken.SecureToken);
            Assert.IsTrue(success, "Bid was not placed!");
        }
        [TestMethod]
        public void Bid_SellerBidderIsTheSame_ReturnsFalse()
        {
            bool success = bidCtrl.Bid(testItem.Id, 40, testSeller.LoginToken.SecureToken);

            Assert.IsFalse(success, "The seller and the bidder is the same user!");
        }
        [TestMethod]
        public void Bid_AmountLowerThanInitialPrice_ReturnsFalse()
        {
            User bidder = new User
            {
                UserName = "TestBidder",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(bidder);

            bidder = userCtrl.Login("TestBidder", "TestPassword");

            bool success = bidCtrl.Bid(testItem.Id, testItem.InitialPrice-1, bidder.LoginToken.SecureToken);

            Assert.IsFalse(success, "The bid amount is lower than the initialPrice!");
        }
        [TestMethod]
        public void Bid_AmountLowerThanHighestBid_ReturnsFalse()
        {
            User bidder = new User
            {
                UserName = "TestBidder",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(bidder);

            bidder = userCtrl.Login("TestBidder", "TestPassword");

            bidCtrl.Bid(testItem.Id, testItem.InitialPrice + 2, bidder.LoginToken.SecureToken);
            testItem = itemCtrl.GetItemById(testItem.Id);

            bool success = bidCtrl.Bid(testItem.Id, testItem.WinningBid.Amount - 1, bidder.LoginToken.SecureToken);

            Assert.IsFalse(success, "The bid amount is lower than the highest bid!");
        }
        [TestMethod]
        public void Bid_AfterEndDate_ReturnsFalse()
        {
            int itemId = itemCtrl.CreateItem(
                "TestItem",
                "This is a test item",
                10,
                1,
                testSeller.LoginToken.SecureToken,
                0,
                "");
            testItem = itemCtrl.GetItemById(itemId);

            User bidder = new User
            {
                UserName = "TestBidder",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(bidder);

            bidder = userCtrl.Login("TestBidder", "TestPassword");

            bool success = bidCtrl.Bid(testItem.Id, testItem.InitialPrice + 1, bidder.LoginToken.SecureToken);

            Assert.IsFalse(success, "The bid is not expired!");
        }
    }
}
