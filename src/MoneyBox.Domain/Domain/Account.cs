using Moneybox.Domain.Domain;
using Moneybox.Domain.Domain.Events;
using Moneybox.Domain.SeedWork;
using System;

namespace Moneybox.Domain
{
    public class Account : Entity, IAggregateRoot
    {
        private Guid _id;

        private User _user;

        private decimal _balance;

        private decimal _withdrawn;

        private decimal _paidIn;

        public decimal Balance => _balance;

        public decimal PaidIn => _paidIn;

        public decimal Withdrawn => _withdrawn;

        //this should be moved into decision tables or stored per account
        private static readonly decimal notificationThresold = 500m;
        private static readonly decimal payInLimit = 4000m;

        public User User => _user;
        public Guid Id => _id;

        /// <summary>
        /// In can be that in future thresold value will be setup individually per account
        /// </summary>
        public decimal NotificationThresold => notificationThresold;

        /// <summary>
        /// We can expect that later PayInLimit can be setup individually for each account
        /// </summary>
        public decimal PayInLimit => payInLimit;


        /// <summary>
        /// Ideally, this logic shoud be moved to "Decision Tables" for better flexibility
        /// </summary>
        private Specification<Account> _balanceThresold = new Specification<Account>(acc => acc.Balance < acc.NotificationThresold);
        private Specification<Account> _paidInThresold = new Specification<Account>(acc => acc.PayInLimit - acc.PaidIn < acc.NotificationThresold);

        public Account(Guid id, User user, decimal balance, decimal withdrawn, decimal paidIn)
        {
            _id = id;
            _balance = balance;
            _withdrawn = withdrawn;
            _paidIn = paidIn;
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            _user = user;
        }


        public bool CanWithdraw(decimal amount)
        {
            return _balance - amount >= 0m;
        }

        public bool CanPaidIn(decimal amount)
        {
            return this._paidIn + amount <= this.PayInLimit;
        }

        public void TransferMoney(Account from, decimal amount)
        {
            if (!from.CanWithdraw(amount))
            {
                from.AddDomainEvent(new FailedWithdrawlDomainEvent(from, amount));
                throw new InsufficientFundsOperationException($"Insufficient funds to make transfer. Account - {from._id}, balance - {from._balance}, wihthdraw amount {amount}.");
            }

            if (!CanPaidIn(amount))
            {
                from.AddDomainEvent(new FailedPayInDomainEvent(from, amount));
                throw new PayInLimitOperationException($"Account pay in limit reached. Account - {_id}, paidin - {_paidIn}, pay in amount {amount}.");
            }

            from.ProcessWithdraw(amount);
            ProcessPaidIn(amount);

            if (_balanceThresold.IsSatisfiedBy(from))
            {
                from.AddDomainEvent(new FundsLowDomainEvent(from));
            }

            if (_paidInThresold.IsSatisfiedBy(this))
            {
                AddDomainEvent(new ApproachingPayInLimitDomainEvent(this));
            }
        }

        public void WithdrawMoney(decimal amount)
        {
            if (!CanWithdraw(amount))
            {
                AddDomainEvent(new FailedWithdrawlDomainEvent(this, amount));
                throw new InsufficientFundsOperationException($"Insufficient funds to make transfer. Account - {_id}, balance - {_balance}, wihthdraw amount {amount}");
            }

            ProcessWithdraw(amount);

            if (_balanceThresold.IsSatisfiedBy(this))
            {
                AddDomainEvent(new FundsLowDomainEvent(this));
            }
        }

        private void ProcessWithdraw(decimal amount)
        {
            _balance -= amount;
            _withdrawn -= amount;

            AddDomainEvent(new AccountWithdrawnDomainEvent(this, amount));
        }

        private void ProcessPaidIn(decimal amount)
        {
            _balance += amount;
            _paidIn += amount;
            AddDomainEvent(new AccountPaidInDomainEvent(this, amount));
        }

        public override string ToString()
        {
            return $"{User.Email}, Balance - {Balance}, Withdraw - {Withdrawn}, PaidIn - {PaidIn}";
        }
    }
}
