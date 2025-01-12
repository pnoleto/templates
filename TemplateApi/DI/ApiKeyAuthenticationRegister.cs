using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Infra.DI
{
    public class ApiKeyAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public IEnumerable<string> ApiKeys { get; set; } = [];
    }
    public class ApiKeyAuthenticationSchemeHandler(
        IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder) : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>(options, logger, encoder)
    {
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string apikey = Context.Request.Headers["X-API-KEY"];

            if(string.IsNullOrEmpty(apikey)) return Task.FromResult(AuthenticateResult.Fail("X-API-KEY required"));

            if (!Options.ApiKeys.Contains(apikey)) return Task.FromResult(AuthenticateResult.Fail("Invalid X-API-KEY"));

            var ticket = new AuthenticationTicket(new(new ClaimsIdentity([new(ClaimTypes.Name, "VALID KEY")], Scheme.Name)), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
    public static class ApiKeyAuthenticationRegister
    {
        public static AuthenticationBuilder AddApiKeyAuthentication(this IServiceCollection services, IConfiguration configuration)
            => services.AddAuthentication("ApiKey")
            .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey", options =>
            {
                options.ApiKeys = configuration.GetSection("ApiKeys")?.Get<string[]>() ?? throw new ArgumentNullException(nameof(configuration));
            });
    }
}
