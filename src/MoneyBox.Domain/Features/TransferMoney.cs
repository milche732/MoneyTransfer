using Moneybox.Domain.DataAccess;
using Moneybox.Domain.Domain.Services;
using System;

namespace Moneybox.Domain.Features
{
    public class TransferMoney : ITransferMoney
    {
        private IAccountRepository accountRepository;

        public TransferMoney(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public bool Execute(Guid fromAccountId, Guid toAccountId, decimal amount)
        {
            var from = this.accountRepository.GetAccountById(fromAccountId);
            var to = this.accountRepository.GetAccountById(toAccountId);

            if (from.CanWithdraw(amount) && to.CanPaidIn(amount))
            {
                to.TransferMoney(from, amount);
                this.accountRepository.Update(from);
                this.accountRepository.Update(to);
                return true;
            }

            return false;
        }
    }
}
