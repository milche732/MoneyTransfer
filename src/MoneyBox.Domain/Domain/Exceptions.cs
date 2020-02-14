using System;
using System.Collections.Generic;
using System.Text;

namespace Moneybox.Domain.Domain
{
    public class InsufficientFundsOperationException : InvalidOperationException
    {
        public InsufficientFundsOperationException(string message):base(message)
        { 
        
        }
    }

    public class PayInLimitOperationException : InvalidOperationException
    {
        public PayInLimitOperationException(string message) : base(message)
        {

        }
    }
}
