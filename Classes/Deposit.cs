using System;

namespace BankClasses.Classes
{
    [Serializable]
    public class Deposit : Vklad
    {
        public int TermInMonths { get; private set; } 
        private readonly DateTime closeDate; 

        public Deposit(string accountNumber, DateTime openDate, Client accountHolder, decimal initialBalance, int termInMonths)
            : base(accountNumber, openDate, accountHolder, initialBalance, openDate.AddMonths(termInMonths), 23m) 
        {
            TermInMonths = termInMonths;
            closeDate = openDate.AddMonths(termInMonths);
        }

        
        public decimal CalculateFinalAmount()
        {
            int n = 12; 
            decimal r = InterestRate / 100; 
            decimal t = TermInMonths / 12m; 

            return Balance * (decimal)Math.Pow((double)(1 + r / n), (double)(n * t));
        }

        
        public int DaysUntilClosure()
        {
            return (closeDate - DateTime.Now).Days;
        }
        public override string ToString()
        {
            return base.BankOut() + Environment.NewLine +
                   $"Процентная ставка: {InterestRate}%" + Environment.NewLine +
                   $"Срок вклада: {TermInMonths} мес." + Environment.NewLine +
                   $"Дата закрытия: {closeDate:dd.MM.yyyy}" + Environment.NewLine +
                   $"Осталось дней: {DaysUntilClosure()}" + Environment.NewLine +
                   $"Сумма на закрытие: {CalculateFinalAmount():C}";
        }
    }
}

