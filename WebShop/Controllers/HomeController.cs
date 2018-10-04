using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebShop.DAL.Abstract;
using WebShop.Models;
using WebShop.ViewModels;

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
            return View(filtersList);
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
    }
}