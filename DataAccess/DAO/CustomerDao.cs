using System;
using System.Collections.Generic;
using System.Text;
using DataAccess.Interfaces;
using DataAccess.Model;
using Dapper;
using System.Linq;

namespace DataAccess.DAO
{
    public class CustomerDao(string connectionString)
     : BaseDao(connectionString), IFCustomerDao
    {
        public int CreateCustomer(Customer customer) 
        {
            using var connection = CreateConnection();

            string sql = @"
        INSERT INTO mealmate_v1.""Customer""
            (""firstName"", ""lastName"", ""email"", ""phoneNumber"", ""postalCode_fk"")
        VALUES
            (@FirstName, @LastName, @Email, @PhoneNumber, @PostalCodeId)
        RETURNING ""customerId"";";

            return connection.QuerySingle<int>(sql, new
            {
                customer.FirstName,
                customer.LastName,
                customer.Email,
                customer.PhoneNumber,
                PostalCodeId = customer.PostalCode?.PostalCodeId
            });
        }
        public Customer? GetCustomerById(int customerId)
        {
            using var connection = CreateConnection();

            string sql = @"
        SELECT
            c.""customerId"" AS CustomerId,
            c.""firstName"" AS FirstName,
            c.""lastName"" AS LastName,
            c.""phoneNumber"" AS PhoneNumber,
            c.""email"" AS Email,
            p.""postalCodeId"" AS PostalCodeId,
            p.""postalCode"" AS PostalCode,
            p.""city"" AS City 
        FROM mealmate_v1.""Customer"" c
        JOIN mealmate_v1.""PostalCode"" p
            ON c.""postalCode_fk"" = p.""postalCodeId""
        WHERE c.""customerId"" = @CustomerId;";

            return connection.Query<Customer, PostalCode, Customer>(
                sql,
                (customer, postalCode) =>
                {
                    customer.PostalCode = postalCode;
                    return customer;
                },
                new { CustomerId = customerId },
                splitOn: "PostalCodeId"
            ).SingleOrDefault();
        }
    }
}
