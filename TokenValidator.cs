using System.Security.Claims;

namespace FirstWebApi;

public static class TokenValidator
{
  public static bool Validate(ClaimsIdentity? claimsIdentity)
  {
    var userId = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;
    var expired = claimsIdentity?.Claims.FirstOrDefault(claim => claim.Type == "Id")?.Value;

    // TODO: Find User

    return userId is "1a82bfcd-f096-4e7a-b89b-8e6240bf42b6";
  }
}
