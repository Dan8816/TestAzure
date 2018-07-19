using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Products.Models;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Products.Controllers
{
    public class HomeController : Controller
    {
        private YourContext _context;

        public HomeController(YourContext context)
        {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Route("products/new")]
        public IActionResult NewProduct()
        {
            ViewData["Message"] = "You can add a product to the db.";

            return View();
        }

        [HttpPost]
        [Route("CreateProduct")]
        public IActionResult CreateProduct(NewProduct NewestProduct)
        {
            System.Console.WriteLine("**********Hitting the CreateProduct Route**********");
            if(_context.products.Where(checkProduct => checkProduct.ProductName == NewestProduct.ProductName).FirstOrDefault() != null)
            {
                System.Console.WriteLine("**********ProductName already exists**********");
                ModelState.AddModelError("ProductName", "Product already exists");
                return RedirectToAction("NewProduct", NewestProduct);
            }
            System.Console.WriteLine("**********ProductName is Unique**********");
            if(ModelState.IsValid)
            {
                Product newProduct = new Product
                {
                    ProductName = NewestProduct.ProductName,
                    Description = NewestProduct.Description,
                    Price = NewestProduct.Price,
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now
                };
                _context.Add(newProduct);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("prod_id", newProduct.Id);
                int? prod_id = newProduct.Id;
                System.Console.WriteLine("**********" + prod_id + "**********");
                return Redirect($"Product/{prod_id}");
            };
            System.Console.WriteLine("**********CreateProduct Failed**********");
            return RedirectToAction("NewProduct", NewestProduct);
        }
        [HttpGet]
        [Route("categories/new")]
        public IActionResult NewCategory()
        {
            ViewData["Message"] = "You can ad a category to the db.";

            return View();
        }

        [HttpPost]
        [Route("CreateCategory")]
        public IActionResult CreateCategory(NewCategory NewestCategory)
        {
            System.Console.WriteLine("**********Hitting the CreateCategory Route**********");
            if(_context.categories.Where(checkCategrory => checkCategrory.CategoryName == NewestCategory.CategoryName).FirstOrDefault() != null)
            {
                System.Console.WriteLine("**********CategoryName already exists**********");
                ModelState.AddModelError("CategoryName", "Category already exists");
            }
            System.Console.WriteLine("**********ProductName is Unique**********");
            if(ModelState.IsValid)
            {
                Category newCategory = new Category
                {
                    CategoryName = NewestCategory.CategoryName,
                    Created_At = DateTime.Now,
                    Updated_At = DateTime.Now
                };
                _context.Add(newCategory);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("cat_id", newCategory.Id);
                int? cat_id = newCategory.Id;
                System.Console.WriteLine("**********" + cat_id + "**********");
                return Redirect($"Category/{cat_id}");
            };
            System.Console.WriteLine("**********CreateCategory Failed**********");
            return View("Index");
        }

        [HttpGet]
        [Route("Product/{prod_id}")]
        public IActionResult Product(int prod_id)
        {
            System.Console.WriteLine("**********Hitting the Product Route and the prod_id is " + prod_id + "**********");
            ViewData["Message"] = "Add this Product to a Category";
            ViewModel view = new ViewModel()
            {
                ViewCategory = new Category(),
                ViewProduct =  _context.products.SingleOrDefault(p => p.Id == prod_id),
                Groupings = new Groupings(),
                ProductList = _context.products
                                .Include(p => p.Grouplist)
                                .ThenInclude(g => g.AddedCategory)
                                .ToList(),
                CategoryList = _context.categories.ToList()
            };
            System.Console.WriteLine($"The product name is " + view.ViewProduct.ProductName + "**********");
            Product ThisProduct = _context.products.SingleOrDefault(p => p.Id == prod_id);
            ViewBag.Product = ThisProduct; 
            return View(view);
        }

        [HttpGet]
        [Route("Category/{cat_id}")]
        public IActionResult Category(int cat_id)
        {
            System.Console.WriteLine("**********Hitting the Category Route and the cat_id is " + cat_id + "**********");
            ViewData["Message"] = "Add Products to this Category";
            ViewModel view = new ViewModel()
            {
                ViewCategory = _context.categories.SingleOrDefault(c => c.Id == cat_id),
                ViewProduct = new Product(), 
                Groupings = new Groupings(),
                CategoryList = _context.categories
                                .Include(p => p.Grouplist)
                                .ThenInclude(g => g.CategorizedProduct)
                                .ToList(),
                ProductList = _context.products.ToList()
            };
            System.Console.WriteLine($"The category name is " + view.ViewCategory.CategoryName + "**********");
            Category ThisCategory = _context.categories.SingleOrDefault(c => c.Id == cat_id);
            ViewBag.Category = ThisCategory; 
            return View(view);
        }

        [HttpPost]
        [Route("LinkCat2Prod/{id}")]
        public IActionResult LinkCat2Prod(ViewModel FormData, int id)
        {
            System.Console.WriteLine("**********Hitting the LinkCat2Prod Route and the id is " + id + " **********");
            System.Console.WriteLine("*****" + FormData.Groupings.ProductId + "*****");
            Groupings NewCatorgization = new Groupings()
            {
                ProductId = FormData.Groupings.ProductId,
                CategorizedProduct = _context.products.SingleOrDefault(p => p.Id == FormData.Groupings.ProductId),
                CategoryId = FormData.Groupings.CategoryId,
                AddedCategory = _context.categories.SingleOrDefault(c => c.Id == FormData.Groupings.CategoryId),
            };
            _context.categorized.Add(NewCatorgization);
            _context.SaveChanges();
            NewCatorgization.AddedCategory.Grouplist.Add(NewCatorgization);
            _context.SaveChanges();

            return Redirect("/");
        }

        //[HttpPost]
        //[Route("LinkProd2Cat")]
        //public IActionResult LinkProd2Cat()
        //{
            //return View("Index");
        //}

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
