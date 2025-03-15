using Application.Results.Base;

namespace Application.Results
{
    public class LoginResult: ResultBase
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
