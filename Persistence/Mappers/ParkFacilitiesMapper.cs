using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;

namespace VacationParkApp.Persistence.Mappers
{
    public class ParkFacilitiesMapper : IParkFacilitiesMapper
    {
        public void LinkFacilityToPark(int facilityId, int parkId)
        {
            string sql = @"
                INSERT INTO ParkFacilities (ParkId, FacilityId)
                VALUES (@ParkId, @FacilityId)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkId", parkId);
            cmd.Parameters.AddWithValue("@FacilityId", facilityId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UnlinkFacilityFromPark(int facilityId, int parkId)
        {
            string sql = @"
                DELETE FROM ParkFacilities
                WHERE ParkId=@ParkId AND FacilityId=@FacilityId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkId", parkId);
            cmd.Parameters.AddWithValue("@FacilityId", facilityId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<int> GetFacilitiesForPark(int parkId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT FacilityId
                FROM ParkFacilities
                WHERE ParkId=@ParkId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkId", parkId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }
            return list;
        }

        public IEnumerable<int> GetParksForFacility(int facilityId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT ParkId
                FROM ParkFacilities
                WHERE FacilityId=@FacilityId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FacilityId", facilityId);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }
            return list;
        }
    }
}
