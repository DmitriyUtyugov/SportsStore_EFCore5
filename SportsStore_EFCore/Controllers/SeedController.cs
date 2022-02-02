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
