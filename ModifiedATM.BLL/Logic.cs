using ModifiedATM.BO;
using System;

namespace ModifiedATM.BLL
{
    internal class Logic
    {
        public bool IsInFile(string username)
        {
            Data data = new Data();

            return data.IsInFile(username);
            
            
        }
     
    }
}
