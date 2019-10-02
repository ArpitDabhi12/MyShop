using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.Services;
using MyShop.WebUI.Controllers;
using MyShop.WebUI.Tests.Mocks;

namespace MyShop.WebUI.Tests.Controllers
{
    [TestClass]
    public class BasketControllerTests
    {
        [TestMethod]
        public void CanAddBasketItems()
        {
            //Assign
            IRepository<Basket> basketContext = new MockContext<Basket>();
            IRepository<Product> productContext = new MockContext<Product>();
            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(productContext, basketContext);
            var basketController = new BasketController(basketService);
            basketController.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), basketController);

            //basketService.AddToBasket(httpContext, "1");

            //Act
            basketController.AddToBasket("1");

            Basket basket = basketContext.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetBasketSummary()
        {
            IRepository<Basket> basketRepository = new MockContext<Basket>();
            IRepository<Product> productRepository = new MockContext<Product>();
            var httpContext = new MockHttpContext();

            productRepository.Insert(new Product() { Id = "1", Price = 10.00m });
            productRepository.Insert(new Product() { Id = "2", Price = 5.00m });

            Basket basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2 });
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 1 });
            basketRepository.Insert(basket);

            IBasketService basketService = new BasketService(productRepository, basketRepository);
            var controller = new BasketController(basketService);
            httpContext.Request.Cookies.Add(new System.Web.HttpCookie("eCommerceBasket") { Value = basket.Id });
            controller.ControllerContext = new System.Web.Mvc.ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;

            var basketSummary = (BasketSummaryViewModel)result.ViewData.Model;
            Assert.AreEqual(3, basketSummary.BasketCount);
            Assert.AreEqual(25.00m, basketSummary.BasketTotal);

        }
    }
}
