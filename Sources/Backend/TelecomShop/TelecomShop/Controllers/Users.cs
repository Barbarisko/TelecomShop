using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.UnitOfWork;


namespace TelecomShop.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Users : ControllerBase
    {

        private readonly ILogger<Auth> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public Users(ILogger<Auth> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public UserStats GetStats()
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            var billinfInfo = _unitOfWork.BillingAccountRepo.GetAll().First((info) => info.UserId == userId);
            var productInfo = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);
 
            if (billinfInfo == null || productInfo == null)
            {
                throw new BadRequestException("could not find user stats!");
            }
            return new UserStats
            {
                Balance = billinfInfo.Balance ?? 0,
                InternetBalance = productInfo.DataLeft ?? 0,
                InternetLimit = productInfo.DataAmount ?? 0,
                PhoneNumber = user.Msisdn,
                SmsBalance = productInfo.SmsLeft ?? 0,
                SmsLimit = productInfo.SmsAmount ?? 0,
                VoiceBalance = productInfo.VoiceAmount ?? 0,
                VoiceLimit = productInfo.VoiceLeft ?? 0,
                Name = user.Name ?? "",
                Surname = user.Surname ?? ""
            };
        }
    }
}
