using LocationAPI.Controllers;
using LocationAPI.ViewModel;
using LocationDomain.Model;
using LocationService.CountryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobJunster.Tests.Country
{
    [TestClass]
    public class CountryControllerTests
    {
        CountryController _controller;
        Mock<ICountryService> _country;
        public CountryControllerTests()
        {
            _country = new Mock<ICountryService>();
            _controller = new CountryController(_country.Object);
        }
        [TestMethod]
        public async Task Create_New_controller()
        {
            CountryViewModel viewModel = new CountryViewModel()
            {
                Id = 1,
                CountryName = "Test",
            };
            var response = await _controller.CreateCountry(viewModel) as CreatedAtActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);
            var item = response.Value as CountryViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CountryName, item.CountryName);
            };
        }
                [TestMethod]
                public async Task Test_Add_controller_With_Spaces()
                {
                    CountryViewModel viewModel = new CountryViewModel()
                    {
                        Id = 1,
                        CountryName = "  Test  ",
                    };
                    var resquest = await _controller.CreateCountry(viewModel) as CreatedAtActionResult;
                    var response = await _controller.SingleCountry(1);
                    var item = response.Value as CountryViewModel;
                    if (item != null)
                    {
                        Assert.AreEqual(viewModel.CountryName, item.CountryName);
                    };
                }
                [TestMethod]
                public async Task Test_Update_controller()
                {
                    CountryViewModel viewModel = new CountryViewModel()
                    {
                        Id = 1,
                        CountryName = "Test2",
                    };
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    var update = await _controller.EditCountry(1, viewModel) as OkObjectResult;
                    var response = await _controller.SingleCountry(1);
                    var item = response.Value as CountryViewModel;
                    if (item != null)
                    {
                        Assert.AreEqual(viewModel.CountryName, item.CountryName);
                    };
                }
                [TestMethod]
                public async Task Test_Delete_controller()
                {
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    var update = await _controller.DeleteCountry(1);
                    var result = update.Result as OkObjectResult;
                    Assert.AreEqual(result.StatusCode, 200);
                }
                [TestMethod]
                public async Task Test_Delete_Incorrect_controller()
                {
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    var update = await _controller.DeleteCountry(123);
                    var result = update.Result as BadRequestResult;
                    Assert.AreEqual(result.StatusCode, 400);
                }
                [TestMethod]
                public async Task Test_Get_controller()
                {
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    var get = await _controller.SingleCountry(1);
                    var item = get.Value as CountryViewModel;
                    if (item != null)
                    {
                        Assert.AreEqual("Test", item.CountryName);
                    };
                }
                [TestMethod]
                public async Task Test_Get_Incorrecrt_controller()
                {
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    var get = await _controller.SingleCountry(123);
                    var result = get.Result as BadRequestObjectResult;
                    Assert.AreEqual(result.StatusCode, 400);
                }
                [TestMethod]
                public async Task Test_Get_All_controller()
                {
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 2, CountryName = "Test2" });
                    _country.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 3, CountryName = "Test3" });
                    var res = _controller.Index() as List<CountryViewModel>;
                    Assert.IsNotNull(res);
                }
    }
}
