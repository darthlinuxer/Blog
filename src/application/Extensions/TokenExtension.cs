namespace Application.Extensions;

public class TokenExtensions
{
    public static string Issuer { get; set; } = "BlogAPI";
    public static string Audience { get; set; } = "BlogUser";
    public static string SecretKey { get; set; } = "TodoMustHideThisLongSecretKeySomewhereLater";

    public static bool ValidateJwtToken(string token, out ClaimsPrincipal principal)
    {
        principal = null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey); // Replace with your secret key
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = Issuer, // Replace with your valid issuer
            ValidateAudience = true,
            ValidAudience = Audience,
            ValidateLifetime = true // This checks that the token is not expired
        };

        try
        {
            principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }

    public static string CreateToken(BlogUser user, IList<string> roles)
    {
        var claims = new List<Claim>{
            new Claim(ClaimTypes.Sid, user.Id),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.UserName)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}