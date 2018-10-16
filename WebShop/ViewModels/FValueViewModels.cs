using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebShop.Models.Entities;

namespace WebShop.ViewModels
{
    public class FValueViewModel
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
    public class FNameViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FValueViewModel> Children { get; set; }
    }

    public class HomeViewModel
    {
        public List<FNameViewModel> Filters { get; set; }
        public List<Product> Products { get; set; }
    }
}