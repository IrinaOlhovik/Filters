namespace WebShop.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WebShop.Models.Entities;
    using System.Collections.Generic;
    using System.Reflection;
    using System.IO;

    internal sealed class Configuration : DbMigrationsConfiguration<WebShop.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "WebShop.Models.ApplicationDbContext";
        }

        protected override void Seed(WebShop.Models.ApplicationDbContext context)
        {

            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);

            string baseDir = Path.GetDirectoryName(path) + "\\Migrations\\SqlView\\vFilterNameGroups.sql";
            context.Database.ExecuteSqlCommand(File.ReadAllText(baseDir));

            #region InitFilterName
            context.FiltersName.AddOrUpdate(
                h => h.Id,
                new FilterName
                {
                    Id=1,
                    Name="Колір"
                });
            context.FiltersName.AddOrUpdate(
                h => h.Id,
                new FilterName
                {
                    Id = 2,
                    Name = "Розмір"
                });
            #endregion

            #region InitFilterValue
            FilterValue[] values =
            {
                new FilterValue { Id = 1, Name = "L"},
                new FilterValue { Id = 2, Name = "M"},
                new FilterValue { Id = 3, Name = "XL"},
                new FilterValue { Id = 4, Name = "XX"},
                new FilterValue { Id = 5, Name = "Чориний"},
                new FilterValue { Id = 6, Name = "Білий"},
                new FilterValue { Id = 7, Name = "Зелений"},
                new FilterValue { Id = 8, Name = "Жовтий"}
            };
            context.FiltersValue
                    .AddOrUpdate(h => h.Id, values);
            #endregion

            #region InitFilterNameGroups

            FilterNameGroup[] filterNameGroups =
            {
                new FilterNameGroup { FilterNameId = 1, FilterValueId=5 },
                new FilterNameGroup { FilterNameId = 1, FilterValueId=6 },
                new FilterNameGroup { FilterNameId = 1, FilterValueId=7 },
                new FilterNameGroup { FilterNameId = 1, FilterValueId=8 },
                new FilterNameGroup { FilterNameId = 2, FilterValueId=1 },
                new FilterNameGroup { FilterNameId = 2, FilterValueId=2 },
                new FilterNameGroup { FilterNameId = 2, FilterValueId=3 },
                new FilterNameGroup { FilterNameId = 2, FilterValueId=4 },
            };
            context.FilterNameGroups.AddOrUpdate(h => new { h.FilterNameId, h.FilterValueId }, filterNameGroups);
            #endregion

            #region InitProducts
            Product[] products =
            {
                new Product { Id=1, Name="Джинси", Price=240, Description="asdfasfd11111111", CategoryId=1 },
                new Product { Id=2, Name="Брукі", Price=140, Description="asdfasfd1111111111", CategoryId=1 },
                new Product { Id=3, Name="Труси", Price=40, Description="asdfasfd1111111", CategoryId=1 },
                new Product { Id=4, Name="Майка", Price=20, Description="asdfasfd11111111", CategoryId=1 }
            };
            context.Products.AddOrUpdate(p => p.Id, products);
            #endregion

            #region InitFilter
            Filter[] filters =
            {
                new Filter { FilterNameId=1, FilterValueId=6, ProductId=4 },
                new Filter { FilterNameId=1, FilterValueId=7, ProductId=4 },
                new Filter { FilterNameId=2, FilterValueId=2, ProductId=4 },
                new Filter { FilterNameId=1, FilterValueId=6, ProductId=2 },
                new Filter { FilterNameId=1, FilterValueId=7, ProductId=1 },
            };
            context.Filters.AddOrUpdate(f => new { f.FilterNameId, f.FilterValueId, f.ProductId }, filters);
            #endregion


            
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Clothes" },
                new Category { Name = "Play and Toys" },
                new Category { Name = "Feeding" },
                new Category { Name = "Medicine" },
                new Category { Name= "Travel" },
                new Category { Name= "Sleeping" }
            };
            categories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
            context.SaveChanges();

            //var products = new List<Product>
            //{
            //    new Product { Name = "Sleep Suit", Description="For sleeping or general wear",Price=4.99M, CategoryId=categories.Single( c => c.Name == "Clothes").Id},
            //    new Product { Name = "Vest", Description="For sleeping or general wear", Price=2.99M, CategoryId=categories.Single( c => c.Name == "Clothes").Id},
            //    new Product { Name = "Orange and Yellow Lion", Description="Makes a squeaking noise", Price=1.99M, CategoryId=categories.Single( c => c.Name == "Play and Toys").Id},
            //    new Product { Name = "Blue Rabbit", Description="Baby comforter", Price=2.99M, CategoryId=categories.Single( c => c.Name == "Play and Toys").Id  },
            //    new Product { Name = "3 Pack of Bottles", Description="For a leak free drink everytime", Price=24.99M, CategoryId=categories.Single( c => c.Name == "Feeding").Id},
            //    new Product { Name = "3 Pack of Bibs", Description="Keep your baby dry when feeding", Price=8.99M, CategoryId=categories.Single( c => c.Name == "Feeding").Id},
            //    new Product { Name = "Powdered Baby Milk", Description="Nutritional and Tasty", Price=9.99M, CategoryId=categories.Single( c => c.Name == "Feeding").Id},
            //    new Product { Name = "Pack of 70 Disposable Nappies", Description="Dry and secure nappies with snug fit", Price=19.99M, CategoryId=categories.Single( c => c.Name == "Feeding").Id},
            //    new Product { Name = "Colic Medicine", Description="For helping with baby colic pains", Price=4.99M, CategoryId=categories.Single( c => c.Name == "Medicine").Id},
            //    new Product { Name = "Reflux Medicine", Description="Helps to prevent milk regurgitation and sickness", Price=4.99M, CategoryId=categories.Single( c => c.Name == "Medicine").Id},
            //    new Product { Name = "Black Pram and Pushchair System", Description="Convert from pram to pushchair, with raincover", Price=299.99M, CategoryId=categories.Single( c => c.Name == "Travel").Id},
            //    new Product { Name = "Car Seat", Description="For safe car travel", Price=49.99M, CategoryId= categories.Single( c => c.Name == "Travel").Id},
            //    new Product { Name = "Moses Basket", Description="Plastic moses basket", Price=75.99M, CategoryId=categories.Single( c => c.Name == "Sleeping").Id},
            //    new Product { Name = "Crib", Description="Wooden crib", Price=35.99M, CategoryId= categories.Single( c => c.Name == "Sleeping").Id  },
            //    new Product { Name = "Cot Bed", Description="Converts from cot into bed for older children", Price=149.99M, CategoryId=categories.Single( c => c.Name == "Sleeping").Id  },
            //    new Product { Name = "Circus Crib Bale", Description="Contains sheet, duvet and bumper", Price=29.99M, CategoryId=categories.Single( c => c.Name == "Sleeping").Id  },
            //    new Product { Name = "Loved Crib Bale", Description="Contains sheet, duvet and bumper", Price=35.99M, CategoryId=categories.Single( c => c.Name == "Sleeping").Id  }
            //};

            //products.ForEach(c => context.Products.AddOrUpdate(p => p.Name, c));
            //context.SaveChanges();
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
