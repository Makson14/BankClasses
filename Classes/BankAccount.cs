using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClasses.Classes
{

    public class BankAccount
    {
        public string AccountNumber { get; set; }
        public DateTime OpenDate { get; set; }
        public Client AccountHolder { get; set; }
        public decimal Balance { get; private set; }
        public int DepositTerm { get; set; }
        public string Status { get; private set; }

        public BankAccount(string accountNumber, DateTime openDate, Client accountHolder, decimal initialBalance, int depositTerm)
        {
            AccountNumber = accountNumber;
            OpenDate = openDate;
            AccountHolder = accountHolder;
            Balance = initialBalance;
            DepositTerm = depositTerm;
            Status = "открыт";
        }

        public void UpdateStatus()
        {
            
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
        }

        public bool Withdraw(decimal amount)
        {
            if (amount <= Balance)
            {
                Balance -= amount;
                return true;
            }
            return false;
        }

        public bool Transfer(decimal amount, BankAccount targetAccount)
        {
            if (Withdraw(amount))
            {
                targetAccount.Deposit(amount);
                return true;
            }
            return false;
        }
    }
}
