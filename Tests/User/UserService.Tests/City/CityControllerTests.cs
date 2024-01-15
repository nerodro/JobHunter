using LocationAPI.Controllers;
using LocationAPI.ViewModel;
using LocationDomain.Model;
using LocationService.CityService;
using LocationService.CountryService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobJunster.Tests.City
{
    [TestClass]
    public class CityControllerTests
    {
        CityController _controller;
        Mock<ICityService> _cityMock;
        Mock<ICountryService> _countryMock;
        public CityControllerTests()
        {
            _cityMock = new Mock<ICityService>();
            _countryMock = new Mock<ICountryService>();
            _controller = new CityController(_cityMock.Object, _countryMock.Object);
        }
        [TestMethod]
        public async Task Test_Add_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            CityViewModel viewModel = new CityViewModel()
            {
                CountryId = 1,
                CityName = "Test",
            };
            var response = await _controller.CreateCity(viewModel) as CreatedAtActionResult;
            Assert.IsNotNull(response);
            Assert.AreEqual(response.StatusCode, 201);
            var item = response.Value as CityViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CityName, item.CityName);
            };
        }
        [TestMethod]
        public async Task Test_Add_controller_With_Spaces()
        {
            CityViewModel viewModel = new CityViewModel()
            {
                Id = 1,
                CityName = "  Test  ",
            };
            var resquest = await _controller.CreateCity(viewModel) as CreatedAtActionResult;
            var response = await _controller.SingleCity(1);
            var item = response.Value as CityViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CityName, item.CityName);
            };
        }
        [TestMethod]
        public async Task Test_Update_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            CityViewModel viewModel = new CityViewModel()
            {
                Id = 1,
                CityName = "Test2",
                CountryId = 1
            };
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test", CountryId = 1 });
            var update = await _controller.EditCity(1, viewModel) as OkObjectResult;
            var response = await _controller.SingleCity(1);
            var item = response.Value as CityViewModel;
            if (item != null)
            {
                Assert.AreEqual(viewModel.CityName, item.CityName);
            };
        }
        [TestMethod]
        public async Task Test_Delete_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test" });
            var update = await _controller.DeleteCity(1);
            var result = update.Result as OkObjectResult;
            Assert.AreEqual(result.StatusCode, 200);
        }
        [TestMethod]
        public async Task Test_Delete_Incorrect_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test" });
            var update = await _controller.DeleteCity(123);
            var result = update.Result as BadRequestResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
        [TestMethod]
        public async Task Test_Get_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test", CountryId = 1 });
            var get = await _controller.SingleCity(1);
            var item = get.Value as CityViewModel;
            if (item != null)
            {
                Assert.AreEqual("Test", item.CityName);
            };
        }
        [TestMethod]
        public async Task Test_Get_Incorrecrt_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test" });
            var get = await _controller.SingleCity(123);
            var result = get.Result as BadRequestObjectResult;
            Assert.AreEqual(result.StatusCode, 400);
        }
        [TestMethod]
        public async Task Test_Get_All_controller()
        {
            _countryMock.Setup(x => x.GetCountry(1)).ReturnsAsync(new CountryModel { Id = 1, CountryName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 1, CityName = "Test" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 2, CityName = "Test2" });
            _cityMock.Setup(x => x.GetCity(1)).ReturnsAsync(new CityModel { Id = 3, CityName = "Test3" });
            var res = _controller.Index() as List<CityViewModel>;
            Assert.IsNotNull(res);
        }
    }
}
