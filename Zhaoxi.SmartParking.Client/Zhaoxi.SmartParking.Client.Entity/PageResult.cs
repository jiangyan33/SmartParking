namespace Zhaoxi.SmartParking.Client.Entity
{
    public class PageResult<T>
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }

        public int Rows { get; set; }

        public int Pages { get; set; }
    }
}
