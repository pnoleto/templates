namespace Application.Results.Base
{
    public class ResultBase(string status)
    {
        public string Status { get; set; } = status;
    }
}
