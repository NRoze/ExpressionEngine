namespace ExpressionEngine.Api.Extensions
{
    static public class AppBuilderExtensions
    {
        extension(WebApplicationBuilder builder)
        {
            public IServiceCollection AddCors(string policyName)
            {
                return builder.Services.AddCors(options =>
                {
                    var allowedOrigins = builder.Configuration
                        .GetSection("AllowedOrigins").Get<string[]>() ?? [];

                    options.AddPolicy(policyName, policy =>
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
                });
            }
        }
    }
}
