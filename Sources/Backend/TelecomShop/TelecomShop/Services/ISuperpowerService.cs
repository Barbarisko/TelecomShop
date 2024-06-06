using TelecomShop.DBModels;

namespace TelecomShop.Services
{
    public interface ISuperpowerService
    {
        public IEnumerable<Product> GetAll(int planId);
        public void DisconnectAddon(int addonId);

        public void AddAddonToPlan(int addonId, int userId, string? extendedChars);


    }
}
