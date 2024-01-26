using Microsoft.AspNetCore.Mvc;

namespace FirstWebApi.Controllers;

[ApiController]
[Route("authorization")]
public class AuthorizationController : ControllerBase
{
  private readonly IConfiguration _configuration;

  public AuthorizationController(IConfiguration configuration)
  {
    _configuration = configuration;
  }


  [HttpGet]
  [Route("")]
  public Token GetToken()
  {
    var tokenGenerator = new TokenGenerator(_configuration);
    var user = new User(
      name: "User Name",
      lastName: "User Last Name",
      email: "User Email"
      );

    var token = tokenGenerator
      .BuildClaims(user)
      .Generate();

    SetRefreshToken(token.getRefreshToken());

    return token;
  }

  [HttpGet]
  [Route("refresh")]
  public Token? RefreshToken()
  {
    var refreshTokenValue = Request.Cookies["refresh-token"];

    if (refreshTokenValue == null) return null;

    // TODO: Bind RefreshToken to the User

    var refreshTokenUser =
      "YU07r6QUqV9HKFaw0GqRSqLI2h00SkgjpQF4CUWYPCU=.LYkIhGwUmVElU5hGN8nm0qDn7UhLyF6Cs2c2fkEcCro=.bhYZxachr+GHle7Td9j2DXQ5qrB0J18mYufK/oQZuXU=";

    // if (refreshTokenValue != refreshTokenUser) return null;

    // Same code of the GetToken method
    var tokenGenerator = new TokenGenerator(_configuration);
    var user = new User(
      name: "User Name",
      lastName: "User Last Name",
      email: "User Email"
    );

    return tokenGenerator
      .BuildClaims(user)
      .Generate();
  }

  private void SetRefreshToken(RefreshToken refreshToken)
  {
    var cookieOptions = new CookieOptions
    {
      HttpOnly = true,
      Expires = refreshToken.Expires
    };

    Response.Cookies.Append("refresh-token", refreshToken.Value, cookieOptions);
  }
}
