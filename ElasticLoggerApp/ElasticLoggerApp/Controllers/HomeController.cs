using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElasticLoggerApp.Models;
using ElasticLoggerApp.Data;

namespace ElasticLoggerApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            using (ElasticContext dbContext = new ElasticContext())
            {
                var data = dbContext.Products.ToList();
                return View(data);
            }
        }

        public IActionResult Detail(int? ID)
        {
            Product product = null;
            if (ID == null)
            {
                product = new Product();
            }
            else
            {
                using (ElasticContext dbContext = new ElasticContext())
                {
                    product = dbContext.Products.FirstOrDefault(prod => prod.Id == ID);
                }
            }
            return View(product);
        }
        public async Task<IActionResult> Update(Product product)
        {
            using (ElasticContext dbContext = new ElasticContext())
            {
                if (product.Id == 0)
                {
                    var newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.SeriNumber = product.SeriNumber;
                    newProduct.Price = product.Price;
                    newProduct.Count = product.Count;
                    await dbContext.Products.AddAsync(newProduct);
                }
                else
                {
                    var updateProduct = dbContext.Products.FirstOrDefault(prod => prod.Id == product.Id);
                    updateProduct.Name = product.Name;
                    updateProduct.SeriNumber = product.SeriNumber;
                    updateProduct.Price = product.Price;
                    updateProduct.Count = product.Count;
                }
                dbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
