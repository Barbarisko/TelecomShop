using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using OfficeOpenXml;
using System.Data;
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
        public List<UsageStats> GetStatistics(DateTime dateStart, DateTime dateEnd)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);

            var activePlan = Common.GetCurrentActivePlan(userId, _unitOfWork.ActiveProductRepo);

            var usageInfo = _unitOfWork.UsageRepo.GetAll().Where((u) =>
                                                           u.UserId == userId 
                                                           && u.ActiveProductId == activePlan.Id
                                                           && (u.Date >= DateOnly.FromDateTime(dateStart) 
                                                           || u.Date <= DateOnly.FromDateTime(dateEnd)));

            var statsForFrontend = new List<UsageStats>();

            foreach (var usage in usageInfo)
            {
                statsForFrontend.Add(new UsageStats()
                {
                    Date = new DateTime(usage.Date ?? DateOnly.FromDateTime(DateTime.Now), TimeOnly.MinValue),
                    DataUsed = usage.DataUsed,
                    SmsUsed = usage.SmsUsed,
                    VoiceUsed = usage.VoiceUsed,
                    MoneySpent = usage.MoneySpent
                });
            }

            return statsForFrontend;
        }

        [HttpGet]
        public void GenerateFileStatistics(DateTime dateStart, DateTime dateEnd)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);

            var activePlan = Common.GetCurrentActivePlan(userId, _unitOfWork.ActiveProductRepo);

            var usageInfo = _unitOfWork.UsageRepo.GetAll().Where((u) =>
                                                           u.UserId == userId
                                                           && u.ActiveProductId == activePlan.Id
                                                           && (u.Date >= DateOnly.FromDateTime(dateStart)
                                                           || u.Date <= DateOnly.FromDateTime(dateEnd))).ToArray();
            
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage())
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Statistics of usage");

                // Get the dimensions of the array
                int rows = usageInfo.Count();

                worksheet.Cells[1, 1].Value = "Date";
                worksheet.Cells[1, 2].Value = "Data Used";
                worksheet.Cells[1, 3].Value = "Voice Used";
                worksheet.Cells[1, 4].Value = "SMS Used";
                worksheet.Cells[1, 5].Value = "Money Spent";


                // Load data from array to worksheet
                for (int row = 1; row <= rows; row++)
                {
                    var col = 1;
                   
                    worksheet.Cells[row + 1, col ++].Value = usageInfo[row-1].Date;
                    worksheet.Cells[row + 1, col ++].Value = usageInfo[row-1].DataUsed;
                    worksheet.Cells[row + 1, col ++].Value = usageInfo[row-1].VoiceUsed;
                    worksheet.Cells[row + 1, col ++].Value = usageInfo[row-1].SmsUsed;
                    worksheet.Cells[row + 1, col ++].Value = usageInfo[row-1].MoneySpent;
                    
                }


                // Save the Excel file
                string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads", "UserReport" + DateTime.Now.ToString("yyyy MMM dd hh-mm") + ".xlsx");
                //string downloadsPath = Path.Combine(System.IO.Directory.GetCurrentDirectory());

                FileInfo fileInfo = new FileInfo(downloadsPath);
                package.SaveAs(downloadsPath);

            }

        }
    }
}
