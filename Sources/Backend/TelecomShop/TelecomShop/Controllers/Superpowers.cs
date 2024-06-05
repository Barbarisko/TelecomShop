using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TelecomShop.ErrorHandlers;
using TelecomShop.Models;
using TelecomShop.Services;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Controllers
{
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
            var cpi = unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);
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

                superpowers.Add(new Superpower
                {
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
            var user = unitOfWork.UserRepo.Get(userId);
            var cpi = unitOfWork.ActiveProductRepo.GetAll().First((info) => info.UserId == userId);

            var superpowers = new List<Superpower>();


            var addons = unitOfWork.ActiveProductRepo.GetAll().Where((a)=> a.ParentProductId == cpi.Id);

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
                    PriceOneTimeTotal = addon.PriceOneTimeTotal,
                    PriceRecurrentTotal = addon.PriceRecurrentTotal,
                    Characteristics = charsForFrontend,
                    CharacteristicListValues = charsListValues
                });
            }

            return superpowers;
           // return [new Superpower { Name = "1" }, new Superpower { Name = "2" }];
        }

        [Authorize]
        [HttpDelete]
        public void DisconnectSuperpower(int addonId)
        {
           superpowerService.DisconnectAddon(addonId);
        }


        [Authorize]
        [HttpPost]
        public Plan ConnectSuperpower(int addonId, int planId, string? extendedChars)
        {
           superpowerService.AddAddonToPlan(addonId, planId, extendedChars);

            Plans p = new Plans(_logger, unitOfWork);

            return p.GetCurrent();
        }


    }
}
