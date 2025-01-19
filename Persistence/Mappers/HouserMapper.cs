using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.Models;

namespace VacationParkApp.Persistence.Mappers
{
    public class HouseMapper : IHouseMapper
    {
        public House GetById(int id)
        {
            House result = null;
            string sql = @"
                SELECT Id, Street, Number, IsActive, Capacity
                FROM Houses
                WHERE Id = @Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = new House
                {
                    Id = reader.GetInt32(0),
                    Street = reader.GetString(1),
                    Number = reader.GetString(2),
                    IsActive = reader.GetBoolean(3),
                    Capacity = reader.GetInt32(4)
                };
            }
            return result;
        }

        public IEnumerable<House> GetAll()
        {
            var list = new List<House>();
            string sql = @"
                SELECT Id, Street, Number, IsActive, Capacity
                FROM Houses
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new House
                {
                    Id = reader.GetInt32(0),
                    Street = reader.GetString(1),
                    Number = reader.GetString(2),
                    IsActive = reader.GetBoolean(3),
                    Capacity = reader.GetInt32(4)
                });
            }
            return list;
        }

        public void Add(House house)
        {
            string sql = @"
                INSERT INTO Houses (Street, Number, IsActive, Capacity)
                VALUES (@Street, @Number, @IsActive, @Capacity)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Street", house.Street);
            cmd.Parameters.AddWithValue("@Number", house.Number);
            cmd.Parameters.AddWithValue("@IsActive", house.IsActive);
            cmd.Parameters.AddWithValue("@Capacity", house.Capacity);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(House house)
        {
            string sql = @"
                UPDATE Houses
                SET Street=@Street, Number=@Number, IsActive=@IsActive, Capacity=@Capacity
                WHERE Id=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Street", house.Street);
            cmd.Parameters.AddWithValue("@Number", house.Number);
            cmd.Parameters.AddWithValue("@IsActive", house.IsActive);
            cmd.Parameters.AddWithValue("@Capacity", house.Capacity);
            cmd.Parameters.AddWithValue("@Id", house.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
