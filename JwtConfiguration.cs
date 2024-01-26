using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace FirstWebApi;

public sealed class JwtConfiguration
{
  public readonly string SecretKey;
  public readonly double ExpireMinutes;
  public readonly string Audience;
  public readonly string Issuer;

  public JwtConfiguration(IConfiguration configuration)
  {
    SecretKey = configuration["Jwt:SecretKey"];
    ExpireMinutes = Convert.ToDouble(configuration["Jwt:ExpireMinutes"]!);
    Audience = configuration["Jwt:AudienceToken"]!;
    Issuer = configuration["Jwt:IssuerToken"]!;
  }
}

public static class JwtConfigurationService
{
  public static void SetJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
  {
    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
    {
      var jwtConfiguration = new JwtConfiguration(configuration);
      options.TokenValidationParameters = new TokenValidationParameters
      {
        // TODO: Check RequireAudience - ValidateAudience - ValidateIssuer
        RequireAudience = false,
        ValidateAudience = false,
        ValidateIssuer = false,
        RequireExpirationTime = false,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfiguration.Issuer,
        ValidAudience = jwtConfiguration.Audience,
        IssuerSigningKey = TokenGenerator.SecurityKey(jwtConfiguration)
      };
    });
  }
}
