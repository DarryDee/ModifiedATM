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

                            Admin admin = new();
                            Logic logic1 = new();

                            Console.WriteLine("-----Admin-----");
                            Console.Write("\nEnter Login: ");
                            admin.Name = Console.ReadLine();

                            Console.Write("\nEnter Pin code: ");
                            admin.Password = Convert.ToInt32(Console.ReadLine());

                            if (logic1.AdminInFile(admin))
                            {
                                AdminView(admin.Name, admin.Password);
                            }
                            
                            else
                            {
                                Console.WriteLine("Login Failed.Please try again\n");
                                goto Login;
                            }

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
            Console.Clear();
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
                            logic.CashTransfer(username);
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


        public void AdminView(string? username, int? pin)
        {
            Console.Clear();
            Console.WriteLine("=====Admin View======");
            Console.WriteLine("1=====Create New Account\n" +
                "2=====Delete Existing Account\n"+ 
                "3=====Update Account Information\n"+
                "4=====Serach for Account\n"+ 
                "5=====View Reports\n"+
                "6=====Exit");

        Options:
            Console.WriteLine("Please enter one of the options above: ");

            int options = Convert.ToInt16(Console.ReadLine());

            Logic logic = new(); 
            try
            {
                if (options == 1 || options == 2 || options == 3 || options == 4 || options == 5 || options == 6)
                {


                    switch (options)
                    {
                        case 1:
                            logic.CreateNewAccount();
                            break;

                        case 2:
                            logic.DeleteExistingAccount();
                            break;

                        case 3:
                            logic.UpdateAccountInformation();
                            break;

                        case 4:
                            logic.SearchForAccount();
                            break;

                        case 5:
                            logic.ViewReports();
                            break;

                        case 6:
                             Environment.Exit(0);
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Choose an option between 1 - 6");
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