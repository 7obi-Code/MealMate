using DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Interfaces
{
        public interface IFCustomerDao
        {
            int CreateCustomer(Customer customer);

            Customer? GetCustomerById(int customerId);
        }
}

