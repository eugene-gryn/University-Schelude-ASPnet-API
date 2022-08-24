namespace DAL.Entities;

public class Tokens {
    public string Token { get; set; } = String.Empty;
    public string RefreshToken { get; set; } = String.Empty;

    public DateTime TokenCreated { get; set; } = DateTime.UtcNow;
    public DateTime TokenExpires { get; set; } = DateTime.UtcNow;


}