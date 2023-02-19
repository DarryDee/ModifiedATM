
using ModifiedATM.BO;
using ModifiedATM.DAL;
using System;

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
            Console.WriteLine("Enter the cash amount to deposit: ");

            int deposit = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("\nCash Deposited Successfully.");

            Console.WriteLine("Do you wish to print a receipt (Y/N)? ");


            if(Console.ReadLine() == "y" || Console.ReadLine() == "Y")
            {
                DateTime t = DateTime.Now;

                string message = "Amount Transferred: ";
                PrintReceipt(customer, message, withdraw, t);
            }
            else
            {

            }
        }

        public void DisplayBalance(string username)
        {

        }

        public void Exit (string username)
        {
            Environment.Exit(0);
        }
         
        public static void PrintReceipt(Customer customer,string message,int withdrawn, DateTime t)
        {
            Console.WriteLine($"{customer.AccountNumber}");
            Console.WriteLine($"Date: {t:dd/MM/yyyy}");
            Console.WriteLine("\n"+ message, withdrawn);
            Console.WriteLine($"{customer.Balance}");
        }        
        
         #endregion
    } 

}

