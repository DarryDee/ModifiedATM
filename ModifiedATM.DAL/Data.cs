using ModifiedATM.BO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
            

            HashSet<int> accountNumbers = new HashSet<int>(number.Select(c => c.AccountNumber));

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
        public bool isInFile(Admin user)
        {
            List<Admin> list = ReadFile<Admin>("admin");
            foreach(Admin admin in list)
            {
                if(admin.Name == user.Name && admin.Password == user.Password)
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateFile(Customer customer)
        {
            List<Customer> list = ReadFile<Customer>("customer.txt");
            
            for(int i = 0; i < list.Count; i++)
            {
                if(customer == list[i])
                {
                    list[i] = customer;
                }
            }

            SaveToFile(list);

        }

        
        
        public void SaveToFile<T>(List<T> list)
        {
            // Overwrite the file with first object in the list
            string jsonOutput = JsonConvert.SerializeObject(list[0]);
            if (list[0] is Admin)
            {
                File.WriteAllText("admins.txt", jsonOutput + Environment.NewLine);
            }
            else if (list[0] is Customer)
            {
                File.WriteAllText("customers.txt", jsonOutput + Environment.NewLine);
            }

            // Appends the other objects of list to the file
            for (int i = 1; i < list.Count; i++)
            {
                AddToFile(list[i]);
            }
        }
        

        
        public void AddToFile<T>(T obj)
        {
            string jsonoutput = JsonConvert.SerializeObject(obj);

            if (obj is Admin)
            {
                File.AppendText("admin.txt" +jsonoutput + Environment.NewLine);
            }
            else if (obj is Customer)
            {
                File.AppendText("customer.txt" +jsonoutput+ Environment.NewLine);
            }
           
        }
        public void ReduceBalance(Customer c, int withdraw)
        {
            c.Balance -= withdraw;
            UpdateFile(c);
        }

    }
}
