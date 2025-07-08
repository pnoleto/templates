namespace Application.DTO.Base
{
    public class PagingBase(int page, int pageSize)
    {
        public int Page { get; set; } = page;
        public int PageSize { get; set; } = pageSize;
    }
}
