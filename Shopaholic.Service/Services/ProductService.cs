using Shopaholic.Util.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
                    product.UpdateTime = TimeUtil.GetLocalDateTime();
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
                    origin.UpdateTime = TimeUtil.GetLocalDateTime();
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

        public ProductSearchResDTO SearchProductWithCategory(string searchStr, int page, int pageSize)
        {
            using (dbContext)
            {
                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                var filterData = dbContext.Products.Where(x => !x.IsDelete && (x.Id.ToString().Contains(searchStr) || x.Name.Contains(searchStr) || x.Description.Contains(searchStr) ||
                    x.Content.Contains(searchStr)));

                List<Product> products = filterData.OrderBy(y => y.Id).Skip((page-1) * pageSize).Take(pageSize).Include(c => c.Category).ToList();                
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                foreach (Product product in products)
                {
                    ProductDTO productDTO = new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        CategoryId = product.CategoryId,
                        CategoryName = product.Category.Name,
                        Content = product.Content,
                        Image = product.Image,
                        Price = product.Price,
                        Stock = product.Stock
                    };
                    productDTOs.Add(productDTO);
                }

                int count = filterData.Count();
                ProductSearchResDTO productSearchResDTO = new ProductSearchResDTO
                {
                    ProductDTOs = productDTOs,
                    TotalPages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1
                };
                return productSearchResDTO;
            }
        }

        public void AddProductRange(List<ProductDTO> productDTOs)
        {
            using (dbContext)
            {
                List<Product> productList = new List<Product>();
                foreach (ProductDTO productDto in productDTOs)
                {
                    productList.Add(new Product
                    {
                        Name = productDto.Name,
                        Description = productDto.Description,
                        Content = productDto.Content,
                        Price = productDto.Price,
                        Stock = productDto.Stock,
                        Image = productDto.Image,
                        CategoryId = productDto.CategoryId,
                        IsDelete = productDto.IsDelete
                    });
                }
                dbContext.Products.AddRange(productList);
                dbContext.SaveChanges();
            }
        }

        public ProductSearchResDTO SearchProductByCategory(int categoryId, string searchStr, int page, int pageSize)
        {
            using (dbContext)
            {
                searchStr = string.IsNullOrEmpty(searchStr) ? "" : searchStr;
                var filterData = dbContext.Products.Where(x => x.CategoryId==categoryId && !x.IsDelete && (x.Id.ToString().Contains(searchStr) 
                    || x.Name.Contains(searchStr)
                    || x.Description.Contains(searchStr) 
                    || x.Content.Contains(searchStr) ) );

                List<Product> products = filterData.OrderBy(y => y.Id).Skip((page-1) * pageSize)
                    .Take(pageSize).ToList();
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                foreach (Product product in products)
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
                    productDTOs.Add(productDTO);
                }

                int count = filterData.Count();
                ProductSearchResDTO productSearchResDTO = new ProductSearchResDTO
                {
                    ProductDTOs = productDTOs,
                    TotalPages = count % pageSize == 0 ? count / pageSize : count / pageSize + 1
                };
                return productSearchResDTO;
            }
        }

        public List<ProductDTO> GetProductByMonthFlowTop()
        {
            using (dbContext)
            {
                int flowRange = -29;
                var flowInRange = dbContext.WebFlows.Where(x => x.CreateTime >= DateTime.Now.Date.AddDays(flowRange) 
                    && x.CreateTime < DateTime.Now.Date.AddDays(1));
                var group = flowInRange.GroupBy(x => x.Enter.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Last())
                    .Select(s => new
                    {
                        ProductId = s.Key,
                        Count = s.Key.Count(),
                    });
                var topFive = group.OrderByDescending(o => o.Count).Take(5);
                List<ProductDTO> productDTOs = new List<ProductDTO>();
                foreach(var flow in topFive)
                {
                    var product = dbContext.Products.SingleOrDefault(x => x.Id.ToString()==flow.ProductId);
                    productDTOs.Add(new ProductDTO
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Price = product.Price,
                    });
                }
                return productDTOs;
            }
        }
    }
}
