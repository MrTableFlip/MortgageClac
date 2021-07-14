using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MortgageClac.LoanHelpers
{
    public class LoanUtils
    {
        /// <summary>
        /// Calc an interest payment
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="rate"></param>
        /// <param name="term"></param>
        /// <returns></returns>
        public decimal CalcPayment (decimal amount, decimal rate, int term)
        {
            var rateD = Convert.ToDouble(rate);
            var amountD = Convert.ToDouble(amount);

            var paymentD = (amountD * (rateD/1200)) / (1 - Math.Pow(1 + (rateD/1200), -(term*12)));

            return Convert.ToDecimal(paymentD);
        }

        public decimal CalcMyMonthlyRate (decimal rate)
        {
            return rate / 1200m;
        }

        public decimal CalcMonthlyInterest(decimal balance, decimal rate)
        {
            return balance * rate;
        }
    }
}
