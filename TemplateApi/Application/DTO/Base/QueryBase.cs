namespace Application.DTO.Base
{
    public class QueryBase: PagingBase
    {
        public string Q { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }

}
