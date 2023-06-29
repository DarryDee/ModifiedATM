using ModifiedATM.BO;
using ModifiedATM.DAL;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace ModifiedATM.BLL
{
    public class Logic
    {
        public bool LoginDetailsValid(string username, int pin)
        {
            Data data = new();

            return data.LoginDetailsValid(username, pin);
        }

        public bool AdminInFile(Admin admin)
        {
            Data data = new();

            return data.AdminInFile(admin);
        }

        #region Customer 

        public void WithdrawCash(string username)
        {
            Console.WriteLine("Please selcect one of the following options");
            Console.WriteLine("WithdrawCash-----1");
            Console.WriteLine("FastCash---------2");

            //Whether User types in Y or N
            char choice;

            try
            {
                if (Console.ReadLine() == "1")
                {
                WithdrawCash:
                    int[] CashOptions = new int[] { 500, 1000, 2000, 5000, 10000, 15000, 20000 };

                    Console.WriteLine("1-------" + CashOptions[0]);
                    Console.WriteLine("2-------" + CashOptions[1]);
                    Console.WriteLine("3-------" + CashOptions[2]);
                    Console.WriteLine("4-------" + CashOptions[3]);
                    Console.WriteLine("5-------" + CashOptions[4]);
                    Console.WriteLine("6-------" + CashOptions[5]);
                    Console.WriteLine("7-------" + CashOptions[6]);

                    Console.Write("Select one of the denominations of money: ");
                    int select = Convert.ToInt32(Console.ReadLine());



                    if (select == 1 || select == 2 || select == 3 || select == 4 || select == 5 || select == 6 || select == 7)
                    {
                        Data data = new();
                        DateTime t = DateTime.Now;
                        Customer customer = data.GetCustomer(username);

                        Console.Write("Are you sure you want to withdraw Rs." + CashOptions[select - 1] + "(Y/N): ");
                        choice = Convert.ToChar(Console.ReadLine());   
                        if (choice == 'Y' || choice == 'y')
                        {
                            if (customer != null && customer.Balance > CashOptions[select - 1])
                            {
                                data.ReduceBalance(customer, CashOptions[select - 1]);
                            }
                            else
                            {
                                Console.WriteLine("There is a possibility that you don't have enough money to withdraw " + CashOptions[select - 1]);

                                goto WithdrawCash;
                            }

                            Console.Write("Do you want to print a receipt?(Y/N): ");
                            choice = Convert.ToChar(Console.ReadLine());
                            if (choice == 'y' || choice == 'Y')
                            {
                                t = DateTime.Now;

                                string message = "You have withdrawn: ";

                                PrintReceipt(customer, message ,CashOptions[select - 1] , t);
                            }
                            else
                            {
                                Environment.Exit(0);
                            }

                            data.SaveReceipt(customer, "Withdraw Cash", CashOptions[select - 1], t);

                        }

                        else
                        {
                            goto WithdrawCash;
                        }


                    }
                    else
                    {
                        Console.WriteLine("Give in a valid number!");
                        goto WithdrawCash;
                    }


                }
                else
                {
                FastCash:
                    {
                        Data data = new();
                        DateTime t = DateTime.Now;
                        Customer customer = data.GetCustomer(username);

                        string message = " ";

                        Console.Write("Enter the withdrawal amount: ");
                        int withdraw = Convert.ToInt16(Console.ReadLine());

                        if (customer != null && customer.Balance >= withdraw)
                        {
                            data.ReduceBalance(customer, withdraw);
                        }
                        else
                        {
                            Console.WriteLine("You don't have enough money to withdraw " + withdraw);

                            goto FastCash;
                        }

                        Console.WriteLine("Do you want to print a receipt?(Y/N): ");
                        choice = Convert.ToChar(Console.ReadLine());
                        if (choice == 'y' || choice == 'Y')
                        {
                            t = DateTime.Now;

                            message = "Withdrawn: ";
                            PrintReceipt(customer, message, withdraw, t);
                        }

                        data.SaveReceipt(customer, "Withdraw Cash(Normal Cash)", withdraw, t);

                    }
                }
            }
           
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CashTransfer(string username)
        {
            CashTransferStart:

            Console.Write("Enter amount in multiples of 500: ");
            int transfer = Convert.ToInt16(Console.ReadLine());

            if(transfer % 500 == 0)
            {
                // Customer as logged in
                Data data = new();
                DateTime t = DateTime.Now;
                Customer customer = data.GetCustomer(username);

                
                Console.Write("Please enter the account number to which you wish to transfer money to: ");

                int guessnumber = Convert.ToInt16(Console.ReadLine());

                // Customer's Pin
                Customer pincustomer = data.GetCustomerOfPin(guessnumber);

                if (data.AccountNumberInFile(guessnumber))
                {
                    Console.WriteLine("You wish to deposit Rs " +  transfer.ToString("0.000") + " in account held by " + pincustomer.Username  + " If this information is correct please re-enter the account number: ");

                    if (guessnumber == Convert.ToInt16(Console.ReadLine()))
                    {

                        if (customer != null && customer.Balance >= transfer)
                        {
                            data.ReduceBalance(customer, transfer);
                            data.TransferMoney(pincustomer, transfer);
                        }
                        else
                        {
                            Console.WriteLine("There is a possibility that you don't have enough money to transfer " + transfer);
                            
                            goto CashTransferStart;
                        }
                        

                        Console.WriteLine("Transaction confirmed.");


                        Console.Write("Do you wish to print a receipt (Y/N): ");

                        char recepit = Convert.ToChar(Console.ReadLine());
                        
                        if(recepit == 'y' || recepit == 'Y') 
                        {
                            t = DateTime.Now;
                            PrintReceipt(customer, "Amount Transferred: ", transfer, t);
                        }

                        data.SaveReceipt(customer, "Cash Transfer", transfer, t);
                        
                    }
                    else
                    {
                        
                        Console.WriteLine("Account number not found!");
                        goto CashTransferStart;
                    }
                }
                else
                {
                    Console.WriteLine("You can't deposit Cash into your own and a non - existing account");
                    goto CashTransferStart;
                }
                
               

            }
            else
            {
                Console.WriteLine("The amount you entered was not in multiples of 500 ");
                goto CashTransferStart;
            }

        }

        public void DepositCash(string username) 
        {

            Data data = new();
            DateTime t = DateTime.Now;
            Customer customer = data.GetCustomer(username);
            
            Console.Write("Enter the cash amount to deposit: ");

            int deposit = Convert.ToInt16(Console.ReadLine());

            data.Deposit(customer, deposit);

            Console.WriteLine("\nCash Deposited Successfully.");

            Console.Write("Do you wish to print a receipt (Y/N)?: ");

            char recepeit = Convert.ToChar(Console.ReadLine());

            if(recepeit == 'y' || recepeit == 'Y')
            {
                t = DateTime.Now;
                PrintReceipt(customer, "Deposited: ", deposit, t);
               
            }

            data.SaveReceipt(customer,"Deposit Cash", deposit, t);

        }

        public void DisplayBalance(string username)
        {
            Data data = new();
            
            Customer customer = data.GetCustomer(username);

            Console.WriteLine("Account  #" + customer.AccountNumber);

            DateTime t = DateTime.Now;
            Console.WriteLine($"Date: {t:dd/MM/yyyy}\n");

            Console.WriteLine("Balance: " + customer.Balance);

            PrintReceipt(customer, "Displayed Balance", 0, t);

            data.SaveReceipt(customer,"Display Balance", 0, t);
            
        }

        public static void PrintReceipt(Customer customer, string message, int withdrawn, DateTime t)
        {
            Console.WriteLine($"\nAccount #{customer.AccountNumber}");
            Console.WriteLine($"Date: {t:dd/MM/yyyy}");
            Console.WriteLine("\n" + message + withdrawn);
            Console.WriteLine($"Your Balance: {customer.Balance}");
        }
        #endregion

        #region Admin

        public void CreateNewAccount()
        {
            try
            {
                Customer createCustomer = new Customer();
                Data data = new();

                Console.Write("Name: ");
                createCustomer.Username = Console.ReadLine();

                Console.Write("Pin Code: ");
                createCustomer.Pin = Convert.ToInt32(Console.ReadLine());

            TypeSavings:

                Console.Write("Type :(Savings, Current): ");
                string createCostumerS = Console.ReadLine();

                if (createCostumerS == "Savings" || createCustomer.Typ == "Current")
                {
                    createCustomer.Typ = createCostumerS;
                }
                else
                {
                    Console.WriteLine("Type in Savings or Current\n");
                    goto TypeSavings;
                }

            TypeStatus:
                Console.Write("Starting Balance: ");
                createCustomer.Balance = Convert.ToInt32(Console.ReadLine());

                Console.Write("Status: (Active, Passive): ");
                string createCustomerStatus = Console.ReadLine();

                if (createCustomerStatus == "Active" || createCustomerStatus == "Passive")
                {
                    createCustomer.Status = createCustomerStatus;
                }
                else
                {
                    Console.WriteLine("Type in Active or Passive\n");
                    goto TypeStatus;
                }

                int accountnumber = data.AccountNumberByOne(1);
                createCustomer.AccountNumber = accountnumber;

                data.UpdateFile(createCustomer);
                Console.WriteLine("Account Successfully Created - the account number assigned is " + accountnumber);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public void DeleteExistingAccount()
        {
            Data data = new();
            Console.Write("Enter the account number to which you want to delete: ");
            int chooseAccount = Convert.ToInt32(Console.ReadLine());

            Customer customer = data.GetCustomerOfPin(chooseAccount);

            if(data.AccountNumberInFile(chooseAccount))
            {
                Console.WriteLine("You wish to delete the account held by " + customer.Username + "; If this Information is correct please re-enter the account number : ");
                int chooseAccount1 = Convert.ToInt16(Console.ReadLine());

                if(chooseAccount == chooseAccount1)
                {
                    data.DeleteAccount(customer);

                    Console.WriteLine("Account Deleted Succesfully");
                }
                else
                {
                    Console.WriteLine("You typed in two different account numbers!");
                }


            }
            else
            {
                Console.WriteLine("The account number you entered is not being held by anyone");
            }

            Console.WriteLine("Account Deleted Succesfully");
        }

        public void UpdateAccountInformation()
        {
            try
            {
                Console.Write("Enter the Account Number you wish to update: ");
                int accountNumber = Convert.ToInt32(Console.ReadLine());

                Data data = new();

                Customer customer = data.GetCustomerOfPin(accountNumber);

                if (data.AccountNumberInFile(accountNumber))
                {
                    Console.WriteLine(
                        $"Account #{customer.AccountNumber}\n" +
                        $"Type: {customer.Typ}\n" +
                        $"Holder: {customer.Username}\n" +
                        $"Balance: {customer.Balance}\n" +
                        $"Status:  {customer.Status}");

                    Console.WriteLine("Please enter in the fields you wish to update (leave blank otherwise): ");

                    Console.Write("Pin: ");

                    
                    string? changePin = Console.ReadLine();

                    if (!string.IsNullOrEmpty(changePin))
                    {
                        customer.Pin = Convert.ToInt16(changePin);
                    }

                    // Change Holder's Name
                    Console.Write("Holders Name: ");

                    string? changeName = Console.ReadLine();

                    if (!string.IsNullOrEmpty(changeName))
                    {
                        customer.Username = changeName;
                    }

                    // Change Status
                    Console.Write("Status: ");

                    string? changeStatus = Console.ReadLine();

                    if(changeStatus == "Active" || changeStatus == "Passive")
                    {
                        if(!string.IsNullOrEmpty(changeStatus))
                        {
                            customer.Status = changeStatus;
                        }
                    }

                    data.UpdateFile(customer);

                }
                else
                {
                    Console.WriteLine("Something went wrong.There is a possibility that the Account number is not found");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SearchForAccount()
        {
            Data data = new();

            Console.WriteLine("Type in the following information to search for a specific account");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Search Menu");
            Console.ResetColor();

            // Account ID
            int? userID;
            Console.Write("Account ID: ");
            string? userid = Console.ReadLine();
            if(string.IsNullOrEmpty(userid))
            {
                userID = null;
            }
            else
            {
                userID = Convert.ToInt32(userid);
            }


            // Customer's Name
            Console.Write("Holder Name: ");
            string? name = Console.ReadLine();
            if(string.IsNullOrEmpty(name))
            { 
                name = null;
            }


            // Type of Customer's Account
            Console.Write("Type (Savings Current ): ");
            string? type = Console.ReadLine();
            if(string.IsNullOrEmpty(type))
            {
                type = null;
            }


            // Balance of Customer
            Console.Write("Balance: ");
            int? balance;
            string? stringBalance = Console.ReadLine();
            if(string.IsNullOrEmpty(stringBalance))
            {
                balance = null;
            }
            else
            {
                balance= Convert.ToInt32(stringBalance);
            }


            // Status of Customers Account
            Console.Write("Status: ");
            string? status = Console.ReadLine();
            if(string.IsNullOrEmpty(status))
            {
                status = null;
            }

            data.SearchforAccount(userID ,name ,type , balance, status);

        }

        public void ViewReports()
        {
            try
            {
                Data data = new();

            choise:
                {
                    Console.Write("1---Account By Amount\n" +
                                      "2---Account By Date\n\n" +
                                      "Your choise: ");
                }

                string? option = Console.ReadLine();

                if (option == "1")
                {
                    Console.Write("Enter the minimum amount: ");
                    int? min = Convert.ToInt16(Console.ReadLine());

                    Console.Write("Enter maximum amount: ");
                    int? max = Convert.ToInt16(Console.ReadLine());

                    data.SearchBetweenMaxAndMini(min, max);

                }

                else if (option == "2")
                {
                    string regExPattern = @"^\d{2}/\d{2}/\d{4}$";

                    TypeDate:
                    {
                        Console.Write("Enter the starting date: ");
                    }

                    var startDate = string.Format("{0:dd/MM/yyyy}", Console.ReadLine());
                    if (!Regex.IsMatch(startDate, regExPattern))
                    {
                        Console.WriteLine("Please format your date like this dd/MM/yyyy the next time\n");
                        goto TypeDate;
                    }
                    

                    Console.Write("Enter the ending date: ");
                    string endDate = string.Format("{0:dd/MM/yyyy}", Console.ReadLine());
                    if (!Regex.IsMatch(endDate, regExPattern))
                    {
                        Console.WriteLine("Please format your date like this dd/MM/yyyy the next time\n");
                        goto TypeDate;
                    }

                    data.SearchDateOfAccount(startDate, endDate);        
                }

                else
                {
                    Console.WriteLine("Give in a Valid Number\n");

                    goto choise;
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        #endregion
    }

}

