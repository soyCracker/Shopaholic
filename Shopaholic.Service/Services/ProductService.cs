using Microsoft.AspNetCore.Http;
using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;

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
                    product.IsDelete = true;
                    //dbContext.Products.Remove(product);
                    dbContext.SaveChanges();
                    return true;
                }
                return false;
            }
        }

        public ProductDTO GetProduct(int id)
        {
            using(dbContext)
            {
                Product product = dbContext.Products.SingleOrDefault(x => x.Id == id && x.IsDelete == false);
                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Content = product.Content,
                    Image = product.Image,
                    Price = product.Price,
                    Stock = product.Stock
                };
                return productDTO;
            }
        }

        public List<ProductDTO> GetProductList()
        {
            using (dbContext)
            {
                List<Product> productList = dbContext.Products.Where(x => x.IsDelete == false).ToList();
                List<ProductDTO> productDTOList = new List<ProductDTO>();
                foreach (Product product in productList)
                {
                    ProductDTO productDTO = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        CategoryName = dbContext.Categories.SingleOrDefault(x => x.Id== product.CategoryId).Name,
                        Content = product.Content,
                        Image = product.Image,
                        Price = product.Price,
                        Stock = product.Stock
                    };
                    productDTOList.Add(productDTO);
                }
                return productDTOList;
            }
        }

        public List<ProductDTO> SearchProduct(string name, string description, string content)
        {
            using(dbContext)
            {
                name = name == null ? "" : name;
                description = description == null ? "" : description;
                content = content == null ? "" : content;

                List<Product> productList = dbContext.Products.Where(x => x.IsDelete == false && (x.Name.Contains(name) || x.Description.Contains(description) ||
                    x.Content.Contains(content))).ToList();
                List<ProductDTO> productDTOList = new List<ProductDTO>();
                foreach (Product product in productList)
                {
                    ProductDTO productDTO = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        Content = product.Content,
                        Image = product.Image,
                        Price = product.Price,
                        Stock = product.Stock
                    };
                    productDTOList.Add(productDTO);
                }
                return productDTOList;
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

        public ProductWithAllCategoryDTO GetProductWithAllCategory(int id)
        {
            using (dbContext)
            {
                Product product = dbContext.Products.SingleOrDefault(x => x.Id == id);
                ProductDTO productDTO = new ProductDTO
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId,
                    Content = product.Content,
                    Image = product.Image,
                    Price = product.Price,
                    Stock = product.Stock
                };
                List<Category> categoryList = dbContext.Categories.ToList();
                List<CategoryDTO> categoryDTOList = new List<CategoryDTO>();
                foreach (Category category in categoryList)
                {
                    CategoryDTO categoryDTO = new CategoryDTO
                    {
                        Name = category.Name,
                        Id = category.Id
                    };
                    categoryDTOList.Add(categoryDTO);
                }
                ProductWithAllCategoryDTO productWithAllCategoryDTO = new ProductWithAllCategoryDTO
                {
                    ProductDTOItem = productDTO,
                    CategoryDTOList = categoryDTOList
                };
                return productWithAllCategoryDTO;
            }
        }

        public int GetProductPages(int pageSize)
        {
            using(dbContext)
            {
                int count = dbContext.Products.Count();
                return count%pageSize==0 ? count/pageSize : count/pageSize+1;
            }       
        }

        public List<ProductDTO> SearchProduct(string name, string description, string content, int page, int pageSize)
        {
            using (dbContext)
            {
                name = name == null ? "" : name;
                description = description == null ? "" : description;
                content = content == null ? "" : content;

                List<Product> productList = dbContext.Products.Where(x => x.Name.Contains(name) || x.Description.Contains(description) ||
                    x.Content.Contains(content)).OrderBy(y => y.Id).Skip((page-1) * pageSize).Take(pageSize).ToList();
                List<ProductDTO> productDTOList = new List<ProductDTO>();
                foreach (Product product in productList)
                {
                    ProductDTO productDTO = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        Content = product.Content,
                        Image = product.Image,
                        Price = product.Price,
                        Stock = product.Stock
                    };
                    productDTOList.Add(productDTO);
                }
                return productDTOList;
            }
        }
    }
}
