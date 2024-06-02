using System.Security.Claims;
using TelecomShop.Models;

namespace TelecomShop.Controllers
{
    public static class ClaimsHelper
    {
        public static string Username(IEnumerable<Claim> claims)
        {
            return claims.First((Claim claim) =>
            {
                return claim.Type == ClaimTypes.Name;
            }).Value;
        }
        public static string PhoneNumber(IEnumerable<Claim> claims)
        {
            return claims.First((Claim claim) =>
            {
                return claim.Type == ClaimTypes.MobilePhone;
            }).Value;
        }

        public static int ID(IEnumerable<Claim> claims)
        {
            return Convert.ToInt32(claims.First((Claim claim) =>
            {
                return claim.Type == ClaimTypes.NameIdentifier;
            }).Value);
        }
    }
}
