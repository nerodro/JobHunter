using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserAPI.Controllers;
using UserAPI.ViewModel;
using UserDomain.Models;
using UserService.LanguageService;

namespace JobJunster.Tests.Language
{
    [TestClass]
    public class LanguageControllerTests 
    {
        LanguageController _controller;
        //ILanguageService _languageService;
        Mock<ILanguageService> _languageService;
        public LanguageControllerTests()
        {
            //_languageService = new LanguageServiceFake();
            _languageService = new Mock<ILanguageService>();
            _controller = new LanguageController(_languageService.Object);
        }
        [TestMethod]
        public async Task TestAdd_With_Correct_Data()
        {
            LanguageViewModel viewModel = new LanguageViewModel()
            {
                LanguageName = "Spainish",
            };
            var response = await _controller.CreateLanguage(viewModel) as CreatedAtActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);
            var item = response.Value as LanguageViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.LanguageName, item.LanguageName);
            }
        }
        [TestMethod]
        public async Task TestAdd_With_Spaces()
        {
            LanguageViewModel viewModel = new LanguageViewModel()
            {
                LanguageName = "  Spainish  ",
            };
            var response = await _controller.CreateLanguage(viewModel) as CreatedAtActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);
            var item = response.Value as LanguageViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.LanguageName.Trim(), item.LanguageName);
            }
        }
        [TestMethod]
        public async Task TestDelete_Correct()
        {
            LanguageViewModel viewModel = new LanguageViewModel()
            {
                LanguageName = "France",
            };
            var data = await _controller.CreateLanguage(viewModel) as CreatedAtActionResult;
            var item = data.Value as LanguageModel;
            var response = await _controller.DeleteLanguage((int)item.Id);
            var result = response.Result as OkObjectResult;
            Assert.AreEqual(result.StatusCode, 200);
        }
        [TestMethod]
        public async Task TestDelete_Incorrect()
        {
            var response = await _controller.DeleteLanguage(1488);
            var result = response.Result as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
    }
}
