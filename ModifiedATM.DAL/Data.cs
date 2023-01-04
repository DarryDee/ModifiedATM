using ModifiedATM.BO;
using Newtonsoft.Json;

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

    // Checks if Pin is in File
    public bool PinIsInFile(int pin)
    {
        List<Customer> results = ReadFile<Customer>("customer.txt");
        foreach (Customer customer in results)
        {
            if (customer.Pin == Convert.ToString(pin))
            {
                return true;
            }
        }

        return false;
    }

}