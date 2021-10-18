namespace Shopaholic.CMS.Model.Response
{
    public class MessageModel<T>
    {

        public int Status { get; set; } = 200;

        public bool Success { get; set; } = true;

        public string Msg { get; set; } = "";

        public T Data { get; set; }

    }
}
