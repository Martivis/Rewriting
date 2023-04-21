namespace Rewriting.WebApp;

public class TokenModel
{
    public string Token { get; set; }
    public DateTime ExpireDate { get; set; }

    public TokenModel(string token, int expiresInSeconds)
    {
        Token = token;
        ExpireDate = DateTime.UtcNow.AddSeconds(expiresInSeconds);
    }

    public TokenModel()
    {
    }
}
