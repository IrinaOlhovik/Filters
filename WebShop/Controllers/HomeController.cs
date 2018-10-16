using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL.Abstract;
using WebShop.Models;
using WebShop.ViewModels;
using WebShop.Models.Entities;

namespace WebShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly ApplicationDbContext _context;// = new ApplicationDbContext();

        public HomeController(IUserService userService, ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        //public ApplicationDbContext MyDbContext
        //{
        //    get
        //    {
        //        return HttpContext.GetOwinContext().Get<ApplicationDbContext>();
        //    }
        //}
        public ActionResult Index()
        {
            //int mycount = _userService.GetCountUsers();
            //Будую дерево фільтрів
            var filtersList = GetListFilters();
            ViewBag.RoleId = 0;// _userService.AddRole("Admin");
            HomeViewModel homeViewModel = new HomeViewModel
            {
                Filters = filtersList,
                Products = new List<Product>()
               
            };
            return View(homeViewModel);
        }
        private List<FNameViewModel> GetListFilters()
        {
            var query = from f in _context.VFilterNameGroups.AsQueryable()
                        where f.FilterValueId != null
                        select new
                        {
                            FNameId = f.FilterNameId,
                            FName = f.FilterName,
                            FValueId = f.FilterValueId,
                            FValue = f.FilterValue
                        };
            var groupNames = from f in query
                             group f by new
                             {
                                 Id = f.FNameId,
                                 Name = f.FName
                             } into g
                             orderby g.Key.Name
                             select g;
            List<FNameViewModel> listGroupFilters =
                groupNames.Select(fn => new FNameViewModel
                {
                    Id = fn.Key.Id,
                    Name = fn.Key.Name,
                    Children = (from v in fn
                                group v by new FValueViewModel
                                {
                                    Id = v.FValueId,
                                    Name = v.FValue
                                } into g
                                select g.Key).ToList()
                }).ToList();


            //new List<FNameViewModel>();
            //foreach (var filterName in groupNames)
            //{
            //    FNameViewModel fName = new FNameViewModel
            //    {
            //        Id = filterName.Key.Id,
            //        Name = filterName.Key.Name
            //    };

            //    fName.Children = (from v in filterName
            //                      group v by new FValueViewModel
            //                      {
            //                          Id = v.FValueId,
            //                          Name = v.FValue
            //                      } into g
            //                      select g.Key).ToList();

            //    listGroupFilters.Add(fName);
            //}
            return listGroupFilters;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public static Predicate<T> Or<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (predicate(item))
                    {
                        return true;
                    }
                }
                return false;
            };
        }
        public static Predicate<T> And<T>(params Predicate<T>[] predicates)
        {
            return delegate (T item)
            {
                foreach (Predicate<T> predicate in predicates)
                {
                    if (!predicate(item))
                    {
                        return false;
                    }
                }
                return true;
            };
        }
        [HttpPost]
        public ActionResult Index(string[] fvalues, List<FNameViewModel> filtersGroup)
        {
            var filtersList = GetListFilters();
            List<FNameViewModel> listFilters = new List<FNameViewModel>();
            Predicate <Models.Entities.Filter> predicate = s => true;
            Predicate<Models.Entities.Filter> predicateChild = s => false;
            foreach (var item in fvalues)
            {
                int id = int.Parse(item);
                var query = from f in _context.VFilterNameGroups.AsQueryable()
                            where f.FilterValueId == id
                            select new
                            {
                                FNameId = f.FilterNameId,
                                FName = f.FilterName,
                                FValueId = f.FilterValueId,
                                FValue = f.FilterValue
                            };
                var groupNames = from f in query
                                 group f by new
                                 {
                                     Id = f.FNameId,
                                     Name = f.FName
                                 } into g
                                 orderby g.Key.Name
                                 select g;
                List<FNameViewModel> listGroup =
                    groupNames.Select(fn => new FNameViewModel
                    {
                        Id = fn.Key.Id,
                        Name = fn.Key.Name,
                        Children = (from v in fn
                                    group v by new FValueViewModel
                                    {
                                        Id = v.FValueId,
                                        Name = v.FValue
                                    } into g
                                    select g.Key).ToList()
                    }).ToList();
                listFilters.AddRange(listGroup);
            }
            foreach (var item in listFilters)
            {
                foreach (var child in item.Children)
                {
                    Predicate<Models.Entities.Filter> predicate1 = s => s.FilterValueId == child.Id;
                    predicateChild = And(predicateChild,predicate1);
                }
                predicate = Or(predicate,predicateChild);
                predicateChild = s => false;
            }
            List<Models.Entities.Filter> filters = _context.Filters.ToList().FindAll(predicate);
            List<Product> products = new List<Product>();
            foreach (var item in filters)
            {
                if (products.SingleOrDefault(p => p.Id == item.ProductId) != null)
                    continue;
                products.Add(_context.Products.SingleOrDefault(p => p.Id == item.ProductId));
            }
            //foreach (var item in fvalues)
            //{
            //    int id = int.Parse(item);
            //    filtersList.SingleOrDefault(f => f.Id == id).IsChecked = true;
            //}
                HomeViewModel homeViewModel = new HomeViewModel
            {
                Filters = filtersList,
                Products = products
            };
            return View(homeViewModel);
        }
    }
}