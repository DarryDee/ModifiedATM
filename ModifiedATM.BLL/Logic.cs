
using ModifiedATM.BO;
using ModifiedATM.DAL;
using System;
using System.Data;
using System.Reflection;
using System.Threading.Channels;

namespace ModifiedATM.BLL
{
    public class Logic
    {
        public bool IsInFile(string username)
        {
            Data data = new();
            
            return data.IsInFile(username);

        }

        public bool PinIsInFile(int pin)
        {
            Data data = new();

            return data.PinIsInFile(pin);

        }

        #region Customer 

        public void WithdrawCash(string username)
        {
            Console.WriteLine("Please selcect one of the following options");
            Console.WriteLine("WithdrawCash-----1");
            Console.WriteLine("FastCash---------2");
            try
            {
                if (Console.ReadLine() == "1")
                {
                WithdrawCash:
                    int[] CashOptions = new int[] { 500, 1000, 2000, 5000, 10000, 15000, 20000 };

                    Console.WriteLine("1-------{0}" + CashOptions[0]);
                    Console.WriteLine("2-------{0}" + CashOptions[1]);
                    Console.WriteLine("3-------{0}" + CashOptions[2]);
                    Console.WriteLine("4-------{0}" + CashOptions[3]);
                    Console.WriteLine("5-------{0}" + CashOptions[4]);
                    Console.WriteLine("6-------{0}" + CashOptions[5]);
                    Console.WriteLine("7-------{0}" + CashOptions[6]);

                    Console.Write("Select one of the denominations of money: ");
                    int select = Convert.ToInt32(Console.ReadLine());



                    if (select == 1 || select == 2 || select == 3 || select == 4 || select == 5 || select == 6 || select == 7)
                    {
                        Data data = new();
                        Customer customer = data.GetCustomer(username);
                        Console.Write("Are you sure you want to withdraw Rs." + CashOptions[select - 1] + "(Y/N): ");
                        if (Console.ReadLine() == "Y" || Console.ReadLine() == "y")
                        {
                            if (customer != null && customer.Balance > CashOptions[select - 1])
                            {
                                data.ReduceBalance(customer, CashOptions[select - 1]);
                            }

                            Console.WriteLine("Do you want to print a receipt?: ");
                            if (Console.ReadLine() == "Y" || Console.ReadLine() == "y")
                            {
                                DateTime t = DateTime.Now;

                                string message = "You have withdrawn: ";

                                PrintReceipt(customer, message ,select, t);
                            }
                            else
                            {
                                Environment.Exit(0);
                            }

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

                    Data data = new();
                    Customer customer = data.GetCustomer(username);

                    Console.WriteLine("Enter the withdrawal amount: ");
                    int withdraw = Convert.ToInt16(Console.ReadLine());

                    if (customer != null && customer.Balance > withdraw)
                    {
                        data.ReduceBalance(customer, withdraw);
                    }

                    Console.WriteLine("Do you want to print a receipt?: ");
                    if (Console.ReadLine() == "Y" || Console.ReadLine() == "y")
                    {
                        DateTime t = DateTime.Now;

                        string message = "Withdrawn: ";
                        PrintReceipt(customer,message,withdraw, t);
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

            Console.WriteLine("Enter amount in multiples of 500: ");
            int withdraw = Convert.ToInt16(Console.ReadLine());

            if(withdraw % 500 == 0)
            {
                // Customer as logged in
                Data data = new();
                Customer customer = data.GetCustomer(username);

                
                Console.WriteLine("Please enter the account number to which you wish to transfer money to: ");

                int guessnumber = Convert.ToInt16(Console.ReadLine());

                // Customer's Pin
                Data data1 = new();
                Customer pincustomer = data.GetCustomerOfPin(guessnumber);

                if (PinIsInFile(guessnumber))
                {
                    Console.WriteLine("You wish to deposit Rs " +  withdraw.ToString("0.000") + " in account held by " + pincustomer.Username  + " If this information is correct please re-enter the account number: ");
                    int guessnumber1 = Convert.ToInt16(Console.ReadLine());

                    if(guessnumber == guessnumber1)
                    {
                        Console.WriteLine("Transaction confirmed.");
                        Console.WriteLine("Do you wish to print a receipt (Y/N): ");

                        char recepit = Convert.ToChar(Console.ReadLine());
                        
                        if(Console.ReadLine() == "Y" || Console.ReadLine() == "y") 
                        {
                            DateTime t = DateTime.Now;

                            string message = "Amount Transferred: ";
                            PrintReceipt(customer, message, withdraw, t);
                        }
                        
                    }
                    else
                    {
                        
                        Console.WriteLine("Account number not found!");
                        goto CashTransferStart;
                    }
                }
                else
                {
                    Console.WriteLine("You can't deposit Cash into your own account");
                }
                
               

            }
            else
            {
                Console.WriteLine("The amount you entered was not in multiples of 500 ");
            }

        }

        public void DepositCash(string username) 
        {

            Data data = new();
            Customer customer = data.GetCustomer(username);

            Console.WriteLine("Enter the cash amount to deposit: ");

            int deposit = Convert.ToInt16(Console.ReadLine());

            customer.Balance += deposit;

            Console.WriteLine("\nCash Deposited Successfully.");

            Console.WriteLine("Do you wish to print a receipt (Y/N)? ");


            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                DateTime t = DateTime.Now;

                string message = "Deposited: ";
                PrintReceipt(customer, message, deposit, t);
            }
        }

        public void DisplayBalance(string username)
        {
            Data data = new();
            
            Customer customer = data.GetCustomer(username);

            Console.WriteLine("Account  #" + customer.AccountNumber);

            DateTime t = DateTime.Now;
            Console.WriteLine($"Date: {t:dd/MM/yyyy}\n");

            Console.WriteLine("Balance: " + customer.Balance);

        }

        public void Exit (string username)
        {
            Environment.Exit(0);
        }
         
        public static void PrintReceipt(Customer customer,string message,int withdrawn, DateTime t)
        {
            Console.WriteLine($"Account #{customer.AccountNumber}");
            Console.WriteLine($"Date: {t:dd/MM/yyyy}");
            Console.WriteLine("\n"+ message, withdrawn);
            Console.WriteLine($"{customer.Balance}");
        }

        #endregion

        #region Admin

        public void CreateNewAccount()
        {
            try
            {
                Customer createCustomer = new Customer();
                Data data = new();

                Console.WriteLine("Login: ");
                createCustomer.Username = Console.ReadLine();

                Console.WriteLine("Pin Code: ");
                createCustomer.Pin = Convert.ToInt32(Console.ReadLine());

            TypeSavings:

                Console.WriteLine("Type: (Savings, Current) ");
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


                Console.WriteLine("Starting Balance: ");
                createCustomer.Balance = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Status: ");
                string createCustomerStatus = Console.ReadLine();

                if (createCustomer.Status == "Active" || createCustomer.Status == "Passive")
                {
                    createCustomer.Status = createCustomerStatus;
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
            Console.WriteLine("Enter the account number to which you want to delete: ");
            int chooseAccount = Convert.ToInt32(Console.ReadLine());

            Customer customer = data.GetCustomerOfPin(chooseAccount);

            if(PinIsInFile(chooseAccount))
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
                Console.WriteLine("Enter the Account Number you wish to update: ");
                int accountNumber = Convert.ToInt32(Console.ReadLine());

                Data data = new();

                Customer customer = data.GetCustomerOfPin(accountNumber);

                if (PinIsInFile(accountNumber))
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
                    Console.WriteLine("Holders Name: ");

                    string? changeName = Console.ReadLine();

                    if (!string.IsNullOrEmpty(changeName))
                    {
                        customer.Username = changeName;
                    }

                    // Change Status

                    string? changeStatus = Console.ReadLine();

                    if(changeStatus == "Active" || changeStatus == "Passive")
                    {
                        if(!string.IsNullOrEmpty(changeStatus))
                        {
                            customer.Status = changeStatus;
                        }
                    }



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

        }

        public void ViewReports()
        {

        }

        public void Exit()
        {

        }


        #endregion
    }

}

