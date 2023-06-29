using ModifiedATM.BO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json.Serialization;

namespace ModifiedATM.DAL
{

    public class Data
    {
        // Returns a list of objects from file
        public static List<T> ReadFile<T>(string filePath)
        {
            var results = new List<T>();

            try
            {
                // Read the file and deserialize the JSON data
                var json = File.ReadAllText(filePath);


                // Before
               //var customers = JsonConvert.DeserializeObject<List<T>>(json);
                var customers = JsonConvert.DeserializeObject<List<T>>(json);

                if (customers != null)
                {

                    // Iterate through the list of objects and set the properties of the T object
                    foreach (var customer in customers)
                    {
                        results.Add(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while reading the file: {ex.Message}");
            }

            return results;
        }

        // Checks if an username is in File
        public bool IsInFile(string username)
        {
            List<Customer> results = ReadFile<Customer>("customer.txt");
            foreach (Customer customer in results)
            {
                if (customer.Username == username)
                {
                    return true;
                }
            }

            return false;
        }
        
        // Unique Account number when new Customer Account is being created 
        public int AccountNumberByOne(int guess)
        {
            List<Customer> number = ReadFile<Customer>("customer.txt");
            

            HashSet<int?> accountNumbers = new HashSet<int?>(number.Select(c => c.AccountNumber));

            while(accountNumbers.Contains(guess))
            {
                guess++;
            }
            return guess;
            
        }

        // Deletes Account
        public void DeleteAccount(Customer customer)
        {
            List<Customer> name = ReadFile<Customer>("customer.txt");

            bool delete = name.Remove(customer);

            if (delete)
            {
                SaveToFile(name);
            }
            else
            {
                Console.WriteLine("Problem occured");
            }
            
        }

        // Retuturns Customer object
        public Customer GetCustomer (string username)
        {
            List<Customer> name = ReadFile<Customer>("customer.txt");

            foreach(Customer customer in name)
            {
                if(customer.Username == username)
                {
                    return customer;
                }
            }
            return null;
        }

        // Returns Pin of Customer (validation reasons)
        public Customer GetCustomerOfPin(int accountNumber)
        {
            List<Customer> number = ReadFile<Customer>("customer.txt");

            foreach(Customer customer in number)
            {
                if(customer.AccountNumber == accountNumber)
                {
                    return customer;
                }
            }
            return null;
        }

        //Checks if AccountNumber is in File
        public bool AccountNumberInFile(int accountNumber)
        {
            List<Customer> number = ReadFile<Customer>("customer.txt");

            foreach (Customer customer in number)
            {
                if (customer.AccountNumber == accountNumber)
                {
                    return true;
                }
            }
            return false;
        }

        // Checks if Pin is in File
        public bool PinIsInFile(int pin)
        {
            List<Customer> results = ReadFile<Customer>("customer.txt");

            foreach (Customer customer in results)
            {
                if (customer.Pin == pin)
                {
                    return true;
                }
            }

            return false;
        }

        public bool LoginDetailsValid(string username, int pin)
        {
            List<Customer> results = ReadFile<Customer>("customer.txt");

            foreach (Customer customer in results)
            {
                if (customer.Pin == pin && customer.Username == username)
                {
                    return true;
                }
            }

            return false;
        }

        // If Admin Prop. is the same as in File
        public bool AdminInFile(Admin user)
        {
            List<Admin> list = ReadFile<Admin>("admin.txt");
            foreach(Admin admin in list)
            {
                if(admin.Name == user.Name && admin.Password == user.Password)
                {
                    return true;
                }
            }

            return false;
        }

        // Loops through File and returns Accounts with the same Prop.
        public void SearchforAccount(int? accountID, string name, string type, int? balance, string status)
        {
            List<Customer> customers = ReadFile<Customer>("customer.txt");
            bool foundCustomer = false;

            foreach (Customer customer in customers)
            {
                int? tempAccountID = accountID;
                string tempName = name;
                string tempType = type;
                int? tempBalance = balance;
                string tempStatus = status;

                #region CheckIfNull
                if (tempAccountID == null)
                {
                    tempAccountID = customer.AccountNumber;
                }
                if (string.IsNullOrEmpty(tempName))
                {
                    tempName = customer.Username;
                }
                if (string.IsNullOrEmpty(tempType))
                {
                    tempType = customer.Typ;
                }
                if (tempBalance == null)
                {
                    tempBalance = customer.Balance;
                }
                if (string.IsNullOrEmpty(tempStatus))
                {
                    tempStatus = customer.Status;
                }

                #endregion

                if (customer.Username == tempName
                && customer.AccountNumber == tempAccountID
                && customer.Typ == tempType
                && customer.Balance == tempBalance
                && customer.Status == tempStatus)
                {
                    foundCustomer = true;
                    List<Customer> SameDetails = new List<Customer>();

                    SameDetails.Add(customer);




                    Console.WriteLine("Account ID".PadRight(15)
                                      + "Holders Name".PadRight(20)
                                      + "Type".PadRight(10)
                                      + "Balance".PadRight(14)
                                      + "Status\n".PadRight(10));



                    Console.WriteLine($"{customer.AccountNumber}".PadRight(15)
                                        + $"{customer.Username}".PadRight(20)
                                        + $"{customer.Typ}".PadRight(10)
                                        + $"{customer.Balance}".PadRight(14)
                                        + $"{customer.Status}\n\n".PadRight(10));


                }
            }

            if (!foundCustomer)
            {
                Console.WriteLine("\nThere is no Account with such Data you just passed in");
                
            }
        }

        // Loopos through File to check Transaction activity
        public void SearchDateOfAccount(string firstDate, string lastDate)
        {
            try
            {
                
                DateTime startDate = DateTime.Parse(firstDate);
                DateTime endDate = DateTime.Parse(lastDate);

                bool found = false;
                List<Transactions> transactions = ReadFile<Transactions>("transaction.txt");

                foreach (Transactions transaction in transactions)
                {
                    if (transaction.Date > startDate && transaction.Date < endDate)
                    {
                        Console.WriteLine("\nTransaction Type".PadRight(22)
                                      + "Holders Name".PadRight(17)
                                      + "Amount".PadRight(12)
                                      + "Date\n".PadRight(10));

                        Console.WriteLine($"{transaction.TransactionType}".PadRight(22)
                                       + $"{transaction.Name}".PadRight(17)
                                       + $"{transaction.Amount}".PadRight(12)
                                       + $"{transaction.Date}\n\n".PadRight(10));

                        found = true;
                    }
                    
                }
                if (!found)
                {
                    Console.WriteLine("There is no Account between both dates");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        // Looks for Account between min and max
        public void SearchBetweenMaxAndMini(int? mini, int? max)
        {
            List<Customer> list = ReadFile<Customer>("customer.txt");
            bool foundCustomer = false;
            foreach (Customer customer in list)
            {
                if(customer.Balance >= mini && customer.Balance < max)
                {
                    foundCustomer = true;
                    Console.WriteLine("\nAccount ID".PadRight(15)
                                      + "Holders Name".PadRight(20)
                                      + "Type".PadRight(10)
                                      + "Balance".PadRight(14)
                                      + "Status\n".PadRight(10));

                    string formatBalance = string.Format(CultureInfo.GetCultureInfo("en-US"), "{0:C}", customer.Balance);

                    Console.WriteLine($"{customer.AccountNumber}".PadRight(15)
                                        + $"{customer.Username}".PadRight(20)
                                        + $"{customer.Typ}".PadRight(10)
                                        + $"{formatBalance}".PadRight(14)
                                        + $"{customer.Status}\n\n".PadRight(10));
                }
                        
            }

            if (!foundCustomer)
            {
                Console.WriteLine("There is no Account between both amounts");
            }
        }

        // Update objects
        public void UpdateFile(Customer customer)
        {
            List<Customer> list = ReadFile<Customer>("customer.txt");
            
            bool found = false;

            
            for(int i = 0; i < list.Count; i++)
            {
                if (customer.AccountNumber == list[i].AccountNumber)
                {
                    list[i] = customer;
                    found = true;
                    break;
                }   
            }

            if (!found)
            {
                list.Add(customer);
            }

            SaveToFile(list);

        }

        //Saves all Prop. into a File
        public static void SaveToFile<T>(List<T> list)
        {

            string jsonOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
            if (list[0] is Admin)
            {
                File.WriteAllText("admin.txt", jsonOutput + Environment.NewLine);
            }
            else if (list[0] is Customer)
            {

                File.WriteAllText("customer.txt", jsonOutput + Environment.NewLine);
            }
            else if (list[0] is Transactions)
            {
                File.WriteAllText("transaction.txt", jsonOutput + Environment.NewLine);
            }
            
            
        }
        
        // Deduct Balance
        public void ReduceBalance(Customer c, int withdraw)
        {
            c.Balance -= withdraw;
            UpdateFile(c);
        }

        public void TransferMoney(Customer reciever, int amount)
        {
            reciever.Balance += amount;
            UpdateFile(reciever);
        }

        public void Deposit (Customer c, int amount)
        {
            c.Balance += amount;
            UpdateFile(c);
        }

        // Saves transactions in Prop.
        public void SaveReceipt(Customer customer, string typ, int withdrawn, DateTime t)
        {
            List<Transactions> list = ReadFile<Transactions>("transaction.txt");
            Transactions transaction = new()
            {
                TransactionType = typ,
                Name = customer.Username,
                Amount = withdrawn,
                Date = DateTime.Now.Date
            };


            list.Add(transaction);

            SaveToFile(list);
        }
    }
}
