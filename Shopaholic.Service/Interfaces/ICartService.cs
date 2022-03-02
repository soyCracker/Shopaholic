using Shopaholic.Service.Model.Moels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopaholic.Service.Interfaces
{
    public interface ICartService
    {
        bool Add(string accountId, int productId, int quantity);

        void Delete(int cartId);

        List<CartWithProductDTO> GetCartWithProductList(string accountId);
    }
}
