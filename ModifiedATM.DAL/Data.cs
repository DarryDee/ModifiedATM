using ModifiedATM.BO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.Diagnostics;
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
        
        public Customer GetCustomerOfPin(int pin)
        {
            List<Customer> number = ReadFile<Customer>("customer.txt");

            foreach(Customer customer in number)
            {
                if(customer.AccountNumber == pin)
                {
                    return customer;
                }
            }
            return null;
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

        public void SearchforAccount(int? accountID, string name, string type, int? balance, string status)
        {
            List<Customer> customers = ReadFile<Customer>("customer.txt");
            
            foreach (Customer customer in customers)
            {
                
                #region CheckIfNull
                if (accountID == null)
                {
                    customer.AccountNumber = accountID;
                }
                if (name == null)
                {
                    customer.Username = name;
                }
                if (type == null)
                {
                    customer.Typ = type;
                }
                if (balance == null)
                {
                    customer.Balance = balance;
                }
                if (status == null)
                {
                    customer.Status = status;
                }
                #endregion 

                if (customer.Username == name
                && customer.AccountNumber == accountID 
                && customer.Typ == type
                && customer.Balance == balance
                && customer.Status == status)
                {
                    List<Customer> SameDetails = new List<Customer>();

                    SameDetails.Add(customer);

                    Console.WriteLine("{0}   {1}   {2}   {3}   {4}", "User ID", "Holders Name", "Type", "Balance", "Status");

                    Console.WriteLine(customer);
                    //       Console.WriteLine("\n{0}   {1}   {2}   {3}   {4}",SameDetails.);
                }
                else
                {
                    Console.WriteLine("There is no Account with such Data you just passed in");
                }
                
            }


        }

        public void UpdateFile(Customer customer)
        {
            List<Customer> list = ReadFile<Customer>("customer.txt");
            
            bool found = false;

            for(int i = 0; i < list.Count; i++)
            {
                if (customer == list[i])
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

        
        
        public static void SaveToFile<T>(List<T> list)
        {
            
            string jsonOutput = JsonConvert.SerializeObject(list, Formatting.Indented);
            Console.WriteLine(list);
            if (list is Admin)
            {
                File.WriteAllText("admin.txt", jsonOutput + Environment.NewLine);
            }
            else if (list[0] is Customer)
            {

                File.WriteAllText("customer.txt", jsonOutput + Environment.NewLine);
            }

            /*
            // Appends the other objects of list to the file
            for (int i = 1; i < list.Count; i++)
            {
                bool finished = false;
                if(i == list.Count - 1)
                {
                    finished = true;
                }
                AddToFile(list[i], finished);
            }
            */
        }
        

        /*
        public static void AddToFile<T>(T obj, bool finished)
        {
            string? jsonoutput = JsonConvert.SerializeObject(obj);
            if (obj is Admin)
            {
                File.AppendAllText("admin.txt", jsonoutput + Environment.NewLine);
            }
            else if (obj is Customer)
            {
                File.AppendAllText("customer.txt", jsonoutput + Environment.NewLine);
            }

            
            if (finished)
            {
                if(obj is Admin)
                {
                    File.AppendAllText("admin.txt", "]" + Environment.NewLine);
                }

                else if (obj is Customer)
                {
                    File.AppendAllText("customer.txt", "]" + Environment.NewLine);
                }
            }
           */
        
        public void ReduceBalance(Customer c, int withdraw)
        {
            c.Balance -= withdraw;
            UpdateFile(c);
        }

    }
}
