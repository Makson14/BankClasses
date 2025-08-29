using System;

namespace BankClasses.Classes
{
    [Serializable]
    public class Vklad : BankAccount
    {
        public decimal InterestRate { get; set; } 

        public Vklad(string accountNumber, DateTime openDate, Client accountHolder, decimal initialBalance, DateTime depositTerm, decimal interestRate)
            : base(accountNumber, openDate, accountHolder, initialBalance, depositTerm)
        {
            InterestRate = interestRate;
        }
        public (decimal, DateTime) CalculateMonthlyInterest()
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            }

            decimal monthlyInterest = Balance * (InterestRate / 100) / 12; 
            DateTime nextInterestDate = DateTime.Now.AddMonths(1); 

            return (monthlyInterest, nextInterestDate);
        }

        public override string ToString()
        {
            var (monthlyInterest, nextInterestDate) = CalculateMonthlyInterest();

            return base.BankOut() + Environment.NewLine +
                   $"Процентная ставка: {InterestRate}%" + Environment.NewLine +
                   $"Сумма начисления через месяц: {monthlyInterest:C}" + Environment.NewLine +
                   $"Дата начисления: {nextInterestDate:dd.MM.yyyy}";
        }
    }
}

