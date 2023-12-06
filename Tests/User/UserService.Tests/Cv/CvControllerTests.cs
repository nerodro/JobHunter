using Grpc.Net.Testing.Moq.Naming;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UserAPI.Controllers;
using UserAPI.ServiceGrpc;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.CvService;
using UserService.LanguageService;
using UserService.UserService;

namespace UserService.Tests.Cv
{
    [TestClass]
    public class CvControllerTests
    {
        Mock<ICvService> _cvService;
        Mock<ILanguageService> _languageService;
        Mock<IUserService> _userService;
        Mock<CategoryRpc> _rpc;
        CvController _controller;
        public CvControllerTests()
        {
            _cvService = new Mock<ICvService>();
            _languageService = new Mock<ILanguageService>();
            _rpc = new Mock<CategoryRpc>();
            _userService = new Mock<IUserService>();
            _controller = new CvController(_cvService.Object, _languageService.Object, _userService.Object, _rpc.Object);
        }
        [TestMethod]
        public async Task TestAdd_Cv()
        {
            _languageService.Setup(x => x.GetLanguage(1)).ReturnsAsync(new LanguageModel { Id = 1, Language = "Roma" });
            _userService.Setup(x => x.GetUser(1)).ReturnsAsync(new UserModel { Id = 1, Name = "Tom" });
            CategoryViewModel categoryView = new CategoryViewModel()
            {
                Id = 1,
                CategoryName = "Test"
            };
            //var mockClient = new Mock<CategoryRpc>();
            // mockClient.Setup(c => c.GetCategoryById(1)).Returns((int)categoryView.Id);
            CvViewModel _viewModel = new CvViewModel
            {
                Id = 1,
                CategoryId = 1,
                AboutMe = "Test",
                JobNmae = "TestJob",
                LanguageId = 1,
                UserId = 1,
            };
            var response = await _controller.CreateCv(_viewModel) as ObjectResult;
            var item = response.Value as CvViewModel;
            if (item != null)
            {
                Assert.AreEqual(_viewModel.JobNmae, item.JobNmae);
                Assert.AreEqual(_viewModel.AboutMe, item.AboutMe);
                Assert.AreEqual(_viewModel.CategoryId, item.CategoryId);
                Assert.AreEqual(_viewModel.UserId, item.UserId);
                Assert.AreEqual(_viewModel.LanguageId, item.LanguageId);
            }
        }
    }
}
