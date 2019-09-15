using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller
    {

        IRepository<Product> productContext;
        IRepository<ProductCategory> productCategoryContext;

        public ProductManagerController(IRepository<Product> _productContext, IRepository<ProductCategory> _productCategoryContext)
        {
            this.productContext = _productContext;
            this.productCategoryContext = _productCategoryContext;
        }

        // GET: ProductManager
        public ActionResult Index()
        {
            List<Product> products = productContext.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel()
            {
                Product = new Product(),
                productCategories = productCategoryContext.Collection()
            };
            return View(productManagerViewModel);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            else
            {
                productContext.Insert(product);
                productContext.Commit();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string Id)
        {
            Product product = productContext.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                ProductManagerViewModel productManagerViewModel = new ProductManagerViewModel()
                {
                    Product = product,
                    productCategories = productCategoryContext.Collection()
                };
                return View(productManagerViewModel);
            }
        }

        [HttpPost]
        public ActionResult Edit(Product product, string Id)
        {
            Product productToEdit = productContext.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(productToEdit);
                }
                else
                {
                    productToEdit.Category = product.Category;
                    productToEdit.Description = product.Description;
                    productToEdit.Image = product.Image;
                    productToEdit.Name = product.Name;
                    productToEdit.Price = product.Price;

                    productContext.Commit();
                    return RedirectToAction("Index");
                }
            }
        }

        public ActionResult Delete(string Id)
        {
            Product product = productContext.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(product);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product product = productContext.Find(Id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else
            {
                productContext.Delete(Id);
                productContext.Commit();
                return RedirectToAction("Index");
            }
        }
    }
}