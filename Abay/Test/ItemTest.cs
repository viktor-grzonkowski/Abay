using System;
using System.Collections.Generic;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class ItemTest
    {
        private UserController userCtrl;
        private ItemController itemCtrl;
        private User testUser;
        private Item testItem;

        [TestInitialize]
        public void TestInitialize()
        {
            userCtrl = new UserController();
            itemCtrl = new ItemController();

            testUser = new User
            {
                UserName = "TestUser",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestEmail@gmail.com",
                Admin = false
            };

            userCtrl.CreateUser(testUser);
            testUser = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem(
                "TestItem",
                "This is a test item",
                10,
                1,
                testUser.LoginToken.SecureToken,
                3,
                "");
            testItem = itemCtrl.GetItemById(itemId);
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
            TestHelper.DeleteRows("Item", "name");
        }

        [TestMethod]
        public void CreateItem_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3, "");
            Assert.IsTrue(itemId >= 0, "Item was not created!");

            Item item = itemCtrl.GetItemById(itemId);
            Assert.IsNotNull(item, "Item was not found!");
        }
        [TestMethod]
        public void CreateItem_WrongCategory_Error()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, -1, user.LoginToken.SecureToken, 3, "");
            Assert.AreEqual(itemId, -1, "Item was created!");
        }
        [TestMethod]
        public void CreateItem_WrongToken_Error()
        {
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, "", 3, "");
            Assert.AreEqual(itemId, -1, "Item was created!");
        }
        [TestMethod]
        public void SearchForItem_ExpectedScenario()
        {
            List<Item> searchResult = itemCtrl.SearchItems("TestItem", -1);

            string itemName = searchResult[0].Name;

            Assert.AreEqual(itemName, "TestItem", "It's not the same Item!");
        }
        [TestMethod]
        public void UpdateItem_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3, "");
            Item item = itemCtrl.GetItemById(itemId);

            bool success = itemCtrl.UpdateItem(itemId, user.LoginToken.SecureToken, "TestItem_Updated", "", 1);
            Assert.IsTrue(success, "The update was unsuccessful!");

            item = itemCtrl.GetItemById(itemId);
            Assert.AreEqual(item.Name, "TestItem_Updated", "The name of the Item was not updated!");
        }
        [TestMethod]
        public void UpdateItem_WrongCategory_Error()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3, "");
            Item item = itemCtrl.GetItemById(itemId);

            bool success = itemCtrl.UpdateItem(itemId, user.LoginToken.SecureToken, "TestItem_Updated", "", -1);
            Assert.IsFalse(success, "The update was successful!");
        }
        [TestMethod]
        public void DeleteItem_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3, "");
            Item item = itemCtrl.GetItemById(itemId);

            bool success = itemCtrl.DeleteItem(itemId, user.LoginToken.SecureToken);
            Assert.IsTrue(success, "The delete was unsuccessful!");
        }
        [TestMethod]
        public void GetAllItemsByCategory_ExpectedScenario()
        {
            bool success = true;
            List<Item> items = itemCtrl.GetAllActiveItemsByCategory(1);
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Category.Id != 1)
                {
                    success = false;
                    break;
                }
            }

            Assert.IsTrue(success, "There was one or more Item which is not in that Category!");
        }
    }
}
