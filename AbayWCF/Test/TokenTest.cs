using System;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class TokenTest
    {
        private TokenController tokenCtrl;
        private UserController userCtrl;
        private User testUser;

        [TestInitialize]
        public void TestInitialize()
        {
            tokenCtrl = new TokenController();
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
        }
        [TestCleanup]
        public void TestClean()
        {
            TestHelper.DeleteRows("User", "username");
        }

        [TestMethod]
        public void GetUserByToken_ExpectedScenario()
        {
            //Arrange
            string token = testUser.LoginToken.SecureToken;
            User user = tokenCtrl.GetUserByToken(token);

            //Act
            bool success = user.Equals(testUser);

            //Assert
            Assert.IsTrue(success, "The token is not the user's.");
        }
        [TestMethod]
        public void GetUserByToken_InvalidToken_ReturnsNull()
        {
            //Arrange
            string token = "INVALID_TOKEN";

            //Act
            User user = tokenCtrl.GetUserByToken(token);

            //Assert
            Assert.IsNull(user, "The invalid token is the user's.");
        }
        [TestMethod]
        public void CheckTokenTime_ExpectedScenario()
        {
            //Arrange
            string token = testUser.LoginToken.SecureToken;

            //Act
            bool success = tokenCtrl.CheckTokenTime(token);

            //Assert
            Assert.IsTrue(success, "The token has expired!");
        }
    }
}
