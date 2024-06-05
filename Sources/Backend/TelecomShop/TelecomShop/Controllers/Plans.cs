using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Xml.Linq;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.Services;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Controllers
{
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
            var cpi = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);
            var product = _unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0);

            var upgradeOptions = new List<int>();

            // Iterate through the array and print each element
            foreach (string element in product.UpgradeOptions.Split(','))
            {
                if (element != null)
                    upgradeOptions.Add(Convert.ToInt16(element));
            }

            var downgradeOptions = new List<int>();

            // Iterate through the array and print each element
            foreach (string element in product.DowngradeOptions.Split(','))
            {
                if (element!= null)
                    downgradeOptions.Add(Convert.ToInt16(element));
            }


            var availableOptions = _unitOfWork.ProductRepo.GetAll()
                .Where((productOption) =>
                {
                    return (productOption.ActiveFrom <= DateTime.Now
                            || productOption.ActiveFrom == null)
                            && (productOption.ActiveTo >= DateTime.Now
                            || productOption.ActiveTo == null)
                            && productOption.ParentId == null
                            && (upgradeOptions.Contains(productOption.Id)
                            || downgradeOptions.Contains(product.Id));
                        });

            var plans = new List<Plan>();
            

            foreach(var availableOption in availableOptions) {

                var charInvolvements = _unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == product.Id);

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
            var cpi = _unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);
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
               Name = product.Name,
               Description = product.Description,
               PriceRecurrentTotal = product.PriceRecurrent,
               PriceOneTimeTotal = product.PriceOneTime,
               Characteristics = characteristics
            };
        }
    }
}
