using Shopaholic.Entity.Models;
using Shopaholic.Service.Interfaces;
using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Services
{
    public class ShoppingCartService : ICartService
    {
        private readonly ShopaholicContext dbContext;

        public ShoppingCartService(ShopaholicContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public bool Add(string email, int productId, int quantity)
        {
            using(dbContext)
            {
                var product = dbContext.Products.SingleOrDefault(p => p.Id==productId);
                if(product.Stock<quantity)
                {
                    return false;
                }
                else
                {
                    var user = dbContext.CustomerAccounts.SingleOrDefault(c => c.Email == email);
                    product.Stock = product.Stock - quantity;
                    dbContext.ShoppingCarts.Add(new ShoppingCart
                    {
                        AccountId = user.AccountId,
                        ProductId = productId,
                        Quantity = quantity
                    });
                    dbContext.SaveChanges();
                }
                return true;
            }
        }

        public void Delete(int cartId)
        {
            using (dbContext)
            {
                var cart = dbContext.ShoppingCarts.SingleOrDefault(c => c.Id == cartId);
                dbContext.ShoppingCarts.Remove(cart);
                dbContext.SaveChanges();
            }
        }

        public List<CartWithProductDTO> GetCartWithProductList(string email)
        {
            using (dbContext)
            {
                var user = dbContext.CustomerAccounts.SingleOrDefault(c => c.Email == email);
                var carts = dbContext.ShoppingCarts.Where(x=>x.AccountId==user.AccountId).ToList();
                List<CartWithProductDTO> cartDtoList = new List<CartWithProductDTO>();
                foreach (var cart in carts)
                {
                    var product = dbContext.Products.SingleOrDefault(p => p.Id==cart.ProductId);
                    cartDtoList.Add(new CartWithProductDTO
                    {
                        Id = cart.Id,
                        AccountId = cart.AccountId,
                        Quantity = cart.Quantity,
                        ProductId = cart.ProductId,
                        ProductName = product.Name,
                        ProductImage = product.Image,
                        Price = product.Price,
                        CreateTime = cart.CreateTime,
                        UpdateTime = cart.UpdateTime
                    });
                }
                return cartDtoList;
            }
        }
    }
}
