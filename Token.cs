namespace FirstWebApi;

public class Token : RefreshToken
{
  private readonly RefreshToken _refreshToken;


  public Token(string token, DateTime expires, RefreshToken refreshToken) : base(refreshToken)
  {
    Value = token;
    Expires = expires;
    _refreshToken = refreshToken;
  }

  public RefreshToken getRefreshToken() => _refreshToken;
}
