using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace BankClasses.Classes
{
    [Serializable]
    public class Transaction
    {
       // [DataMember]
        public string OperationType { get; set; }
        //[DataMember]
        public string AccountNumber { get; set; }
       // [DataMember]
        public string AccountNumberTwo { get; set; }
       // [DataMember]    
        public DateTime OperationTime { get; set; }
       // [DataMember]
        public decimal kolvo { get; set; }
        public Transaction(string operationType, string accountNumber, decimal Amount)
        {
            OperationType = operationType;
            AccountNumber = accountNumber;
            OperationTime = DateTime.Now;
            kolvo = Amount;
        }
        public override string ToString()
        {
            return "Операция: " + this.OperationType + Environment.NewLine 
                + "Номер счета: " 
                + this.AccountNumber + Environment.NewLine + 
                "Время: " + this.OperationTime + Environment.NewLine + 
                "Количество средств: "+this.kolvo;
        }
        public string ToStrPerevod()
        {
            return "Тип операции: " + this.OperationType + Environment.NewLine +
               "Счёт отправителя: " + this.AccountNumber + Environment.NewLine +
               "Счёт получателя: " + this.AccountNumberTwo + Environment.NewLine +
               "Время операции: " + this.OperationTime + Environment.NewLine +
               "Количество средств: " + this.kolvo;
        }
    }
}

