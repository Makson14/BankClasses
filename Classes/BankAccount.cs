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
            Balance = 0;
        }

        public static BankAccount operator +(BankAccount account, decimal amount)
        {
            if (account.IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            account.Balance += amount;
            return account;
        }

        public void Nullifier()
        {
            if (IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");
            } else
            Balance = 0;
        }
        public static BankAccount operator -(BankAccount account, decimal amount)
        {
            if (account.IsClosed)
                throw new InvalidOperationException("Операция невозможна, так как счет закрыт.");

            if (amount > account.Balance)
                throw new InvalidOperationException("Недостаточно средств на счете.");

            account.Balance -= amount;
            return account;
        }
        public static bool operator ==(BankAccount Account1, BankAccount Account2)
        {
            return Account1.Balance == Account2.Balance;
        }
        public static bool operator ==(BankAccount account, object obj)
        {
            // Если один из объектов null, проверяем другой
            if (ReferenceEquals(account, null))
                return ReferenceEquals(obj, null);

            return account.Equals(obj);
        }

        public static bool operator !=(BankAccount Account1, BankAccount Account2)
        {
            return Account1.Balance != Account2.Balance;
        }
        public static bool operator !=(BankAccount account, object obj)
        {
            return !(account == obj);
        }
        public static BankAccount operator -(BankAccount sourceAccount, (decimal amount, BankAccount targetAccount) transferData)
        {
            if (sourceAccount.IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет отправителя закрыт.");
            }

            if (transferData.targetAccount.IsClosed)
            {
                throw new InvalidOperationException("Операция невозможна, так как счет получателя закрыт.");
            }

            if (sourceAccount.Balance < transferData.amount)
            {
                throw new InvalidOperationException("Недостаточно средств на счете отправителя.");
            }
            sourceAccount.Balance -= transferData.amount;
            transferData.targetAccount.Balance += transferData.amount;

            return sourceAccount;
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
