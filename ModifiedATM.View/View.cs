using System;
using ModifiedATM.BLL;
using ModifiedATM.BO;


namespace ModifiedATM.View
{
    public class View
    {
        public void Loginscreen()
        {
            Console.WriteLine("1--------Customer");
            Console.WriteLine("2--------Admin");

            //-bool sign = false;
            int choice = Convert.ToInt16(Console.ReadLine());
            try
            {

               if(choice == 1 || choice == 2)
               {
                  switch (choice)
                  {
                      
                      case 1:
                            bool SignIn = true;
                  Login:          
                            
                            Logic logic = new();
                            Customer customer = new();

                            Console.WriteLine("-----Customer-----");
                            Console.Write("\nEnter Login: ");
                            customer.Username = Console.ReadLine();

                            Console.WriteLine("\nEnter Pin code ");
                            customer.Pin = Convert.ToInt32(Console.ReadLine());

                            if(logic.IsInFile(customer.Username) && logic.PinIsInFile(customer.Pin))
                            {
                                CustomerView(customer.Username, customer.Pin);
                            }

                            else
                            {
                                Console.WriteLine("Login Failed.Please try again\n");
                                goto Login;
                            }
                            
                            
                            
                         break;

                      case 2:
                            Console.WriteLine("-----Admin-----");
                            Console.Write("\nEnter Login: ");
                            string? login1 = Console.ReadLine();

                            Console.WriteLine("\nEnter Pin code ");
                            int code1 = Convert.ToInt16(Console.ReadKey());

                            break;
                  }
               }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            
        }

        public void CustomerView(string username, int pin)
        {
            Console.WriteLine("=====Customer Menu=====");
            Console.WriteLine("1----Withdraw Cash\n" +
                "2----Cash Transfer\n" +
                "3----Deposit Cash\n" +
                "4----Display Balance\n" +
                "5----Exit");
           Options:
            Console.Write("\nPlease select one of the above options: ");
            int option = Convert.ToInt16(Console.ReadLine());

            Logic logic = new();

            try
            {
                
                if (option == 1 || option == 2 || option == 3 || option == 4 || option == 5)
                {
                    switch (option)
                    {
                        case 1:
                            logic.WithdrawCash(username);
                            break;

                        case 2:
                            logic.CashTransfer(username,pin);
                            break;

                        case 3:
                            logic.DepositCash(username);
                            break;

                        case 4:
                            logic.DisplayBalance(username);
                            break;

                        case 5:
                            Environment.Exit(0);
                            break;


                    }
                }
                else
                {
                    Console.WriteLine("Choose an option between 1- 5");
                    goto Options;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}