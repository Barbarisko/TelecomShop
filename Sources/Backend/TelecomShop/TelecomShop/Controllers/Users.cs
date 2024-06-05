using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TelecomShop.DBModels;
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

            CalculateActivePlanPrice(productInfo);

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
                Surname = user.Surname ?? "", 
                OneTimeTotal = productInfo.PriceOneTimeTotal ?? 0,
                RecurrentTotal = productInfo.PriceOneTimeTotal ?? 0
            };
        }
        [HttpGet]
        public void CalculateActivePlanPrice(ActiveProduct activeProduct)
        {
            var plan = _unitOfWork.ProductRepo.Get(activeProduct.ProductId ?? 0);

            if (activeProduct.PriceOneTimeTotal == 0 || activeProduct.PriceRecurrentTotal == 0)
            {
                activeProduct.PriceOneTimeTotal = plan.PriceOneTime;
                activeProduct.PriceRecurrentTotal = plan.PriceRecurrent;
            }

            var activeAddons = _unitOfWork.ActiveProductRepo.GetAll().Where(x => x.ParentProductId == activeProduct.ProductId);

            foreach (var activeAddon in activeAddons)
            {
                activeProduct.PriceOneTimeTotal += activeAddon.PriceOneTimeTotal;
                activeProduct.PriceRecurrentTotal += activeAddon.PriceRecurrentTotal;
            }
        }

        [HttpGet]
        public void GenerateStatistics(int cpiId)
        {
            var plan = _unitOfWork.ProductRepo.Get(activeProduct.ProductId ?? 0);

            if (activeProduct.PriceOneTimeTotal == 0 || activeProduct.PriceRecurrentTotal == 0)
            {
                activeProduct.PriceOneTimeTotal = plan.PriceOneTime;
                activeProduct.PriceRecurrentTotal = plan.PriceRecurrent;
            }

            var activeAddons = _unitOfWork.ActiveProductRepo.GetAll().Where(x => x.ParentProductId == activeProduct.ProductId);

            foreach (var activeAddon in activeAddons)
            {
                activeProduct.PriceOneTimeTotal += activeAddon.PriceOneTimeTotal;
                activeProduct.PriceRecurrentTotal += activeAddon.PriceRecurrentTotal;
            }
        }
    }
}
