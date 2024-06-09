using TelecomShop.Controllers;
using TelecomShop.DBModels;
using TelecomShop.ErrorHandlers;
using TelecomShop.UnitOfWork;

namespace TelecomShop.Services
{
    public class SuperpowersService : ISuperpowerService
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
                    return (productOption.ActiveFrom <= DateOnly.FromDateTime(DateTime.Now)
                            || productOption.ActiveFrom == null)
                            && (productOption.ActiveTo >= DateOnly.FromDateTime(DateTime.Now)
                            || productOption.ActiveTo == null)
                            && productOption.ParentId == planId
                            ;
                });
        }

        public void DisconnectAddon(int addonId)
        {
            // TODO: Change Status to disconnected
            var activeAddon = unitOfWork.ActiveProductRepo.GetAll().Where((p) => p.ProductId == addonId && p.ParentProductId!=null&&p.Status=="Active").First();
            if (activeAddon == null)
            {
                throw new KeyNotFoundException();
            }

            if (activeAddon.ParentProductId == null)
            {
                throw new Exception("This is not an addon");
            }

            var activePlan = unitOfWork.ActiveProductRepo.Get(activeAddon.ParentProductId ?? 0);
            var addon = unitOfWork.ProductRepo.Get(activeAddon.ProductId ?? 0);

            activePlan.OneTimeTotal -= addon.PriceOneTime;
            activePlan.RecurrentTotal -= addon.PriceRecurrent;
            activeAddon.Status = "Disconnected";

            unitOfWork.ActiveProductRepo.Update(activePlan);
            unitOfWork.ActiveProductRepo.Update(activeAddon);

            unitOfWork.Save();
        }

        public void AddAddonToPlan(int addonId, int userId, string? extendedChars)
        {
            var activePlan = Common.GetCurrentActivePlan(userId, unitOfWork.ActiveProductRepo);
            if (activePlan == null || activePlan.ProductId == null)
                throw new BadRequestException("no valid plan");

            var addon = unitOfWork.ProductRepo.Get(addonId);

            if (unitOfWork.ActiveProductRepo.Get(addonId) != null)
                throw new InvalidOperationException("this addon is already added");

            var activeAddon = new ActiveProduct
            {
                UserId = activePlan.UserId,
                ProductId = addon.Id,
                ParentProductId = activePlan.Id,
                ContractTerm = activePlan.ContractTerm,
                ExtendedChars = extendedChars,
                BillingAccountId = activePlan.BillingAccountId,
                PurchaceDate = DateOnly.FromDateTime(DateTime.Now),
                OneTimeTotal = addon.PriceOneTime,
                RecurrentTotal = addon.PriceRecurrent,
                Status = "Active"
            };

            var plan = unitOfWork.ProductRepo.Get(activePlan.ProductId ?? 0);

            activePlan.RecurrentTotal = plan.PriceRecurrent + addon.PriceRecurrent;

            var BA = unitOfWork.BillingAccountRepo.Get(activePlan.BillingAccountId ?? 0);
            BA.Balance -= addon.PriceOneTime;

            unitOfWork.ActiveProductRepo.Add(activeAddon);

            Console.WriteLine(activePlan.OneTimeTotal);
            unitOfWork.Save();
        }
    }
}
