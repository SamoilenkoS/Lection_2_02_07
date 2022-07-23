namespace Lection_2_BL.Auth
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
        string GetClaimValueFromToken(string jwtToken, string claimsTypeToGet);
    }
}
