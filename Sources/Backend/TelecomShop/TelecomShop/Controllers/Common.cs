using System.Security.Claims;
using TelecomShop.DBModels;
using TelecomShop.Models;
using TelecomShop.Repository;

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
    public class Common { 
        public static ActiveProduct? GetCurrentActivePlan(int userId, IRepository<ActiveProduct> productRepo)
        {
            return productRepo.GetAll().FirstOrDefault((info) =>
            {
                return info.UserId == userId
                    && info.ParentProductId == null
                    && info.Status == "Active";
            }, null);

        }
    }
}
