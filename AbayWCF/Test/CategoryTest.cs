using System;
using Controller;
using Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test
{
    [TestClass]
    public class CategoryTest
    {
        private CategoryController categoryCtrl;

        [TestInitialize]
        public void TestInitialize()
        {
            categoryCtrl = new CategoryController();
        }

        [DataTestMethod]
        [DataRow(1)]
        public void GetCategoryById_ExpectedScenario(int categoryId)
        {
            //Arrange
            ItemCategory category;

            //Act
            category = categoryCtrl.GetItemCategory(categoryId);

            //Assert
            Assert.AreEqual(category.Id, categoryId, "The category is different!");
        }
    }
}
