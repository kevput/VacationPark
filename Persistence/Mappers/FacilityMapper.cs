using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.Models;

namespace VacationParkApp.Persistence.Mappers
{
    public class FacilityMapper : IFacilityMapper
    {
        public Facility GetById(int id)
        {
            Facility result = null;
            string sql = @"
                SELECT Id, Description
                FROM Facilities
                WHERE Id = @Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = new Facility
                {
                    Id = reader.GetInt32(0),
                    Description = reader.GetString(1)
                };
            }
            return result;
        }

        public IEnumerable<Facility> GetAll()
        {
            var list = new List<Facility>();
            string sql = "SELECT Id, Description FROM Facilities";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Facility
                {
                    Id = reader.GetInt32(0),
                    Description = reader.GetString(1)
                });
            }
            return list;
        }

        public void Add(Facility facility)
        {
            string sql = @"
                INSERT INTO Facilities (Description)
                VALUES (@Description)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Description", facility.Description);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void Update(Facility facility)
        {
            string sql = @"
                UPDATE Facilities
                SET Description=@Description
                WHERE Id=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Description", facility.Description);
            cmd.Parameters.AddWithValue("@Id", facility.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
