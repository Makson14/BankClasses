using BankClasses.Classes;
using System;
using System.Collections;
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
using System.Xml.Linq;

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
        List<BankAccount> bankAccountsL = new List<BankAccount>();
        List<Client> clients = new List<Client>();
        List<Transaction> transactions = new List<Transaction>();
        private String Result(BankAccount account)
        {
            string result;

            result = account.BankOut();

            return result;
        }
        public void OutListBankAccounts()
        {
            TbVivod1.Clear();

            foreach (var tuple in bankAccountsL.Zip(clients, (item1, item2) => (item1, item2)))
            {
                TbVivod1.Text += tuple.item1.BankOut() + Environment.NewLine + "***********************" + Environment.NewLine;
            }
        }
        public void OutListTransactions()
        {
            TransacAll.Clear();

            foreach (var transaction in transactions)
            {
                TransacAll.Text += transaction.ToString() + Environment.NewLine + "***********************" + Environment.NewLine;
            }
        }

        private void AddComboBox(Client user, BankAccount account)
        {
            boxChoice.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);
            Combo1.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);
            Combo2.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            datarojd1.SelectedDateFormat = DatePickerFormat.Long;
            datarojd1.FirstDayOfWeek = DayOfWeek.Sunday;
            datarojd1.DisplayDateEnd = DateTime.Now;
            datarojd1.DisplayDateStart = new DateTime(1904, 01, 01);
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
                Client user1 = new Client(FIO, pass, rojd);
                BankAccount Account1 = new BankAccount(Number, open, user1, Bal, open);
                Account1.EndDate(open);
                String result;

                result = Result(Account1);

                bankAccountsL.Add(Account1);
                clients.Add(user1);

                AddComboBox(user1, Account1);


                OutListBankAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }


        private void Add1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else { 
                decimal amount;
            if (decimal.TryParse(add1.Text, out amount))
            {
                try
                {  
                    bankAccountsL[Combo1.SelectedIndex] += amount;
                    OutListBankAccounts();
                        string operationType = "Пополнение";
                        decimal kolvo = amount;
                        Transaction trans1 = new Transaction(operationType, bankAccountsL[Combo1.SelectedIndex].AccountNumber, kolvo);
                        transactions.Add(trans1);
                        transAcc1.Text = Convert.ToString(bankAccountsL[Combo1.SelectedIndex].BankOut());
                        OutListTransactions();
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
        }

        private void Sub1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else { 
            decimal amount;
                if (decimal.TryParse(add1.Text, out amount))
                {
                    try
                    {
                        bankAccountsL[Combo1.SelectedIndex] -= amount;
                        OutListBankAccounts();
                        string operationType = "Снятие";
                        decimal kolvo = amount;
                        Transaction trans1 = new Transaction(operationType, bankAccountsL[Combo1.SelectedIndex].AccountNumber, kolvo);
                        transactions.Add(trans1);
                        transAcc1.Text = Convert.ToString(bankAccountsL[Combo1.SelectedIndex].BankOut());
                        OutListTransactions();
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
            
        }

        private void Clear1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else { 
            try
                    {
                    bankAccountsL[Combo1.SelectedIndex].Nullifier();
                        OutListBankAccounts();
                    transAcc1.Text = Convert.ToString(bankAccountsL[Combo1.SelectedIndex].BankOut());
                    OutListTransactions();
                }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
            }
        }

        private void Trans1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == Combo2.SelectedIndex)
            {
                MessageBox.Show("Выберите разные счета");
                return;
            }
            if (Combo1.SelectedIndex == -1 || Combo2.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                decimal amount;
                if (decimal.TryParse(add1.Text, out amount) && amount > 0)
                {
                    try
                    {
                        bankAccountsL[Combo1.SelectedIndex] = bankAccountsL[Combo1.SelectedIndex] - (amount, bankAccountsL[Combo2.SelectedIndex]);
                        OutListBankAccounts();
                        string operationType = "Перевод";
                        decimal kolvo = amount;
                        Transaction trans1 = new Transaction(operationType, bankAccountsL[Combo1.SelectedIndex].AccountNumber, kolvo)
                        {
                            AccountNumberTwo = bankAccountsL[Combo2.SelectedIndex].AccountNumber
                        };
                        transactions.Add(trans1);
                        transAcc1.Text = Convert.ToString(bankAccountsL[Combo1.SelectedIndex].BankOut());
                        transAcc2.Text = Convert.ToString(bankAccountsL[Combo2.SelectedIndex].BankOut());
                        OutListTransactions();
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
        }


        private void Close1_Click(object sender, RoutedEventArgs e)
        {
            if (boxChoice.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                bankAccountsL[boxChoice.SelectedIndex].CloseStatus();
                OutListBankAccounts();
                TbVivodSchet.Text = Convert.ToString(bankAccountsL[boxChoice.SelectedIndex].BankOut());

            }
        }

        private void Combo1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            transAcc1.Text = Convert.ToString(bankAccountsL[Combo1.SelectedIndex].BankOut());
        }

        private void Combo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            transAcc2.Text = Convert.ToString(bankAccountsL[Combo2.SelectedIndex].BankOut());
        }

        private void boxChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TbVivodSchet.Text = Convert.ToString(bankAccountsL[boxChoice.SelectedIndex].BankOut());
        }
        private void TransChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TransChoice.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedOperationType = selectedItem.Content.ToString();
                TransacAllChoice.Clear();
                var filteredTransactions = transactions
                    .Where(t => t.OperationType == selectedOperationType);

                foreach (var transaction in filteredTransactions)
                {
                    if (transaction.OperationType == "Перевод")
                    {
                        TransacAllChoice.Text += transaction.ToStrPerevod()
                            + Environment.NewLine
                            + "***********************"
                            + Environment.NewLine;
                    }
                    else
                    {
                        TransacAllChoice.Text += transaction.ToString()
                            + Environment.NewLine
                            + "***********************"
                            + Environment.NewLine;
                    }
                }
                if (!filteredTransactions.Any())
                {
                    TransacAllChoice.Text = "Нет транзакций данного типа.";
                }
            }
        }
    }
}
