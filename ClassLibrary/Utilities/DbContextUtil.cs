using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Util.Utilities
{
    public class DbContextUtil
    {
        public static ShopaholicContext GetDbContext(string connStr)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopaholicContext>();
            optionsBuilder.UseSqlServer(connStr);
            return new ShopaholicContext(optionsBuilder.Options);
        }

        public static ShopaholicContext GetDbContextFromMemory()
        {
            var options = new DbContextOptionsBuilder<ShopaholicContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            return new ShopaholicContext(options);
        }
    }
}
