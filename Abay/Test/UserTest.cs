using System;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class UserTest
    {
        private UserController userCtrl;
        private User testUser;

        [TestInitialize]
        public void TestInitialize()
        {
            userCtrl = new UserController();

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
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
        }

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

            Assert.IsNull(user, "User shouldn't exist in the database!");
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
                UserName = testUser.UserName,
                Password = "TestPassword"
            };

            int success = userCtrl.CreateUser(user);

            Assert.AreEqual(success, -3, "The user should already exist!");
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
    }
}
