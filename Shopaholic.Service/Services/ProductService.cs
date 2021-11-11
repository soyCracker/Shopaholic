using Microsoft.AspNetCore.Http;
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

        public bool AddProduct(string name, string description, int categoryId, string content, string image, int price, 
            int stock)
        {
            using(dbContext)
            {
                Product exist = dbContext.Products.SingleOrDefault(x => x.Name == name);
                if(exist == null)
                {
                    Product product = new Product
                    {
                        Name = name,
                        Description = description,
                        CategoryId = categoryId,
                        Content = content,
                        Image = image,
                        Price = price,
                        Stock = stock
                    };
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

        public List<Product> SearchProduct(string name, string description, string content)
        {
            using(dbContext)
            {
                return dbContext.Products.Where(x => x.Name.Contains(name) || x.Description.Contains(description) || 
                    x.Content.Contains(content) ).ToList();
            }
        }

        public bool UpdateProduct(int id, string name, string description, int categoryId, string content, string image, 
            int price, int stock)
        {
            using (dbContext)
            {
                Product origin = dbContext.Products.SingleOrDefault(x => x.Id == id);
                if(origin!=null)
                {
                    origin.Name = name;
                    origin.Description = description;
                    origin.CategoryId = categoryId;
                    origin.Price = price;
                    origin.Content = content;
                    origin.Stock = stock;
                    origin.Image = image;
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }
    }
}
