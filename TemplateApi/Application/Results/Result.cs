using Application.Results.Base;

namespace Application.Results
{
    public class Result<T>(string status, T data): ResultBase(status)
    {
        public T Data { get; set; } = data;
    }
}
