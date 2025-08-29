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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using Newtonsoft.Json;
using Microsoft.VisualBasic;
using System.Security.Principal;

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
        List<Vklad> vklads = new List<Vklad>(); 
        List<Deposit> deposits = new List<Deposit>();
        private String Result(BankAccount account)
        {
            string result;

            result = account.BankOut();

            return result;
        }
        private void SaveBankAccounts()
        {
            using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Account.json", false))
            {
                string resultJson = JsonConvert.SerializeObject(bankAccountsL, Formatting.Indented);
                writer.WriteLine(resultJson);
            }
        }
        public void OutListBankAccounts()
        {
            TbVivod1.Clear();
            TbVklad.Clear();
            TbDeposit.Clear();
            foreach (var tuple in deposits.Zip(clients, (item1, item2) => (item1, item2)))
            {
                TbDeposit.Text += tuple.item1.ToString() + Environment.NewLine + "***********************" + Environment.NewLine;
            }
            foreach (var tuple in vklads.Zip(clients, (item1, item2) => (item1, item2)))
            {
                TbVklad.Text += tuple.item1.ToString() + Environment.NewLine + "***********************" + Environment.NewLine;
            }
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
        private HashSet<string> uniqueClients = new HashSet<string>();
        private void AddComboBox(Client user, BankAccount account)
        {
            boxChoice.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);
            Combo1.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);
            Combo2.Items.Add(user.FullName + Environment.NewLine + "Номер счета " + account.AccountNumber);


            string displayText = $"{user.FullName} (Паспорт: {user.PassportNumber})";
            if (uniqueClients.Add(displayText))
            {
                vkladbox.Items.Add(displayText);
            }
        }
        
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                using (StreamReader stream = new StreamReader(Environment.CurrentDirectory + "\\Account.json"))
                {
                    string result = stream.ReadToEnd();
                    bankAccountsL.Clear();
                    bankAccountsL = JsonConvert.DeserializeObject<List<BankAccount>>(result);
                    stream.Close();
                }

                using (StreamReader stream = new StreamReader(Environment.CurrentDirectory + "\\Client.json"))
                {
                    string result = stream.ReadToEnd();
                    clients.Clear();
                    clients = JsonConvert.DeserializeObject<List<Client>>(result);
                    stream.Close();
                }

                using (StreamReader stream = new StreamReader(Environment.CurrentDirectory + "\\Transactions.json"))
                {
                    string result = stream.ReadToEnd();
                    transactions.Clear();
                    transactions = JsonConvert.DeserializeObject<List<Transaction>>(result);
                    stream.Close();
                }
                using (StreamReader stream = new StreamReader(Environment.CurrentDirectory + "\\Vklads.json"))
                {
                    string result = stream.ReadToEnd();
                    vklads.Clear();
                    vklads = JsonConvert.DeserializeObject<List<Vklad>>(result);
                    stream.Close();
                }
                using (StreamReader stream = new StreamReader(Environment.CurrentDirectory + "\\Deposits.json"))
                {
                    string result = stream.ReadToEnd();
                    deposits.Clear();
                    deposits = JsonConvert.DeserializeObject<List<Deposit>>(result);
                    stream.Close();
                }
                boxChoice.Items.Clear();
                Combo1.Items.Clear();
                Combo2.Items.Clear();
                vkladbox.Items.Clear();

                foreach (var tuple in bankAccountsL.Zip(clients, (account, client) => (account, client)))
                {
                    string displayText = $"{tuple.client.FullName} (Номер счета {tuple.account.AccountNumber})";
                    boxChoice.Items.Add(displayText);
                    Combo1.Items.Add(displayText);
                    Combo2.Items.Add(displayText);
                }
                HashSet<string> existingItems = new HashSet<string>( vkladbox.Items.Cast<string>() );

                foreach (var tuple in clients.Zip(clients, (account, client) => (account, client)))
                {
                    string displayText = $"{tuple.client.FullName} (Паспорт: {tuple.client.PassportNumber})";

                    if (existingItems.Add(displayText)) 
                    {
                        vkladbox.Items.Add(displayText);
                    }
                }
            } catch
            {
                return;
            }

                    OutListBankAccounts();
                    OutListTransactions();           
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

                using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Account.json", false))
                {
                    string resultJson = JsonConvert.SerializeObject(bankAccountsL);
                    writer.WriteLine(resultJson);
                    writer.Close();
                }
                using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Client.json", false))
                {
                    string resultJson = JsonConvert.SerializeObject(clients);
                    writer.WriteLine(resultJson);
                    writer.Close();
                }

                
                OutListBankAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private void SaveTransaction(string operationType, int accountIndex, decimal amount, int? targetIndex = null)
        {
            string accountNumber = accountIndex < bankAccountsL.Count
                ? bankAccountsL[accountIndex].AccountNumber
                : vklads[accountIndex - bankAccountsL.Count].AccountNumber;

            Transaction transaction = new Transaction(operationType, accountNumber, amount);

            if (targetIndex.HasValue)
            {
                string targetAccountNumber = targetIndex.Value < bankAccountsL.Count
                    ? bankAccountsL[targetIndex.Value].AccountNumber
                    : vklads[targetIndex.Value - bankAccountsL.Count].AccountNumber;

                transaction.AccountNumberTwo = targetAccountNumber;
            }

            transactions.Add(transaction);

            using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Transactions.json", false))
            {
                string resultJson = JsonConvert.SerializeObject(transactions);
                writer.WriteLine(resultJson);
            }

            SaveBankAccounts();
            OutListTransactions();
        }


        private void Add1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(add1.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите правильную сумму.");
                return;
            }

            try
            {
                if (Combo1.SelectedIndex < bankAccountsL.Count) 
                {
                    bankAccountsL[Combo1.SelectedIndex] += amount;
                    transAcc1.Text = bankAccountsL[Combo1.SelectedIndex].BankOut().ToString();
                }
                else 
                {
                    int vkladIndex = Combo1.SelectedIndex - bankAccountsL.Count;
                    vklads[vkladIndex] = (Vklad)(vklads[vkladIndex] + amount);
                    transAcc1.Text = vklads[vkladIndex].ToString();
                }

                SaveTransaction("Пополнение", Combo1.SelectedIndex, amount);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Sub1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!decimal.TryParse(add1.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите правильную сумму.");
                return;
            }

            try
            {
                if (Combo1.SelectedIndex < bankAccountsL.Count)
                {
                    bankAccountsL[Combo1.SelectedIndex] -= amount;
                    transAcc1.Text = bankAccountsL[Combo1.SelectedIndex].BankOut().ToString();
                }
                else 
                {
                    int vkladIndex = Combo1.SelectedIndex - bankAccountsL.Count;
                    vklads[vkladIndex] = (Vklad)(vklads[vkladIndex] - amount);
                    transAcc1.Text = vklads[vkladIndex].ToString();
                }

                SaveTransaction("Снятие", Combo1.SelectedIndex, amount);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear1_Click(object sender, RoutedEventArgs e)
        {
            if (Combo1.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите счет", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                if (Combo1.SelectedIndex < bankAccountsL.Count)
                {
                    bankAccountsL[Combo1.SelectedIndex].Nullifier();
                    transAcc1.Text = bankAccountsL[Combo1.SelectedIndex].BankOut().ToString();
                }
                else
                {
                    int vkladIndex = Combo1.SelectedIndex - bankAccountsL.Count;
                    vklads[vkladIndex].Nullifier();
                    transAcc1.Text = vklads[vkladIndex].ToString();
                }

                OutListTransactions();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message);
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

            if (!decimal.TryParse(add1.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму для перевода.");
                return;
            }

            try
            {
                // Исходный счет
                if (Combo1.SelectedIndex < bankAccountsL.Count)
                {
                    bankAccountsL[Combo1.SelectedIndex] -= amount;
                    transAcc1.Text = bankAccountsL[Combo1.SelectedIndex].BankOut().ToString();
                }
                else
                {
                    int vkladIndex = Combo1.SelectedIndex - bankAccountsL.Count;
                    vklads[vkladIndex] = (Vklad)(vklads[vkladIndex] - amount);
                    transAcc1.Text = vklads[vkladIndex].ToString();
                }

                // Целевой счет
                if (Combo2.SelectedIndex < bankAccountsL.Count)
                {
                    bankAccountsL[Combo2.SelectedIndex] += amount;
                    transAcc2.Text = bankAccountsL[Combo2.SelectedIndex].BankOut().ToString();
                }
                else
                {
                    int vkladIndex = Combo2.SelectedIndex - bankAccountsL.Count;
                    vklads[vkladIndex] = (Vklad)(vklads[vkladIndex] + amount);
                    transAcc2.Text = vklads[vkladIndex].ToString();
                }

                SaveTransaction("Перевод", Combo1.SelectedIndex, amount, Combo2.SelectedIndex);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Ошибка при выполнении перевода: {ex.Message}");
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
            if (Combo1.SelectedIndex >= 0)
            {
                if (Combo1.SelectedIndex < bankAccountsL.Count)
                {
                    var account = bankAccountsL[Combo1.SelectedIndex];
                    transAcc1.Text = $"Обычный счёт: {account.BankOut()}";
                }
                else
                {
                    int vkladIndex = Combo1.SelectedIndex - bankAccountsL.Count;
                    if (vkladIndex >= 0 && vkladIndex < vklads.Count)
                    {
                        var vklad = vklads[vkladIndex];
                        transAcc1.Text = $"Накопительный счёт: {vklad.ToString()}";
                    }
                }
            }
        }

        private void Combo2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo2.SelectedIndex >= 0)
            {
                if (Combo2.SelectedIndex < bankAccountsL.Count)
                {
                    var account = bankAccountsL[Combo2.SelectedIndex];
                    transAcc2.Text = $"{account.BankOut()}";
                }
                else
                {
                    int vkladIndex = Combo2.SelectedIndex - bankAccountsL.Count;
                    if (vkladIndex >= 0 && vkladIndex < vklads.Count)
                    {
                        var vklad = vklads[vkladIndex];
                        transAcc2.Text = $"{vklad.ToString()}";
                    }
                }
            }
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

        private void DelAll_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                bankAccountsL.Clear();
                clients.Clear();
                transactions.Clear();
                vklads.Clear();
                deposits.Clear();  
                File.WriteAllText("Account.json", "[]"); 
                File.WriteAllText("Client.json", "[]");
                File.WriteAllText("Transactions.json", "[]");
                File.WriteAllText("Vklads.json", "[]");
                File.WriteAllText("Deposits.json", "[]");
                TbVivod1.Clear();
                TbDeposit.Clear();
                TbVklad.Clear();
                TransacAll.Clear();
                TransacAllChoice.Clear();
                boxChoice.Items.Clear();
                Combo1.Items.Clear();
                Combo2.Items.Clear();
                vkladbox.Items.Clear();
            }
        }

        private void vkladbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (vkladbox.SelectedIndex == -1)
                return;

            string selectedText = vkladbox.SelectedItem.ToString();

            string clientName = selectedText.Split('(')[0].Trim();

            Client selectedClient = clients.FirstOrDefault(c => c.FullName == clientName);

            if (selectedClient == null)
                return;

            var clientVklads = vklads
                .Where(v => v.AccountHolder == selectedClient)
                .Select(v => $"№{v.AccountNumber}, Баланс: {v.Balance:C}, Ставка: {v.InterestRate}%")
                .ToList();

            var clientDeposits = deposits
                .Where(d => d.AccountHolder == selectedClient)
                .Select(d => $"№{d.AccountNumber}, Баланс: {d.Balance:C}, Срок: {d.DepositTerm} мес.")
                .ToList();
            TbVklad.Text = clientVklads.Any() ? string.Join(Environment.NewLine, clientVklads) : "Нет вкладов";
            TbDeposit.Text = clientDeposits.Any() ? string.Join(Environment.NewLine, clientDeposits) : "Нет депозитов";
        }
        private void VkladOpen_Click(object sender, RoutedEventArgs e)
        {
            DateTime open = DateTime.Now;
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
                Client user1 = clients[vkladbox.SelectedIndex];
                int procent = 18;
                Vklad Account1 = new Vklad(Number, open, user1, Bal, open, procent);
                Account1.EndDate(open);
                vklads.Add(Account1);
                String result;
                result = Result(Account1);
                Combo1.Items.Add(user1.FullName + Environment.NewLine + "Накопительный счёт " + Account1.AccountNumber);
                Combo2.Items.Add(user1.FullName + Environment.NewLine + "Накопительный счёт " + Account1.AccountNumber);
                using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Vklads.json", false))
                {
                    string resultJson = JsonConvert.SerializeObject(vklads);
                    writer.WriteLine(resultJson);
                    writer.Close();
                }
                OutListBankAccounts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }

        private void DepositOpen_Click(object sender, RoutedEventArgs e)
        {
            if (vkladbox.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите клиента для открытия вклада.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Client selectedClient = clients[vkladbox.SelectedIndex]; 
            string termInput = Interaction.InputBox("Введите срок вклада в месяцах:", "Открытие вклада", "6");
            if (!int.TryParse(termInput, out int termInMonths) || termInMonths <= 0)
            {
                MessageBox.Show("Введите корректный срок вклада.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

           
            string amountInput = Interaction.InputBox("Введите сумму вклада:", "Открытие вклада", "10000");
            if (!decimal.TryParse(amountInput, out decimal depositAmount) || depositAmount <= 0)
            {
                MessageBox.Show("Введите корректную сумму вклада.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            Random random = new Random();
            int firstPart = random.Next(1, 10);
            string Number = firstPart.ToString();
            for (int i = 1; i < 12; i++)
            {
                Number += random.Next(0, 10);
            }
            Deposit newDeposit = new Deposit(Number, DateTime.Now, selectedClient, depositAmount, termInMonths);
            deposits.Add(newDeposit);
            using (StreamWriter writer = new StreamWriter(Environment.CurrentDirectory + "\\Deposits.json", false))
            {
                string resultJson = JsonConvert.SerializeObject(deposits);
                writer.WriteLine(resultJson);
                writer.Close();
            }
            OutListBankAccounts();
        }
    }
}

