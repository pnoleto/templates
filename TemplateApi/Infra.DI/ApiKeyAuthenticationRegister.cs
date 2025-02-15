﻿using Microsoft.AspNetCore.Authentication;
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
            string apikey = Context.Request.Headers["x-api-key"].ToString();

            if (string.IsNullOrEmpty(apikey)) return Task.FromResult(AuthenticateResult.Fail("x-api-key required"));

            if (!Options.ApiKeys.Contains(apikey)) return Task.FromResult(AuthenticateResult.Fail("Invalid x-api-key"));

            AuthenticationTicket ticket = new(UserIdentity(), Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }

        private ClaimsPrincipal UserIdentity()
        {
            return new(new ClaimsIdentity([
                    new(ClaimTypes.Sid, Guid.NewGuid().ToString()),
                    new(ClaimTypes.Role, "User"),
                    new(ClaimTypes.Expiration, DateTime.Now.AddDays(10).ToString("s"))
                ], Scheme.Name));
        }
    }

    public static class ApiKeyAuthenticationRegister
    {
        public static IServiceCollection AddApiKeyAuthentication(this IServiceCollection services)
        {
            IConfiguration configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

            if (configuration is null) throw new ArgumentNullException(nameof(configuration));

            services.AddAuthentication("ApiKey")
                .AddScheme<ApiKeyAuthenticationSchemeOptions, ApiKeyAuthenticationSchemeHandler>("ApiKey", options =>
                {
                    options.ApiKeys = configuration.GetSection("ApiKeys").Get<string[]>() ?? throw new ArgumentNullException("Apikeys");
                });

            return services;
        }
    }
}
