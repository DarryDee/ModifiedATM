using System;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace ModifiedATM.View
{
    public class View
    {
        public void Loginscreen()
        {
            Console.WriteLine("1--------Customer");
            Console.WriteLine("2--------Admin");

            //-bool sign = false;
            int choice = Convert.ToInt16(Console.ReadKey());
            try
            {

               if(choice == 1 || choice == 2)
               {
                  switch (choice)
                  {
                      
                      case 1:
                            bool SignIn = true;
                            while (!SignIn)
                            {p
                                Console.WriteLine("-----Customer-----");
                                Console.Write("\nEnter Login: ");
                                string? login = Console.ReadLine();

                                Console.WriteLine("\nEnter Pin code ");
                                int code = Convert.ToInt16(Console.ReadKey());
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

        public static void CustomerView(string username)
        {

        }
    }
}