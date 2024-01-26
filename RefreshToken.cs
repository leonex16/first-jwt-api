namespace FirstWebApi;

public class RefreshToken
{
  public string Value { get; set; }
  public DateTime Expires { get; set; }
  public DateTime CreatedAt { get; set; } = DateTime.Now;

  public bool IsExpired => Expires < DateTime.Now;

  public RefreshToken(string token, DateTime expires)
  {
    Value = token;
    Expires = expires;
  }

  public RefreshToken(RefreshToken refreshToken)
  {
    Value = refreshToken.Value;
    Expires = refreshToken.Expires;
    CreatedAt = refreshToken.CreatedAt;
  }
}
