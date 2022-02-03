using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportsStore_EFCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportsStore_EFCore.Controllers
{ 
    public class SeedController : Controller
    {
        private DataContext context;

        public SeedController(DataContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            ViewBag.Count = context.Products.Count();
            return View(context.Products
                .Include(p => p.Category).OrderBy(p => p.Id).Take(20));
        }

        [HttpPost]
        public IActionResult CreateProductionData()
        {
            ClearData();
            context.Categories.AddRange(new Category[]
            {
                new Category
                {
                    Name = "Watersports",
                    Description = "Make a splash",
                    Products = new Product[]
                    {
                        new Product { Name = "Kayak", Description = "A boat for one person", PurchasePrice = 200, RetailPrice = 275 },
                        new Product { Name = "Lifejacket", Description = "Protective and fashionable", PurchasePrice = 30, RetailPrice = 48.95m },
                    }
                },
                new Category
                {
                    Name = "Soccer",
                    Description = "The wrold's favorite game",
                    Products = new Product[]
                    {
                        new Product { Name = "Soccer ball", Description = "Ball", PurchasePrice = 18, RetailPrice = 19.50m },
                        new Product { Name = "Corner Flags", Description = "Just flags", PurchasePrice = 32.50m, RetailPrice = 34.95m },
                        new Product { Name = "Stadium", Description = "Flat-packed stadium", PurchasePrice = 75000, RetailPrice = 79500 }
                    }
                },
                new Category
                {
                    Name = "Chess",
                    Description = "The thinky game",
                    Products = new Product[]
                    {
                        new Product { Name = "Thinking cap", Description = "Improve brain efficiency", PurchasePrice = 10, RetailPrice = 16 },
                        new Product { Name = "Unsteady chair", Description = "Give your opponent a disadvantage", PurchasePrice = 28, RetailPrice = 29.95m },
                        new Product { Name = "Human Chess Board", Description = "Chess board", PurchasePrice = 68.50m, RetailPrice = 75 },
                        new Product { Name = "Bling-Bling King", Description = "Gold palted king", PurchasePrice = 800, RetailPrice = 1200 },
                    }
                }
            });
            context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CreateSeedData(int count)
        {
            ClearData();
            if(count > 0)
            {
                context.Database.SetCommandTimeout(System.TimeSpan.FromMinutes(10));
                context.Database.ExecuteSqlRaw("DROP PROCEDURE IF EXISTS CreateSeedData");
                context.Database.ExecuteSqlRaw($@"CREATE PROCEDURE CreateSeedData 
                    @RowCount decimal
                   AS
                    BEGIN
                        SET NOCOUNT ON
                        DECLARE @i INT = 1;
                        DECLARE @catId BIGINT;
                        DECLARE @CatCount INT = @RowCount / 10;
                        DECLARE @pprice DECIMAL(5,2);
                        DECLARE @rprice DECIMAL(5,2);
                        BEGIN TRANSACTION
                            WHILE @i <= @CatCount
                                BEGIN
                                    INSERT INTO Categories (Name, Description)
                                    VALUES(CONCAT('Category-', @i), 'Test Data Category');
                                    SET @catId = SCOPE_IDENTITY();
                                    DECLARE @j INT = 1;
                                    WHILE @j <= 10
                                        BEGIN
                                            SET @pprice = RAND()*(500-5+1);
                                            SET @rprice = (RAND() * @pprice) + @pprice;
                                            INSERT INTO Products (Name, CategoryId, PurchasePrice, RetailPrice)
                                            VALUES (CONCAT('Product', @i, '-', @j), @catId, @pprice, @rprice)
                                            SET @j = @j + 1
                                        END
                                    SET @i = @i + 1
                                END
                            COMMIT
                        END");

                context.Database.BeginTransaction();
                context.Database.ExecuteSqlRaw($"EXEC CreateSeedData @RowCount = {count}");
                context.Database.CommitTransaction();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ClearData()
        {
            context.Database.SetCommandTimeout(System.TimeSpan.FromMinutes(10));
            context.Database.BeginTransaction();
            context.Database.ExecuteSqlRaw("DELETE FROM Orders");
            context.Database.ExecuteSqlRaw("DELETE FROM Categories");
            context.Database.CommitTransaction();
            return RedirectToAction(nameof(Index));
        }
    }
}
