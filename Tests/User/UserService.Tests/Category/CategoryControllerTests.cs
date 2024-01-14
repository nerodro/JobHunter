using CategoryAPI.Controllers;
using CategoryAPI.ViewModel;
using CategoryDomain.Model;
using CategoryService.CategoryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ocelot.Responses;
using UserAPI.Controllers;
using UserDomain.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
        [TestMethod]
        public async Task Test_Delete_Category()
        {
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            var update = await _category.DeleteCategory(1);
            var result = update.Result as OkObjectResult;
            Assert.AreEqual(result.StatusCode, 200);
        }
        [TestMethod]
        public async Task Test_Delete_Incorrect_Category()
        {
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            var update = await _category.DeleteCategory(123);
            var result = update.Result as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
        [TestMethod]
        public async Task Test_Get_Category()
        {
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            var get = await _category.SingleCategory(1);
            var item = get.Value as CategoryViewModel;
            if (item != null)
            {
                Assert.AreEqual("Test", item.CategoryName);
            };
        }
        [TestMethod]
        public async Task Test_Get_Incorrecrt_Category()
        {
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            var get = await _category.SingleCategory(123);
            var result = get.Result as BadRequestObjectResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
        [TestMethod]
        public async Task Test_Get_All_Category()
        {
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 1, CategoryName = "Test" });
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 2, CategoryName = "Test2" });
            _categoryMock.Setup(x => x.GetCategory(1)).ReturnsAsync(new CategoryModel { Id = 3, CategoryName = "Test3" });
            var res = _category.Index() as List<CategoryViewModel>;
            Assert.IsNotNull(res);
        }
    }
}
