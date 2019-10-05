using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class BasketController : Controller
    {
        IRepository<Customer> customerRepository;
        IBasketService basketService;
        IOrderService orderService;

        public BasketController(IBasketService basketService, IOrderService orderService, IRepository<Customer> customerRepository)
        {
            this.basketService = basketService;
            this.orderService = orderService;
            this.customerRepository = customerRepository;
        }
        // GET: Basket
        public ActionResult Index()
        {
            var basketItems = basketService.GetBasketItems(this.HttpContext);
            return View(basketItems);
        }

        public ActionResult AddToBasket(string id)
        {
            basketService.AddToBasket(this.HttpContext, id);
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromBasket(string id)
        {
            basketService.RemoveFromBasket(this.HttpContext, id);
            return RedirectToAction("Index");
        }

        public PartialViewResult BasketSummary()
        {
            var basketSummary = basketService.GetBasketSummary(this.HttpContext);
            return PartialView(basketSummary);
        }

        [Authorize]
        public ActionResult CheckOut()
        {
            var customer = customerRepository.Collection().FirstOrDefault(c => c.Email == User.Identity.Name);

            if (customer != null)
            {
                Order order = new Order()
                {
                    City = customer.City,
                    Email = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    State = customer.State,
                    Street = customer.Street,
                    ZipCode = customer.ZipCode
                };

                return View(order);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        [Authorize]
        public ActionResult CheckOut(Order order)
        {
            if (ModelState.IsValid)
            {
                var basketItems = basketService.GetBasketItems(this.HttpContext);
                order.OrderStatus = "Order Created";
                order.Email = User.Identity.Name;

                //payment processing

                order.OrderStatus = "Payment processed";
                orderService.CreateOrder(order, basketItems);
                basketService.ClearBasket(this.HttpContext);

                return RedirectToAction("Thankyou", new { OrderId = order.Id });
            }
            else
            {
                return View(order);
            }
        }

        public ActionResult Thankyou(string orderId)
        {
            ViewBag.OrderId = orderId;
            return View();
        }
    }
}