using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.Repository;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Controllers
{

    public struct ChangePlanParams
    {
        public int planId { get; set; }

        public Dictionary<string, string> chars { get; set; }
    }
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Plans : ControllerBase
    {
        private readonly ILogger<Auth> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public Plans(ILogger<Auth> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<Plan> GetAll()
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            var cpi = Common.GetCurrentActivePlan(userId, _unitOfWork.ActiveProductRepo);
            var product = cpi != null ? _unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0) : null;

            var availableOptions = _unitOfWork.ProductRepo.GetAll()
                .Where((productOption) =>
                {
                    return (productOption.ActiveFrom <= DateOnly.FromDateTime(DateTime.Now)
                            || productOption.ActiveFrom == null)
                            && (productOption.ActiveTo >= DateOnly.FromDateTime(DateTime.Now)
                            || productOption.ActiveTo == null)
                            && productOption.ParentId == null
                            && (product == null ? true : product.Id != productOption.Id);
                });

            //var upgradeOptions = new List<int>();
            //var downgradeOptions = new List<int>();

            //// Iterate through the array and print each element
            //foreach (var otherProduct in availableOptions)
            //{

            //    if (otherProduct == null)
            //        continue;
            //    if (product.Id == otherProduct.Id)
            //        continue;
            //    if (otherProduct.Rate > product.Rate)
            //        upgradeOptions.Add(Convert.ToInt16(product.Id));
            //    else
            //        downgradeOptions.Add(Convert.ToInt16(product.Id));
            //}

            var plans = new List<Plan>();
            foreach (var availableOption in availableOptions)
            {
                var charInvolvements = _unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == availableOption.Id);

                var charsForFrontend = new Dictionary<string, string>();
                var charsListValues = new Dictionary<string, string>();

                foreach (var charInv in charInvolvements)
                {
                    charsForFrontend.Add(_unitOfWork.CharacteristicRepo
                                    .Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);
                    if (charInv.ListValues != null)
                        charsListValues.Add(_unitOfWork.CharacteristicRepo
                                       .Get(charInv.CharId ?? 0).Name, charInv.ListValues);
                }

                plans.Add(new Plan
                {
                    Id = availableOption.Id,
                    Name = availableOption.Name,
                    Description = availableOption.Description,
                    PriceOneTimeTotal = availableOption.PriceOneTime,
                    PriceRecurrentTotal = availableOption.PriceRecurrent,
                    Characteristics = charsForFrontend,
                    CharacteristicListValues = charsListValues
                });
            }

            return plans;

        }

        [Authorize]
        [HttpGet]
        public Plan GetCurrent()
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            var cpi = Common.GetCurrentActivePlan(userId, _unitOfWork.ActiveProductRepo);
            var product = _unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0);
            var charInvolvements = _unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == product.Id);

            var characteristics = new Dictionary<string, string>();

            foreach (var charInv in charInvolvements)
            {
                characteristics.Add(_unitOfWork.CharacteristicRepo.Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);
            }

            if (cpi == null)
            {
                throw new BadRequestException("could not find product info!");
            }

            return new Plan
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                PriceRecurrentTotal = product.PriceRecurrent,
                PriceOneTimeTotal = product.PriceOneTime,
                Characteristics = characteristics
            };
        }

        [Authorize]
        [HttpGet]
        public Plan SelectNewPlan(int planId)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            var cpi = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);
            var product = _unitOfWork.ProductRepo.Get(planId);

            var charInvolvements = _unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == product.Id);

            var characteristics = new Dictionary<string, string>();
            var charValues = new Dictionary<string, string>();

            foreach (var charInv in charInvolvements)
            {
                characteristics.Add(_unitOfWork.CharacteristicRepo.Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);

                if (charInv.ListValues != null)
                    charValues.Add(_unitOfWork.CharacteristicRepo.Get(charInv.CharId ?? 0).Name, charInv.ListValues);
            }

            if (cpi == null)
            {
                throw new BadRequestException("could not find product info!");
            }
            var ETF = CheckandAddETF(cpi, product);

            return new Plan
            {
                Id = planId,
                Name = product.Name,
                Description = product.Description,
                PriceRecurrentTotal = product.PriceRecurrent,
                PriceOneTimeTotal = product.PriceOneTime,
                ETF = ETF,
                Characteristics = characteristics,
                CharacteristicListValues = charValues
            };
        }

        [Authorize]
        [HttpPost]
        public bool ChangePlan([FromBody][Required] ChangePlanParams changePlanParams)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = _unitOfWork.UserRepo.Get(userId);
            var cpi = Common.GetCurrentActivePlan(userId, _unitOfWork.ActiveProductRepo);
            var product = _unitOfWork.ProductRepo.Get(changePlanParams.planId);

            if (cpi == null)
            {
                throw new BadRequestException("could not find old product info!");
            }

            var ETF = CheckandAddETF(cpi, product);

            cpi.Status = "Disconnected";

            var billingAccount = _unitOfWork.BillingAccountRepo.Get(cpi.BillingAccountId ?? 0);

            var currentdate = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc);
            var kind = currentdate.Kind;
            var newActiveProduct = new ActiveProduct()
            {
                UserId = user.Id,
                ProductId = changePlanParams.planId,
                BillingAccountId = billingAccount.Id,
                PurchaceDate = DateOnly.FromDateTime(DateTime.Now),
                OneTimeTotal = product.PriceOneTime,
                RecurrentTotal = product.PriceRecurrent,
                Status = "Active"
            };

            var charInvolvements = _unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == product.Id);
            foreach (var charInv in charInvolvements)
            {
                var key = _unitOfWork.CharacteristicRepo.Get(charInv.CharId ?? 0).Name;
                if (!changePlanParams.chars.ContainsKey(key))
                    continue;
                switch (key)
                {
                    case "Contract Term":
                        newActiveProduct.ContractTerm = Convert.ToInt16(changePlanParams.chars[key]);
                        break;
                    case "Data Amount":
                        newActiveProduct.DataAmount = Convert.ToInt16(changePlanParams.chars[key]);
                        break;

                    case "Voice Amount":
                        newActiveProduct.VoiceAmount = Convert.ToInt16(changePlanParams.chars[key]);
                        break;

                    case "SMS Amount":
                        newActiveProduct.SmsAmount = Convert.ToInt16(changePlanParams.chars[key]);
                        break;

                    //case "Technology":
                    //    newActiveProduct.ExtendedChars = changePlanParams.chars[key];
                    //    break;

                    default: continue;
                }
                //characteristics.Add(_unitOfWork.CharacteristicRepo.Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);
            }


            _unitOfWork.ActiveProductRepo.Add(newActiveProduct);

            billingAccount.Balance -= ETF;

            //if (billingAccount.Balance < 0)
            //    throw new Exception ("You cannot do ch")
            _unitOfWork.Save();
            return true;
        }

        private float CheckandAddETF(ActiveProduct cpi, Product product)
        {
            if (cpi.PurchaceDate == null)
                throw new BadRequestException("No purhase date, please contact administrator!");
            float ETF = 0.0f;

            var purchaceDate = new DateTime((DateOnly)cpi.PurchaceDate, TimeOnly.MinValue);
            var contractEndDate = purchaceDate.AddMonths(cpi.ContractTerm ?? 0);

            if (contractEndDate < DateTime.Now)
            {
                var monthsLeft = (contractEndDate - purchaceDate).TotalDays / 30;

                ETF += (float)(monthsLeft * cpi.RecurrentTotal);
            }

            if (_unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0).PriceRecurrent < product.PriceRecurrent)
            {
                var monthsLeft = (contractEndDate - purchaceDate).TotalDays / 30;

                ETF += (float)(monthsLeft * (product.PriceRecurrent - _unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0).PriceRecurrent));
            }
            return ETF;
        }
    }
}
