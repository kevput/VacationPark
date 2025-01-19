using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;

namespace VacationParkApp.Persistence.Mappers
{
    public class ParkHousesMapper : IParkHousesMapper
    {
        public void LinkHouseToPark(int houseId, int parkId)
        {
            string sql = @"
                INSERT INTO ParkHouses (ParkId, HouseId)
                VALUES (@ParkId, @HouseId)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkId", parkId);
            cmd.Parameters.AddWithValue("@HouseId", houseId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UnlinkHouseFromPark(int houseId, int parkId)
        {
            string sql = @"
                DELETE FROM ParkHouses
                WHERE ParkId=@ParkId AND HouseId=@HouseId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ParkId", parkId);
            cmd.Parameters.AddWithValue("@HouseId", houseId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<int> GetHousesForPark(int parkId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT HouseId
                FROM ParkHouses
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

        public IEnumerable<int> GetParksForHouse(int houseId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT ParkId
                FROM ParkHouses
                WHERE HouseId=@HouseId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@HouseId", houseId);

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
