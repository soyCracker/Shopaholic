namespace Shopaholic.Service.Common.Exceptions
{
    public class ShopaholicConfigNotFoundException : Exception
    {
        public ShopaholicConfigNotFoundException()
        {
        }

        public ShopaholicConfigNotFoundException(string message)
            : base(message)
        {
        }

        public ShopaholicConfigNotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
