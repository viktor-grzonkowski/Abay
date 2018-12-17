using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Test
{
    [TestClass]
    public class BidTest
    {
        private UserController userCtrl;
        private ItemController itemCtrl;
        private BidController bidCtrl;
        private User testSeller;
        private User testBidder;
        private Item testItem;

        [TestInitialize]
        public void TestInitialize()
        {
            userCtrl = new UserController();
            itemCtrl = new ItemController();
            bidCtrl = new BidController();

            //TestSeller
            testSeller = new User
            {
                UserName = "_Test__Seller",
                FirstName = "FirstName",
                LastName = "LastName",
                Password = "TestPassword",
                Email = "TestSellerEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(testSeller);

            //TestBidder
            testBidder = new User
            {
                UserName = "_Test__Bidder",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(testBidder);

            //TestItem
            testSeller = userCtrl.Login(testSeller.UserName, testSeller.Password);
            int itemId = itemCtrl.CreateItem(
                "_Test__Item",
                "This is a test item",
                10,
                1,
                testSeller.LoginToken.SecureToken,
                3,
                "");
            testItem = itemCtrl.GetItemById(itemId);

            testSeller.Password = "TestPassword";
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
            TestHelper.DeleteRows("Item", "name");
            TestHelper.DeleteRows("Bid", "buyerName");
        }

        [TestMethod]
        public void Bid_ExpectedScenario()
        {
            //Arrange
            testBidder = userCtrl.Login(testBidder.UserName, testBidder.Password);

            double amount = testItem.InitialPrice + 1;
            string token = testBidder.LoginToken.SecureToken;

            //Act
            bool success = bidCtrl.Bid(testItem.Id, amount, token);

            //Assert
            Assert.IsTrue(success, "Bid was not placed!");
        }
        [TestMethod]
        public void Bid_OverHighestBid_ExpectedScenario()
        {
            //Arrange
            testBidder = userCtrl.Login(testBidder.UserName, testBidder.Password);

            double amount = testItem.InitialPrice + 1;
            string token = testBidder.LoginToken.SecureToken;

            //Making the first, only and highest bid for the testItem
            bool success = bidCtrl.Bid(testItem.Id, amount, token);

            Assert.IsTrue(success, "The first bid was not placed!");

            //Refreshing information (winning bid) on the testItem
            testItem = itemCtrl.GetItemById(testItem.Id);

            //Bid under the highest bid
            amount = testItem.WinningBid.Amount + 1;

            //Act
            success = bidCtrl.Bid(testItem.Id, amount, token);

            //Assert
            Assert.IsTrue(success, "Bid was not placed!");
        }
        [TestMethod]
        public void Bid_SellerBidderIsTheSame_ReturnsFalse()
        {
            //Arrange
            testSeller = userCtrl.Login(testSeller.UserName, testSeller.Password);

            string token = testSeller.LoginToken.SecureToken;

            //Act
            bool success = bidCtrl.Bid(testItem.Id, 40, token);

            //Assert
            Assert.IsFalse(success, "Despite the seller and bidder are the same, the bid was placed!");
        }
        [TestMethod]
        public void Bid_AmountLowerThanInitialPrice_ReturnsFalse()
        {
            //Arrange
            testBidder = userCtrl.Login(testBidder.UserName, testBidder.Password);

            double amount = testItem.InitialPrice - 1;
            string token = testBidder.LoginToken.SecureToken;

            //Act
            bool success = bidCtrl.Bid(testItem.Id, amount, token);

            //Assert
            Assert.IsFalse(success, "Despite the amount was lower then the initial price, the bid was placed!");
        }
        [TestMethod]
        public void Bid_AmountLowerThanHighestBid_ReturnsFalse()
        {
            //Arrange
            testBidder = userCtrl.Login(testBidder.UserName, testBidder.Password);

            double amount = testItem.InitialPrice + 2;
            string token = testBidder.LoginToken.SecureToken;

            //Making the first, only and highest bid for the testItem
            bidCtrl.Bid(testItem.Id, amount, token);

            //Refreshing information (winning bid) on the testItem
            testItem = itemCtrl.GetItemById(testItem.Id);

            //Bid under the highest bid
            amount = testItem.WinningBid.Amount - 1;

            //Act
            bool success = bidCtrl.Bid(testItem.Id, amount, token);

            //Assert
            Assert.IsFalse(success, "Despite the amount was lower then highest bid, the bid was placed!");
        }
        [TestMethod]
        public void Bid_AfterEndDate_ReturnsFalse()
        {
            //Arrange
            int itemId = itemCtrl.CreateItem("_Test__Item", "This is a test item",
                10, 1, testSeller.LoginToken.SecureToken,
                0, //The duration is set to 0, so it will isntantly expire
                "");
            testItem = itemCtrl.GetItemById(itemId);

            testBidder = userCtrl.Login(testBidder.UserName, testBidder.Password);

            double amount = testItem.InitialPrice + 1;
            string token = testBidder.LoginToken.SecureToken;

            //Act
            bool success = bidCtrl.Bid(testItem.Id, amount, token);

            //Assert
            Assert.IsFalse(success, "Despite the bid was made after the expiration date, the bid was placed!");
        }
        [DataTestMethod]
        [DataRow(5)]
        [DataRow(8)]
        public void GetAllBidsByItem_ExpectedResults(int itemId)
        {
            //Arrange
            List<Bid> bids = bidCtrl.GetAllBidsByItem(itemId);

            //Act
            bool success = true;
            for (int i = 0; i < bids.Count; i++)
            {
                if (bids[i].ItemId != itemId)
                {
                    success = false;
                    break;
                }
            }

            //Assert
            Assert.IsTrue(success, "Not all bids were associated with the Item!");
        }
    }
}
