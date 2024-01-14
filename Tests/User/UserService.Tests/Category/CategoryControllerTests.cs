﻿using CategoryAPI.Controllers;
using CategoryAPI.ViewModel;
using CategoryDomain.Model;
using CategoryService.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserAPI.Controllers;
using UserDomain.Models;

namespace JobJunster.Tests.Category
{
    [TestClass]
    public class CategoryControllerTests
    {
        CategoryController _category;
        Mock<ICategoryService> _categoryMock;
        public CategoryControllerTests()
        {
            _categoryMock = new Mock<ICategoryService>();
            _category = new CategoryController(_categoryMock.Object);
        }
        [TestMethod]
        public async Task Test_Add_Category()
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                CategoryName = "Test",
            };
            var response = await _category.CreateCategory(viewModel) as CreatedAtActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);
            var item = response.Value as CategoryViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CategoryName, item.CategoryName);
            };
        }
        [TestMethod]
        public async Task Test_Add_Category_With_Spaces()
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Id = 1,
                CategoryName = "  Test  ",
            };
            var resquest = await _category.CreateCategory(viewModel) as CreatedAtActionResult;
            var response = await _category.SingleCategory(1);
            var item = response.Value as CategoryViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CategoryName, item.CategoryName);
            };
        }
        [TestMethod]
        public async Task Test_Update_Category()
        {
            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Id = 1,
                CategoryName = "Test2",
            };
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            var update = await _category.EditCategory(1, viewModel) as OkObjectResult;
            var response = await _category.SingleCategory(1);
            var item = response.Value as CategoryViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CategoryName, item.CategoryName);
            };
        }

    }
}
