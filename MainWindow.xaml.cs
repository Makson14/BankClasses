using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BankClasses
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private Classes.BankAccount Account1;
        private Classes.BankAccount Account2;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datarojd1.SelectedDateFormat = DatePickerFormat.Long;
            datarojd1.FirstDayOfWeek = DayOfWeek.Sunday;
            datarojd1.DisplayDateEnd = DateTime.Now;
            datarojd1.DisplayDateStart = new DateTime(1904, 01, 01);
            datarojd2.SelectedDateFormat = DatePickerFormat.Long;
            datarojd2.FirstDayOfWeek = DayOfWeek.Sunday;
            datarojd2.DisplayDateEnd = DateTime.Now;
            datarojd2.DisplayDateStart = new DateTime(1904, 01, 01);
        }

        private void Reg1_Click(object sender, RoutedEventArgs e)
        {
            string FIO = FIOtb1.Text;
            string pass = PASStb1.Text;
            if (string.IsNullOrWhiteSpace(FIO) || FIO.Split(' ').Length < 2)
            {
                MessageBox.Show("Пожалуйста, введите корректное ФИО (фамилия и имя).");
                return;
            }
            if (string.IsNullOrEmpty(pass) || pass.Length != 10 || !pass.All(char.IsDigit))
            {
                MessageBox.Show("Номер паспорта должен состоять из 10 цифр.");
                return;
            }
            DateTime open = DateTime.Now;
            if (datarojd1.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения.");
                return;
            }
            DateTime rojd = Convert.ToDateTime(datarojd1.SelectedDate).Date;
            Random random = new Random();
            int firstPart = random.Next(1, 10);
            string Number = firstPart.ToString();
            for (int i = 1; i < 12; i++)
            {
                Number += random.Next(0, 10);
            }
            decimal Bal = 0;
            try
            {
                Classes.Client Client1 = new Classes.Client(FIO, pass, rojd);
                Account1 = new Classes.BankAccount(Number, open, Client1, Bal, open);
                Account1.EndDate(open);
                TbVivod1.Text = Account1.BankOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void Open2_Click(object sender, RoutedEventArgs e)
        {
            string FIO = FIOtb2.Text;
            string pass = PASStb2.Text;
            if (string.IsNullOrWhiteSpace(FIO) || FIO.Split(' ').Length < 2)
            {
                MessageBox.Show("Пожалуйста, введите корректное ФИО (фамилия и имя).");
                return;
            }
            if (string.IsNullOrEmpty(pass) || pass.Length != 10 || !pass.All(char.IsDigit))
            {
                MessageBox.Show("Номер паспорта должен состоять из 10 цифр.");
                return;
            }

            DateTime open = DateTime.Now;
            if (datarojd2.SelectedDate == null)
            {
                MessageBox.Show("Пожалуйста, выберите дату рождения.");
                return;
            }
            DateTime rojd = Convert.ToDateTime(datarojd2.SelectedDate).Date;
            Random random = new Random();
            int firstPart = random.Next(1, 10);
            string Number = firstPart.ToString();
            for (int i = 1; i < 12; i++)
            {
                Number += random.Next(0, 10);
            }
            decimal Bal = 0;
            try
            {
                Classes.Client Client2 = new Classes.Client(FIO, pass, rojd);
                Account2 = new Classes.BankAccount(Number, open, Client2, Bal, open);
                Account2.EndDate(open);
                TbVivod2.Text = Account2.BankOut();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private void Add1_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account1, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add1.Text, out amount)) 
                {
                    try
                    {
                        Account1 += amount;
                        TbVivod1.Text = Account1.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Введите правильную сумму.");
                }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Sub1_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account1, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add1.Text, out amount))
                {
                    try
                    {
                        Account1 -= amount;
                        TbVivod1.Text = Account1.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Введите правильную сумму.");
                }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Clear1_Click(object sender, RoutedEventArgs e)
        {
            if(ReferenceEquals(Account1, null) == false)
            {
                    try
                    {
                        Account1.Nullifier();
                        TbVivod1.Text = Account1.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Trans1_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account1, null) == false && ReferenceEquals(Account2, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add1.Text, out amount) && amount > 0)
                {
                    try
                    {
                        Account1 = Account1 - (amount, Account2);
                        TbVivod1.Text = Account1.BankOut();
                        TbVivod2.Text = Account2.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show($"Ошибка при выполнении перевода: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректную сумму для перевода.");
                }
            }
            else
            {
                MessageBox.Show("Один или оба счета не созданы.");
            }
        }

        private void Add2_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account2, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add2.Text, out amount))
                {
                    try
                    {
                        Account2 += amount;
                        TbVivod2.Text = Account2.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Введите правильную сумму.");
                }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Sub2_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account2, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add2.Text, out amount))
                {
                    try
                    {
                        Account2 -= amount;
                        TbVivod2.Text = Account2.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Введите правильную сумму.");
                }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Clear2_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account2, null) == false)
            {
                try
                {
                    Account2.Nullifier();
                    TbVivod2.Text = Account2.BankOut();
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Trans2_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account1, null) == false && ReferenceEquals(Account2, null) == false)
            {
                decimal amount;
                if (decimal.TryParse(add2.Text, out amount) && amount > 0)
                {
                    try
                    {
                        Account2 = Account2 - (amount, Account1);
                        TbVivod1.Text = Account1.BankOut();
                        TbVivod2.Text = Account2.BankOut();
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show($"Ошибка при выполнении перевода: {ex.Message}");
                    }
                }
                else
                {
                    MessageBox.Show("Введите корректную сумму для перевода.");
                }
            }
            else
            {
                MessageBox.Show("Один или оба счета не созданы.");
            }
        }

        private void Close2_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account2, null) == false)
            {
                Account2.CloseStatus();
                TbVivod2.Text = Account2.BankOut();
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void Close1_Click(object sender, RoutedEventArgs e)
        {
            if (ReferenceEquals(Account1, null) == false)
            {
                Account1.CloseStatus();
                TbVivod1.Text = Account1.BankOut(); 
            }
            else
            {
                MessageBox.Show("Счет еще не создан.");
            }
        }

        private void equal_Click(object sender, RoutedEventArgs e)
        {
            
                if (Account1 == Account2)
                {
                    MessageBox.Show("Балансы счетов равны");
                }
                else if (Account1 != Account2)
                {
                    MessageBox.Show("Балансы счетов неравны");
                }
            
        }
    }
}
