using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;

namespace VacationParkApp.Persistence.Mappers
{
    public class HouseReservationsMapper : IHouseReservationsMapper
    {
        public void LinkHouseToReservation(int houseId, int reservationId)
        {
            string sql = @"
                INSERT INTO HouseReservations (HouseId, ReservationId)
                VALUES (@HouseId, @ReservationId)
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@HouseId", houseId);
            cmd.Parameters.AddWithValue("@ReservationId", reservationId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void UnlinkHouseFromReservation(int houseId, int reservationId)
        {
            string sql = @"
                DELETE FROM HouseReservations
                WHERE HouseId=@HouseId AND ReservationId=@ReservationId
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@HouseId", houseId);
            cmd.Parameters.AddWithValue("@ReservationId", reservationId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<int> GetHousesForReservation(int reservationId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT HouseId
                FROM HouseReservations
                WHERE ReservationId=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", reservationId);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(reader.GetInt32(0));
            }
            return list;
        }

        public IEnumerable<int> GetReservationsForHouse(int houseId)
        {
            var list = new List<int>();
            string sql = @"
                SELECT ReservationId
                FROM HouseReservations
                WHERE HouseId=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", houseId);
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
