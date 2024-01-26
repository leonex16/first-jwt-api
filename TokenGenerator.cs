using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FirstWebApi;

public class TokenGenerator
{
  private readonly IConfiguration _configuration;
  private readonly List<Claim> _claims = new();

  public TokenGenerator(IConfiguration configuration)
  {
    _configuration = configuration;
  }

  public static SecurityKey SecurityKey(JwtConfiguration jwtConfiguration)
  {
    var byteSecretKey = Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey);
    return new SymmetricSecurityKey(byteSecretKey);
  }

  private static SecurityKey SecurityKey(IConfiguration configuration)
  {
    var jwtConfiguration = new JwtConfiguration(configuration);
    var byteSecretKey = Encoding.UTF8.GetBytes(jwtConfiguration.SecretKey);
    return new SymmetricSecurityKey(byteSecretKey);
  }


  // TODO: Generic Constrains
  public TokenGenerator BuildClaims<T>(T entity) where T : User
  {
    var type = entity.GetType();
    var properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance |
                                        BindingFlags.GetProperty);

    // TODO
    // foreach (var property in properties)
    // {
    //   _claims.Add(new Claim(property.Name, property.GetValue(entity)!.ToString() ?? string.Empty));
    // }

    _claims.Add(new Claim("Id", "1a82bfcd-f096-4e7a-b89b-8e6240bf42b6"));
    _claims.Add(new Claim("Name", entity.Name));
    _claims.Add(new Claim("LastName", entity.LastName));
    _claims.Add(new Claim("Email", entity.Email));

    return this;
  }

  public Token Generate()
  {
    var jwtConfiguration = new JwtConfiguration(_configuration);
    var signingCredentials = BuildSigningCredentials();
    var jwtSecurityToken = BuildJwtSecurityToken(signingCredentials, jwtConfiguration);

    return new Token(
      token: GetToken(jwtSecurityToken),
      expires: DateTimeOffset.FromUnixTimeSeconds((long)jwtSecurityToken.Payload.Exp!).LocalDateTime,
      refreshToken: RefreshTokenGenerator.Generate()
      );
  }

  private SigningCredentials BuildSigningCredentials()
  {
    var securityKey = SecurityKey(_configuration);
    return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
  }

  private JwtSecurityToken BuildJwtSecurityToken(SigningCredentials signingCredentials,
    JwtConfiguration jwtConfiguration)
  {
    return new JwtSecurityToken(
        issuer: jwtConfiguration.Issuer,
        audience: jwtConfiguration.Audience,
        notBefore: DateTime.Now,
        expires: DateTime.Now.AddMilliseconds(jwtConfiguration.ExpireMinutes),
        signingCredentials: signingCredentials,
        claims: _claims)
      ;
  }

  private string GetToken(SecurityToken jwtSecurityToken)
  {
    var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    return jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
  }
}

public class Entity
{
}

public class User : Entity
{
  public readonly string Name;
  public readonly string LastName;
  public readonly string Email;

  public User(string name, string lastName, string email)
  {
    Name = name;
    LastName = lastName;
    Email = email;
  }
}
