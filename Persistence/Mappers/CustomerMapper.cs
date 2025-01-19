using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.Models;

namespace VacationParkApp.Persistence.Mappers
{
    public class CustomerMapper : ICustomerMapper
    {
        public Customer GetById(int id)
        {
            Customer result = null;
            string sql = @"
                SELECT CustomerId, Name, Address
                FROM CustomerPark
                WHERE CustomerId = @Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = new Customer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                };
            }
            return result;
        }

        public IEnumerable<Customer> GetAll()
        {
            var list = new List<Customer>();
            string sql = @"
                SELECT CustomerId, Name, Address
                FROM CustomerPark
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Customer
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Address = reader.GetString(2)
                });
            }
            return list;
        }

        public void Add(Customer customer)
        {
            string sql = @"
                INSERT INTO CustomerPark (Name, Address)
                VALUES (@Name, @Address)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Address", customer.Address);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(Customer customer)
        {
            string sql = @"
                UPDATE CustomerPark
                SET Name=@Name, Address=@Address
                WHERE CustomerId=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", customer.Name);
            cmd.Parameters.AddWithValue("@Address", customer.Address);
            cmd.Parameters.AddWithValue("@Id", customer.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
