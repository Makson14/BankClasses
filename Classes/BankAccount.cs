using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClasses.Classes
{

    public class BankAccount
    {
        public bool IsClosed { get; private set; }
        public string AccountNumber { get; set; }
        public DateTime OpenDate { get; set; }
        public Client AccountHolder { get; set; }
        public decimal Balance { get; private set; }
        public DateTime DepositTerm { get; set; }
        public string Status { get; private set; }

        public BankAccount(string accountNumber, DateTime openDate, Client accountHolder, decimal initialBalance, DateTime depositTerm)
        {
            AccountNumber = accountNumber;
            OpenDate = openDate;
            AccountHolder = accountHolder;
            Balance = initialBalance;
            DepositTerm = depositTerm;
            Status = "Открыт";
        }
        public void EndDate(DateTime date)
        {
            date = OpenDate.AddYears(5);
            DepositTerm = date;
        }
        public void UpdateStatus()
        {
            if(Balance < 0)
            {
                Status = "Банкрот";
            }
            if(Balance >= 0)
            {
                Status = "Открыт";
            }
        }
        public void CloseStatus()
        {
            Status = "Закрыт";
            IsClosed = true;
        }

        public void Deposit(decimal amount)
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            Balance += amount;
        }
        public void Nullifier()
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            Balance = 0;
        }
        public bool Withdraw(decimal amount)
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            if (amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public bool Transfer(decimal amount, BankAccount targetAccount)
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            if (Withdraw(amount))
            {
                targetAccount.Deposit(amount);
                return true;
            }
            return false;
        }
        public string BankOut()
        {
            return "Номер счета: " + this.AccountNumber + Environment.NewLine +
                "Дата открытия счета: " + this.OpenDate + Environment.NewLine +
                "ФИО владельца: " + this.AccountHolder.FullName + Environment.NewLine +
                "Номер паспорта владельца: " + this.AccountHolder.PassportNumber + Environment.NewLine +
                "Дата рождения владельца: " + this.AccountHolder.DateOfBirth + Environment.NewLine +
                "Баланс: " + this.Balance + Environment.NewLine +
                "Дата закрытия счёта: " + this.DepositTerm + Environment.NewLine +
                "Статус счёта: " + this.Status;
        }
    }
}
