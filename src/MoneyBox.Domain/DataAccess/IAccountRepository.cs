using System;

namespace Moneybox.Domain.DataAccess
{
    public interface IAccountRepository
    {
        Account GetAccountById(Guid accountId);

        void Update(Account account);
    }
}
