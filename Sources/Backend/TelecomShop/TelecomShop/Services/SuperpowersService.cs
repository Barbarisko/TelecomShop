using TelecomShop.Controllers;
using TelecomShop.DBModels;
using TelecomShop.Models;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Services
{
    public class SuperpowersService: ISuperpowerService
    {
        private readonly IUnitOfWork unitOfWork;

        public SuperpowersService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<Product> GetAll(int planId)
        {
            return unitOfWork.ProductRepo.GetAll()
                .Where((productOption) =>
                {
                    return (productOption.ActiveFrom <= DateTime.Now
                            || productOption.ActiveFrom == null)
                            && (productOption.ActiveTo >= DateTime.Now
                            || productOption.ActiveTo == null)
                            && productOption.ParentId == planId;
                });
        }
       
        public void DisconnectAddon(int addonId)
        {
            var activeAddon = unitOfWork.ActiveProductRepo.Get(addonId);
            if (activeAddon == null)
            {
                throw new KeyNotFoundException();
            }

            if(activeAddon.ParentProductId != null) {
                throw new Exception("This is not an addon");
            }

            unitOfWork.ActiveProductRepo.Delete(addonId);

            var activePlan = unitOfWork.ActiveProductRepo.Get(activeAddon.ParentProductId ?? 0);
            var addon = unitOfWork.ProductRepo.Get(activeAddon.ProductId ?? 0);

            activePlan.PriceOneTimeTotal -= addon.PriceOneTime;
            activePlan.PriceRecurrentTotal -= addon.PriceRecurrent;

            unitOfWork.ActiveProductRepo.Update(activePlan);

            unitOfWork.Save();
        }

        public void AddAddonToPlan(int addonId, int planId, string? extendedChars)
        {
            var activePlan = unitOfWork.ActiveProductRepo.Get(planId);

            var addon = unitOfWork.ProductRepo.Get(addonId);

            if (unitOfWork.ActiveProductRepo.Get(addonId) != null)
            {
                throw new InvalidOperationException("this addon is already added");
            }
            else
            {
                var activeAddon = new ActiveProduct
                {
                    UserId = activePlan.UserId,
                    ProductId = addon.Id,
                    ParentProductId = activePlan.Id,
                    ContractTerm = activePlan.ContractTerm,
                    ExtendedChars = extendedChars,
                    BillingAccountId = activePlan.BillingAccountId,
                    PurchaceDate = DateTime.Now,
                    PriceOneTimeTotal = addon.PriceOneTime,
                    PriceRecurrentTotal = addon.PriceRecurrent
                };

                var plan = unitOfWork.ProductRepo.Get(activePlan.ProductId ?? 0);

                activePlan.PriceOneTimeTotal = plan.PriceOneTime + addon.PriceOneTime;
                activePlan.PriceRecurrentTotal = plan.PriceRecurrent + addon.PriceRecurrent;

                var BA = unitOfWork.BillingAccountRepo.Get(activePlan.BillingAccountId ?? 0);
                BA.Balance -= addon.PriceOneTime;

                unitOfWork.ActiveProductRepo.Add(activeAddon);
                unitOfWork.ActiveProductRepo.Update(activePlan);
                unitOfWork.BillingAccountRepo.Update(BA);
            }
            Console.WriteLine(activePlan.PriceOneTimeTotal);
            unitOfWork.Save();
        }

    }
}
