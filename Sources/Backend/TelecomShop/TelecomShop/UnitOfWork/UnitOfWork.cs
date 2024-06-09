using TelecomShop.DBModels;
using TelecomShop.Repository;

namespace TelecomShop.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TelcoShopDBContext _context;

        public IRepository<ActiveProduct> ActiveProductRepo { get; }
        public IRepository<BillingAccount> BillingAccountRepo { get; }
        public IRepository<Characteristic> CharacteristicRepo { get; }
        public IRepository<CharInvolvement> CharInvolvementRepo { get; }
        public IRepository<Product> ProductRepo { get; }
        public IRepository<User> UserRepo { get; }
        public IRepository<Usage> UsageRepo { get; }


        public UnitOfWork(TelcoShopDBContext context,
            IRepository<ActiveProduct> activeProductRepo,
            IRepository<BillingAccount> billingAccountRepo,
            IRepository<Characteristic> characteristicRepo,
            IRepository<CharInvolvement> charInvolvementRepo,
            IRepository<Product> productRepo,
            IRepository<User> userRepo
,
            IRepository<Usage> usageRepo)
        {
            _context = context;
            ActiveProductRepo = activeProductRepo;
            BillingAccountRepo = billingAccountRepo;
            CharacteristicRepo = characteristicRepo;
            CharInvolvementRepo = charInvolvementRepo;
            ProductRepo = productRepo;
            UserRepo = userRepo;
            UsageRepo = usageRepo;
        }
        public void Save()
        {
            _context.SaveChanges();
        }

        //public void Dispose()
        //{
        //    Save();
        //}
    }
}
