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
        Mock<ILanguageService> _languageService;
        public LanguageControllerTests()
        {
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
            _languageService.Setup(x => x.GetLanguage(1)).ReturnsAsync(new LanguageModel { Id = 1, Language = "  Roma  " });
            var response = await _controller.SingleLanguage(1) as ObjectResult;
            var item = response.Value as LanguageViewModel;
            if (item != null)
            {
                Assert.AreEqual("Roma", item.LanguageName);
            }
        }
        [TestMethod]
        public async Task TestDelete_Correct()
        {
            _languageService.Setup(x => x.GetLanguage(1)).ReturnsAsync(new LanguageModel {Id = 1, Language = "Russ" });
            var response = await _controller.DeleteLanguage(1);
            var result = response.Result as OkObjectResult;
            Assert.AreEqual(result.StatusCode, 200);
        }
        [TestMethod]
        public async Task TestUpdate_Correct()
        {
            LanguageViewModel viewModel = new LanguageViewModel()
            {
                LanguageName = "France",
            };
            _languageService.Setup(x => x.GetLanguage(1)).ReturnsAsync(new LanguageModel { Id = 1, Language = "Russ" });
            var update =  await _controller.EditLanguage(1, viewModel) as OkObjectResult;
            Assert.IsNotNull(update.Value);
            var modelup = update.Value as LanguageViewModel;
            if (modelup != null)
            {
                Assert.AreEqual(viewModel.LanguageName, modelup.LanguageName);
            }
            else
            {
                throw new Exception();
            }
        }
        [TestMethod]
        public async Task TestDelete_Incorrect()
        {
            var response = await _controller.DeleteLanguage(1488);
            var result = response.Result as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
        [TestMethod]
        public async Task TestGetAllt()
        {
            _languageService.Setup(x => x.GetLanguage(1)).ReturnsAsync(new LanguageModel { Id = 1, Language = "Russ" });
            var res = _controller.Index();
            Assert.IsNotNull(res);
        }
    }
}
