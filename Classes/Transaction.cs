using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankClasses.Classes
{
    public class Transaction
    {
        public string OperationType { get; set; } 
        public string AccountNumber { get; set; }
        public DateTime OperationTime { get; set; }
        public bool IsSuccessful { get; set; }
        public Transaction(string operationType, string accountNumber, bool isSuccessful)
        {
            OperationType = operationType;
            AccountNumber = accountNumber;
            OperationTime = DateTime.Now;
            IsSuccessful = isSuccessful;
        }
        public override string ToString()
        {
            return $"Операция: {OperationType}, Номер счета: {AccountNumber}, " +
                   $"Время: {OperationTime}, Успешность: {IsSuccessful}";
        }
    }
}
}
