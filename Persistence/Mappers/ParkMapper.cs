using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.Models;

namespace VacationParkApp.Persistence.Mappers
{
    public class ParkMapper : IParkMapper
    {
        public Park GetById(int id)
        {
            Park result = null;
            string sql = @"
                SELECT ParkId, Name, Location
                FROM Parks
                WHERE ParkId = @Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = new Park
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Location = reader.GetString(2)
                };
            }
            return result;
        }

        public IEnumerable<Park> GetAll()
        {
            var list = new List<Park>();
            string sql = "SELECT Id, Name, Location FROM Parks";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Park
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Location = reader.GetString(2)
                });
            }
            return list;
        }

        public void Add(Park park)
        {
            string sql = @"
                INSERT INTO Parks (Name, Location)
                VALUES (@Name, @Location)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", park.Name);
            cmd.Parameters.AddWithValue("@Location", park.Location);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(Park park)
        {
            string sql = @"
                UPDATE Parks
                SET Name=@Name, Location=@Location
                WHERE ParkId=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", park.Name);
            cmd.Parameters.AddWithValue("@Location", park.Location);
            cmd.Parameters.AddWithValue("@Id", park.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
