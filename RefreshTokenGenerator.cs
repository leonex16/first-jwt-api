using System.Security.Cryptography;

namespace FirstWebApi;


public static class RefreshTokenGenerator
{
  public static RefreshToken Generate()
  {
    return new RefreshToken(
      token: BuildRefreshToken(),
      expires: DateTime.Now.AddDays(1)
    );
  }

  private static string BuildRefreshToken()
  {
    var bytes = new List<byte[]>
    {
      RandomNumberGenerator.GetBytes(32),
      RandomNumberGenerator.GetBytes(32),
      RandomNumberGenerator.GetBytes(32)
    };

    var hexBytes = bytes
      .Select(Convert.ToBase64String);

    return string.Join(".", hexBytes);
  }
}
