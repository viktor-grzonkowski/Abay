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

            //TestUser
            testUser = new User
            {
                UserName = "_Test__User",
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Password = "TestPassword",
                Email = "TestEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(testUser);

            testUser = userCtrl.Login(testUser.UserName, testUser.Password);

            //TestItem
            int itemId = itemCtrl.CreateItem(
                "_Test__Item",
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
            //Arrange
            string token = testUser.LoginToken.SecureToken;
            int itemId = itemCtrl.CreateItem("_Test__Item", "This is a test item", 10, 1, token, 3, "");

            //Act
            Item item = itemCtrl.GetItemById(itemId);

            //Assert
            Assert.IsNotNull(item, "Item was not created!");
        }
        [TestMethod]
        public void CreateItem_WrongCategory_Error()
        {
            //Arrange
            string token = testUser.LoginToken.SecureToken;
            int itemId = itemCtrl.CreateItem("_Test__Item", "This is a test item", 10, -1, token, 3, "");

            //Act
            Item item = itemCtrl.GetItemById(itemId);

            //Assert
            Assert.IsNull(item, "Item was created!");
        }
        [TestMethod]
        public void CreateItem_WrongToken_Error()
        {
            //Arrange
            string token = "INVALID_TOKEN";
            int itemId = itemCtrl.CreateItem("_Test__Item", "This is a test item", 10, 1, token, 3, "");

            //Act
            Item item = itemCtrl.GetItemById(itemId);

            //Assert
            Assert.IsNull(item, "Item was created!");
        }
        [TestMethod]
        public void SearchForItem_ExpectedScenario()
        {
            //Arrange
            List<Item> searchResult = itemCtrl.SearchItems(testItem.Name, -1);

            //Act
            Item item = searchResult[0];

            //Assert
            Assert.IsTrue(item.Equals(testItem), "The item found is different!");
        }
        [TestMethod]
        public void UpdateItem_ExpectedScenario()
        {
            //Arrange
            string newName = "_Test__Item_Updated";
            string token = testUser.LoginToken.SecureToken;

            //Act
            testItem.Name = newName;
            bool success = itemCtrl.UpdateItem(testItem.Id, token, testItem.Name, testItem.Description, testItem.Category.Id);
            
            //Assert
            Assert.IsTrue(success, "The item was not updated!");

            //Act
            Item item = itemCtrl.GetItemById(testItem.Id);

            //Assert
            Assert.IsTrue(item.Equals(testItem), "The name of the Item was not updated!");
        }
        [TestMethod]
        public void UpdateItem_WrongCategory_Error()
        {
            //Arrange
            string token = testUser.LoginToken.SecureToken;

            //Act
            bool success = itemCtrl.UpdateItem(testItem.Id, token, testItem.Name, testItem.Description, -1);

            //Assert
            Assert.IsFalse(success, "The item was updated!");
        }
        [TestMethod]
        public void DeleteItem_ExpectedScenario()
        {
            //Arrange
            string token = testUser.LoginToken.SecureToken;

            //Act
            bool success = itemCtrl.DeleteItem(testItem.Id, token);

            //Assert
            Assert.IsTrue(success, "The Item was deleted!");
        }
        [DataTestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(4)]
        public void GetAllItemsByCategory_ExpectedScenario(int categoryId)
        {
            //Arrange
            List<Item> items = itemCtrl.GetAllActiveItemsByCategory(categoryId);

            //Act
            bool success = true;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].Category.Id != categoryId)
                {
                    success = false;
                    break;
                }
            }

            //Assert
            Assert.IsTrue(success, "Not all items were associated with the Category!");
        }
    }
}
