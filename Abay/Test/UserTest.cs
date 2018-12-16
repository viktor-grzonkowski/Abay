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
            testUser.Password = "TestPassword";
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
        }

        [TestMethod]
        public void UserLogin_ExpectedScenario()
        {
            //Arrange
            string username = testUser.UserName;
            string password = testUser.Password;

            //Act
            User user = userCtrl.Login(username, password);

            //Assert
            Assert.IsNotNull(user.LoginToken, "User was not logged in!");
        }
        [TestMethod]
        public void UserLogin_AnautherizedUser_ReturnFalse()
        {
            //Arrange
            string username = "AnauthorizedUser";
            string password = "password";

            //Act
            User user = userCtrl.Login(username, password);

            //Assert
            Assert.IsNull(user, "Anautherized user was logged in!");
        }
        [TestMethod]
        public void GetUserInformation_ExpectedScenario()
        {
            //Arrange
            string username = testUser.UserName;
            User user = userCtrl.GetUserInformation(username);

            //Act
            bool success = user.Equals(testUser);

            //Assert
            Assert.IsTrue(success, "The user information is incorrect!");
        }
        [TestMethod]
        public void CreateUser_ExpectedScenario()
        {
            //Arrange
            User user = new User
            {
                UserName = "_Test__123456",
                FirstName = "Test",
                LastName = "Test",
                Password = "123qweasd",
                Email = "test@test.com",
                Admin = false
            };

            //Act
            int success = userCtrl.CreateUser(user);

            //Assert
            Assert.AreEqual(success, 1, "The User was not created!");
        }
        [TestMethod]
        public void CreateUser_UsernameAlreadyUsed()
        {
            //Arrange
            User user = new User
            {
                UserName = testUser.UserName,
                Password = "TestPassword"
            };

            //Act
            int success = userCtrl.CreateUser(user);

            //Assert
            Assert.AreEqual(success, -3, "The username is not used!");
        }
        [TestMethod]
        public void CreateUser_PasswordTooShort()
        {
            //Arrange
            User user = new User
            {
                UserName = "_Test__123456",
                Password = ""
            };

            //Act
            int success = userCtrl.CreateUser(user);

            //Assert
            Assert.AreEqual(success, -2, "The password is long enough!");
        }
        [TestMethod]
        public void CreateUser_UsernameTooShort()
        {
            //Arrange
            User user = new User
            {
                UserName = "",
                Password = "TestPassword"
            };

            //Act
            int success = userCtrl.CreateUser(user);

            //Assert
            Assert.AreEqual(success, -1, "The username is long enough!");
        }
    }
}
