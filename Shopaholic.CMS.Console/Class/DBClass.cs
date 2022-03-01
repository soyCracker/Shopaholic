using Microsoft.EntityFrameworkCore;
using Shopaholic.Entity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Base.Console.Class
{
    public class DBClass
    {
        public static ShopaholicContext GetDbContext(string connStr)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ShopaholicContext>();
            optionsBuilder.UseSqlServer(connStr);
            return new ShopaholicContext(optionsBuilder.Options);
        }
    }
}
