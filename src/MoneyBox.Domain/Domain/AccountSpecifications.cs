using Moneybox.Domain.SeedWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain
{
    public static class AccountSpecifications
    {
        public static Specification<Account> balanceThresold = new Specification<Account>(acc => acc.Balance >= 500);
    }
}
