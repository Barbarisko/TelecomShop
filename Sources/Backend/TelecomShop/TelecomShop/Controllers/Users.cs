using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.FrontendModels;
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
            var productInfo = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId && info.ParentProductId==null && info.Status=="Active");

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
                OneTimeTotal = productInfo.OneTimeTotal ?? 0,
                RecurrentTotal = productInfo.OneTimeTotal ?? 0
            };
        }
        [HttpGet]
        public void CalculateActivePlanPrice(ActiveProduct activeProduct)
        {
            var plan = _unitOfWork.ProductRepo.Get(activeProduct.ProductId ?? 0);

            if (activeProduct.OneTimeTotal == 0 || activeProduct.RecurrentTotal == 0)
            {
                activeProduct.OneTimeTotal = plan.PriceOneTime;
                activeProduct.RecurrentTotal = plan.PriceRecurrent;
            }

            var activeAddons = _unitOfWork.ActiveProductRepo.GetAll().Where(x => x.ParentProductId == activeProduct.ProductId);

            foreach (var activeAddon in activeAddons)
            {
                activeProduct.OneTimeTotal += activeAddon.OneTimeTotal;
                activeProduct.RecurrentTotal += activeAddon.RecurrentTotal;
            }
        }

        [HttpGet]
        public List<UsageStats> GetStatistics(int cpiId)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            //var productInfo = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);

            var usageInfo = _unitOfWork.UsageRepo.GetAll().Where((u)=>u.UserId==userId && u.ActiveProductId==cpiId);

            var statsForFrontend = new List<UsageStats>();

            foreach (var usage in usageInfo) {
                statsForFrontend.Add(new UsageStats()
                {
                    DateStart = new DateTime(usage.DateStart ?? DateOnly.FromDateTime(DateTime.Now), TimeOnly.MinValue),
                    DateEnd = new DateTime(usage.DateEnd ?? DateOnly.FromDateTime(DateTime.Now), TimeOnly.MinValue),
                    DataUsed = usage.DataUsed,
                    SmsUsed = usage.SmsUsed,
                    VoiceUsed = usage.VoiceUsed,
                    MoneySpent = usage.MoneySpent
                });
            }

            return statsForFrontend;
        }

        [HttpGet]
        public string GenerateFileStatistics(int cpiId)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            //var productInfo = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);

            var usageInfo = _unitOfWork.UsageRepo.GetAll().Where((u) => u.UserId == userId && u.ActiveProductId == cpiId).ToList();

            //var totalDataUsed = usageInfo.Sum(r => r.);
            //var averageDailyUsage = records.Average(r => r.DataUsedInMB);
            //var maximumDailyUsage = records.Max(r => r.DataUsedInMB);
            //var minimumDailyUsage = records.Min(r => r.DataUsedInMB);

            //return new DataUsageStatistics
            //{
            //    TotalDataUsed = totalDataUsed,
            //    AverageDailyUsage = averageDailyUsage,
            //    MaximumDailyUsage = maximumDailyUsage,
            //    MinimumDailyUsage = minimumDailyUsage
            //};

            //using (var writer = new StreamWriter(filePath))
            //{
            //    writer.WriteLine("Statistic,Value");
            //    writer.WriteLine($"Total Data Used (MB),{statistics.TotalDataUsed}");
            //    writer.WriteLine($"Average Daily Usage (MB),{statistics.AverageDailyUsage}");
            //    writer.WriteLine($"Maximum Daily Usage (MB),{statistics.MaximumDailyUsage}");
            //    writer.WriteLine($"Minimum Daily Usage (MB),{statistics.MinimumDailyUsage}");
            //}

            //string filePath = "DataUsageStatistics.csv";
            //fileService.WriteStatisticsToFile(statistics, filePath);

            return "stats";
        }
    }
}
