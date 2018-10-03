using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebShop.Models;
using WebShop.Models.Entities;

namespace WebShop.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;// = new ApplicationDbContext();

        public CategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Categories
        public ActionResult Index()
        {
            return View(_context.Categories.OrderBy(c => c.Name).ToList());
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Categories.Add(category);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

       
        [HttpPost]
        public ContentResult CreateCategory(string name, int? parentId)
        {
            var category = new Category
            {
                ParentId = parentId,
                Name = name
            };
            _context.Categories.Add(category);
            _context.SaveChanges();

            string json = JsonConvert.SerializeObject(category.Id);

            return Content(json, "application/json");
        }
        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Entry(category).State = EntityState.Modified;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _context.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            Category category = _context.Categories.Find(id);
            foreach(var p in category.Products)
            {
                p.CategoryId = null;
            }
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ContentResult GetParentCategories()
        {
            var categories =
                _context.Categories
                .Where(c => c.ParentId == null)
                .Select(c=>new
                {
                    id=c.Id,
                    parent = "#",
                    text = c.Name,
                    children = c.Children.Count()!=0 
                }).ToList();
            string json = JsonConvert.SerializeObject(categories);

            return Content(json, "application/json");
        }
        [HttpGet]
        public ContentResult GetChildrenCategories(int id)
        {
            var categories =
                _context.Categories
                .Where(c => c.ParentId == id)
                .Select(c => new
                {
                    id = c.Id,
                    parent = id,
                    text = c.Name,
                    children = c.Children.Count() != 0
                }).ToList();
            string json = JsonConvert.SerializeObject(categories);

            return Content(json, "application/json");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _context.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
