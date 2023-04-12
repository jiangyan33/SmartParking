namespace Zhaoxi.SmartParking.Server.Models.Dto
{
    public class PageQuery
    {
        public int PageNo { get; set; } = 1;

        public int PageSize { get; set; } = 20;

        public string Search { get; set; }
    }
}
