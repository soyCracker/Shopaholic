using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;

namespace Shopaholic.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly ShopaholicContext dbContext;

        public ProductService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool AddProduct(Product product)
        {
            using(dbContext)
            {
                Product exist = dbContext.Products.SingleOrDefault(x => x.Name == product.Name);
                if(exist == null)
                {
                    dbContext.Products.Add(product);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            using (dbContext)
            {
                Product product = dbContext.Products.SingleOrDefault(x=>x.Id == id);
                if(product!=null)
                {
                    dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public Product GetProduct(int id)
        {
            using(dbContext)
            {
                return dbContext.Products.SingleOrDefault(x=>x.Id==id);
            }
        }

        public List<Product> GetProductList()
        {
            using (dbContext)
            {
                return dbContext.Products.ToList();
            }
        }

        public bool UpdateProduct(Product product)
        {
            using (dbContext)
            {
                Product origin = dbContext.Products.SingleOrDefault(x => x.Id == product.Id);
                if(origin!=null)
                {
                    origin.Name = product.Name;
                    origin.Description = product.Description;
                    origin.CategoryId = product.CategoryId;
                    origin.Price = product.Price;
                    origin.Content = product.Content;
                    origin.Stock = product.Stock;
                    origin.Image = product.Image;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
