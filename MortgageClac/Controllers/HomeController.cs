using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MortgageClac.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MortgageClac.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        //get method
        public IActionResult App()
        {
            Loan loan = new Loan();
            return View(loan);
        }

        //post method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult App(Loan loan)
        {
            var loanHelper = new LoanHelpers.LoanUtils();

            int term = loan.Term * 12;
            int counter = 1;
            var mRate = loanHelper.CalcMyMonthlyRate(loan.Rate);
            decimal totalInterest = new();
            decimal balance = loan.Amount;
            for (int i = 1; i <= term; i++)
            {
                LoanPayment loanPay = new LoanPayment();

                loanPay.Balance = Decimal.Round(balance, 2);
                loanPay.Month = i;
                loanPay.Payment = Decimal.Round((loanHelper.CalcPayment(loan.Amount, loan.Rate, loan.Term)), 2);
                loanPay.MonthlyInterest = Decimal.Round((loanHelper.CalcMonthlyInterest(loanPay.Balance, mRate)), 2);
                loanPay.MonthlyPrincipal = Decimal.Round((loanPay.Payment - loanPay.MonthlyInterest), 2);
                loanPay.TotalInterest = Decimal.Round((totalInterest + loanPay.MonthlyInterest), 2);
                loanPay.Balance = Decimal.Round((balance - loanPay.MonthlyPrincipal), 2);

                if (loanPay.Balance < 0)
                {
                    loanPay.Balance = 0;
                }
                else
                {
                    balance = loanPay.Balance;
                }

                totalInterest = loanPay.TotalInterest;
                loan.Payments.Add(loanPay);
                counter++;
                loan.Payment = loanPay.Payment;
            }
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.Amount + totalInterest;
            return View(loan);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
