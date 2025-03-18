using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Security.Claims;

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
        private ClaimsPrincipal UserIdentity(bool isAdmin = false)
        {
            return new(new ClaimsIdentity([
                    new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, isAdmin? "Admin": "User"),
                    new(ClaimTypes.Expiration, DateTime.Now.AddDays(10).ToString("s"))
                ], Scheme.Name));
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            string apikey = Context.Request.Headers["x-api-key"].ToString();

            if (string.IsNullOrEmpty(apikey)) return Task.FromResult(AuthenticateResult.Fail("x-api-key required"));

            if (!Options.ApiKeys.Contains(apikey)) return Task.FromResult(AuthenticateResult.Fail("Invalid x-api-key"));

            bool isAdmin = apikey == "ApiKey1";

            AuthenticationTicket ticket = new(UserIdentity(isAdmin), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

    }

    public static class ApiKeyAuthenticationExtension
    {
        private const string ApiKeyScheme = "ApiKey";

        public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider()
                .GetRequiredService<IConfiguration>();

            ArgumentNullException.ThrowIfNull(configuration);

            services.AddAuthorization(options =>
            {
                options.AddPolicy(ApiKeyScheme, policy =>
                {
                    policy.AddAuthenticationSchemes(ApiKeyScheme)
                    .RequireAuthenticatedUser();
                });
            }).AddAuthentication(ApiKeyScheme)
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>(ApiKeyScheme, options =>
                {
                    options.ApiKeys = configuration?.GetSection("ApiKeys")?.Get<string[]>() ?? [];
                });

            return services;
        }
    }
}
