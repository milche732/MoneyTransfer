using Moneybox.Domain.DataAccess;
using Moneybox.Domain.Domain.Services;
using System;

namespace Moneybox.Domain.Features
{
    public class WithdrawMoney : IWithdrawMoney
    {
        private IAccountRepository accountRepository;

        public WithdrawMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public bool Execute(Guid fromAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            if (from.CanWithdraw(amount))
            {
                from.WithdrawMoney(amount);
                this.accountRepository.Update(from);
                return true;
            }

            return false;
        }
    }
}
