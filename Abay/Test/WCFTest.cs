using Database;
using System.Collections.Generic;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Test
{
    [TestClass]
    public class WCFTest
    {
        private UserController userCtrl = new UserController();
        private ItemController itemCtrl = new ItemController();
        private BidController bidCtrl = new BidController();
        private User testUser;

        [TestInitialize]
        public void TestInitialize()
        {
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
        }
        [TestCleanup]
        public void TestClean()
        {
            string tableName = "User";
            string columnName = "username";

            using (SqlConnection connection = DBConnection.GetConnection())
            {
                using (SqlCommand cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "DELETE " +
                                      $"FROM {tableName} " +
                                      $"WHERE {columnName} LIKE 'test%';";

                    cmd.ExecuteNonQuery();
                }
            }
        }

        #region User Test

        [TestMethod]
        public void UserLogin_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");

            Assert.IsNotNull(user.LoginToken, "User could not login no token was generated!");
        }
        [TestMethod]
        public void UserLogin_AnoutherizedUser_ReturnFalse()
        {
            User user = userCtrl.Login("RandomUserThatDoesNotExist", "Password");

            Assert.IsNull(user.LoginToken, "User shouldn't exist in the database!");
        }
        [TestMethod]
        public void GetUserInformation_ExpectedScenario()
        {
            User user = userCtrl.GetUserInformation("TestUser");

            Assert.IsNotNull(user, "There is no such user!");
            Assert.AreEqual(user.FirstName, testUser.FirstName, "It's not the right user!");
        }
        [TestMethod]
        public void CreateUser_ExpectedScenario()
        {
            User user = new User
            {
                UserName = "Test123456",
                FirstName = "Test",
                LastName = "Test",
                Password = "123qweasd",
                Email = "test@test.com",
                Admin = false
            };

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, 1, "The user wasn't created!");
        }
        [TestMethod]
        public void CreateUser_UsernameAlreadyUsed()
        {
            User user = new User
            {
                UserName = "a"
            };

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, -3, "A user should already exist with the username 'a'!");
        }
        [TestMethod]
        public void CreateUser_PasswordTooShort()
        {
            User user = new User
            {
                UserName = "Asa123456",
                Password = ""
            };

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, -2, "The password should be too short!");
        }
        [TestMethod]
        public void CreateUser_UsernameTooShort()
        {
            User user = new User
            {
                UserName = ""
            };

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, -1, "The username should be too short!");
        }
        [TestMethod]
        public void CreateUser_Error()
        {
            User user = new User();

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, 0, "The user shouldn't have been created!");
        }
        #endregion

        #region Item Test

        [TestMethod]
        public void CreateItem_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1,  user.LoginToken.SecureToken, 3);
            Assert.IsTrue(itemId >= 0, "Item was not created!");

            Item item = itemCtrl.GetItemById(itemId);
            Assert.IsNotNull(item, "Item was not found!");
        }
        [TestMethod]
        public void CreateItem_WrongCategory_Error()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");

            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, -1, user.LoginToken.SecureToken, 3);
            Assert.AreEqual(itemId, -1, "Item was created!");
        }
        [TestMethod]
        public void CreateItem_WrongToken_Error()
        {
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, "", 3);
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
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3);
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
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3);
            Item item = itemCtrl.GetItemById(itemId);

            bool success = itemCtrl.UpdateItem(itemId, user.LoginToken.SecureToken, "TestItem_Updated", "", -1);
            Assert.IsFalse(success, "The update was successful!");
        }
        [TestMethod]
        public void DeleteItem_ExpectedScenario()
        {
            User user = userCtrl.Login("TestUser", "TestPassword");
            int itemId = itemCtrl.CreateItem("TestItem", "This is a test item", 10, 1, user.LoginToken.SecureToken, 3);
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

        #endregion

        #region Bid Test

        [TestMethod]
        public void Bid_ExpectedScenario()
        {
            User bidder = new User
            {
                UserName = "TestBidderUser",
                FirstName = "TestBidderFirstName",
                LastName = "TestBidderLastName",
                Password = "TestBidderPassword",
                Email = "TestBidderEmail@gmail.com",
                Admin = false
            };
            userCtrl.CreateUser(bidder);

            bidder = userCtrl.Login("TestBidderUser", "TestBidderPassword");

            List<Item> result = itemCtrl.SearchItems("TestItem", -1);

            int itemId = result[0].Id;
            bool success = bidCtrl.Bid(itemId, 40, bidder.LoginToken.SecureToken);
            Assert.IsTrue(success, "Bid was not placed!");
        }
        [TestMethod]
        public void Bid_SellerBidderIsTheSame_ReturnsFalse()
        {
            List<Item> result = itemCtrl.SearchItems("TestItem", -1);
            User user = userCtrl.Login("TestUser", "TestPassword");

            int itemId = result[0].Id;
            bool success = bidCtrl.Bid(itemId, 40, user.LoginToken.SecureToken);

            Assert.IsFalse(success, "The seller and the bidder is the same user!");
        }
        
        #endregion
    }
}
