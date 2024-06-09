using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.Services;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Controllers
{
    public struct ConnectSuperpowerData
    {
        public int addonId { get; set; }
        public string? extendedChars { get; set; }
    }

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class Superpowers : ControllerBase
    {

        private readonly ILogger<Auth> _logger;
        private readonly IUnitOfWork unitOfWork;

        private ISuperpowerService superpowerService;

        public Superpowers(ILogger<Auth> logger, ISuperpowerService superpowerService, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            this.superpowerService = superpowerService;
            this.unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet]
        public List<Superpower> GetAll()
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var user = unitOfWork.UserRepo.Get(userId);
            var cpi = Common.GetCurrentActivePlan(userId, unitOfWork.ActiveProductRepo);
            var product = unitOfWork.ProductRepo.Get(cpi.ProductId ?? 0);

            var addons = superpowerService.GetAll(product.Id);
            var superpowers = new List<Superpower>();

            foreach (var addon in addons)
            {

                var charInvolvements = unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == addon.Id);

                var charsForFrontend = new Dictionary<string, string>();
                var charsListValues = new Dictionary<string, string>();

                foreach (var charInv in charInvolvements)
                {
                    charsForFrontend.Add(unitOfWork.CharacteristicRepo
                                    .Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);
                    if (charInv.ListValues != null)
                        charsListValues.Add(unitOfWork.CharacteristicRepo
                                       .Get(charInv.CharId ?? 0).Name, charInv.ListValues);
                }

                var isActive = unitOfWork.ActiveProductRepo.GetAll().Count((info) =>
                {
                    return info.UserId == userId
                        && info.ProductId == addon.Id
                        && info.ParentProductId != null
                        && info.Status == "Active";
                }) > 0;

                superpowers.Add(new Superpower
                {
                    Id = addon.Id,
                    Type = addon.Type,
                    IsActive = isActive,
                    Name = addon.Name,
                    Description = addon.Description,
                    PriceOneTimeTotal = addon.PriceOneTime,
                    PriceRecurrentTotal = addon.PriceRecurrent,
                    Characteristics = charsForFrontend,
                    CharacteristicListValues = charsListValues
                });
            }


            return superpowers;
        }



        [Authorize]
        [HttpGet]
        public List<Superpower> GetCurrent()
        {
            var userId = ClaimsHelper.ID(User.Claims);
            var cpi = Common.GetCurrentActivePlan(userId, unitOfWork.ActiveProductRepo);

            var superpowers = new List<Superpower>();


            var addons = unitOfWork.ActiveProductRepo.GetAll().Where((a) => a.ParentProductId == cpi.Id);

            foreach (var addon in addons)
            {

                var charInvolvements = unitOfWork.CharInvolvementRepo.GetAll().Where((info) => info.ProductId == addon.ProductId);

                var charsForFrontend = new Dictionary<string, string>();
                var charsListValues = new Dictionary<string, string>();

                foreach (var charInv in charInvolvements)
                {
                    charsForFrontend.Add(unitOfWork.CharacteristicRepo
                                    .Get(charInv.CharId ?? 0).Name, charInv.DefaultValue);
                    if (charInv.ListValues != null)
                        charsListValues.Add(unitOfWork.CharacteristicRepo
                                       .Get(charInv.CharId ?? 0).Name, charInv.ListValues);
                }
                var product = unitOfWork.ProductRepo.Get(addon.ProductId ?? 0);

                superpowers.Add(new Superpower
                {
                    Name = product.Name,
                    Description = product.Description,
                    PriceOneTimeTotal = addon.OneTimeTotal,
                    PriceRecurrentTotal = addon.RecurrentTotal,
                    Characteristics = charsForFrontend,
                    CharacteristicListValues = charsListValues
                });
            }

            return superpowers;
        }

        [HttpPost]
        public bool DisconnectSuperpower([Required]int addonId)
        {
            superpowerService.DisconnectAddon(addonId);
            return true;
        }


        [HttpPost]
        public bool ConnectSuperpower([Required][FromBody] ConnectSuperpowerData data)
        {
            var userId = ClaimsHelper.ID(User.Claims);
            superpowerService.AddAddonToPlan(data.addonId, userId, data.extendedChars);

            unitOfWork.Save();
            return true;
        }


    }
}
