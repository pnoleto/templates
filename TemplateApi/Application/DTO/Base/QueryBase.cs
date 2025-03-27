namespace Application.DTO.Base
{
    public class QueryBase: PagingBase
    {
        public string? Q { get; set; }
        public string? Source { get; set; }
    }

}
