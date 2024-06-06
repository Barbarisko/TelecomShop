using System;
using System.Collections.Generic;
using System.Text;
using TelecomShop.DBModels;
using TelecomShop.Repository;

namespace TelecomShop.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepository<ActiveProduct> ActiveProductRepo { get; }
        public IRepository<BillingAccount> BillingAccountRepo { get; }
        public IRepository<Characteristic> CharacteristicRepo { get; }
        public IRepository<CharInvolvement> CharInvolvementRepo { get; }
        public IRepository<Product> ProductRepo { get; }
        public IRepository<User> UserRepo { get; }
        public IRepository<MonthlyUsage> UsageRepo { get; }

        void Save();

    }
}
