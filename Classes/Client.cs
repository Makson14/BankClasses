using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BankClasses.Classes
{
    [Serializable]
    public class Client
    {
        //[DataMember]
        public string FullName { get; set; }
        //[DataMember]
        public string PassportNumber { get; set; }
        //[DataMember]
        public DateTime DateOfBirth { get; set; }

        public Client(string fullName, string passportNumber, DateTime dateOfBirth)
        {
            FullName = fullName;
            PassportNumber = passportNumber;
            DateOfBirth = dateOfBirth;
        }
    }
}
