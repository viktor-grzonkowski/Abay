using System;
using System.Collections.Generic;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class WCFTest
    {
        private UserController userCtrl = new UserController();
        private ItemController itemCtrl = new ItemController();
        private BidController bidCtrl = new BidController();

        [TestCleanup]
        public void TestClean()
        {
            CleanUp();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            User user = new User
            {
                UserName = "TestUser",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(user);
        }

        //
        // USER TEST
        //

        [TestMethod]
        public void UserLogin()
        {
            Token tk = userCtrl.Login("TestUser", "TestPassword");
            Assert.IsNotNull(tk,"User could not login no token was generated!");
        }

        [TestMethod]
        public void GetUserInformation()
        {
            User getUser = userCtrl.GetUserInformation("TestUser");
            Assert.IsNotNull(getUser, "There is no such user!");
            Assert.IsTrue(string.Equals(getUser.FirstName, "TestFirstName"),"It's not the right user or user was not found!");
        }

        //
        // ITEM TEST
        //

        [TestMethod]
        public void CreateItem()
        {
            Token tk = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem("TestItem", 10, tk.SecureToken, 1, "This is a test item", 3);
            Assert.IsTrue(itemId >= 0,"Item was not created!");
            Item item = itemCtrl.GetItemById(itemId);
            Assert.IsNotNull(item, "Item was not found!");
        }

        [TestMethod]
        public void SearchForItem()
        {
            List<Item> newList = itemCtrl.SearchItems("TestItem", -1);
            Assert.IsTrue(string.Equals(newList[0].Name, "TestItem"),"It's not the same Item!");
        }

        //
        // BID TEST
        //

        [TestMethod]
        public void BidSellerBuyerSame()
        {
            List<Item> newList = itemCtrl.SearchItems("TestItem", -1);
            Token tk = userCtrl.Login("TestUser", "TestPassword");

            Assert.IsFalse(bidCtrl.Bid(newList[0].Id, 40, tk.SecureToken));
        }

        [TestMethod]
        public void BidDifferentBuyer()
        {
            User userTwo = new User
            {
                UserName = "Test2User",
                FirstName = "Test2FirstName",
                LastName = "Test2LastName",
                Password = "Test2Password",
                Email = "Test2Email@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(userTwo);

            Token token = userCtrl.Login("Test2User", "Test2Password");

            List<Item> newList = itemCtrl.SearchItems("TestItem", -1);

            Assert.IsTrue(bidCtrl.Bid(newList[0].Id, 40, token.SecureToken),"Bid wa not placed!");

            Item item = itemCtrl.GetItemById(newList[0].Id);

            Assert.IsTrue(string.Equals(userTwo.UserName,item.BuyerUser.UserName),"It's the wrong buyer!");
        }


        private void CleanUp()
        {
            //TestHelper.DeleteTest("User","username");
        }
    }
}
