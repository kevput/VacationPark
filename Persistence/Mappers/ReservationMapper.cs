using Microsoft.Data.SqlClient;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.Models;

namespace VacationParkApp.Persistence.Mappers
{
    public class ReservationMapper : IReservationMapper
    {
        public Reservation GetById(int id)
        {
            Reservation result = null;
            string sql = @"
                SELECT ReservationId, StartDate, EndDate, CustomerId
                FROM Reservations
                WHERE ReservationId = @Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                result = new Reservation
                {
                    Id = reader.GetInt32(0),
                    StartDate = reader.GetDateTime(1),
                    EndDate = reader.GetDateTime(2),
                    CustomerId = reader.GetInt32(3)
                };
            }
            return result;
        }

        public IEnumerable<Reservation> GetAll()
        {
            var list = new List<Reservation>();
            string sql = @"
                SELECT ReservationId, StartDate, EndDate, CustomerId
                FROM Reservations
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new Reservation
                {
                    Id = reader.GetInt32(0),
                    StartDate = reader.GetDateTime(1),
                    EndDate = reader.GetDateTime(2),
                    CustomerId = reader.GetInt32(3)
                });
            }
            return list;
        }

        public void Add(Reservation reservation)
        {
            string sql = @"
                INSERT INTO Reservations (StartDate, EndDate, CustomerId)
                VALUES (@StartDate, @EndDate, @CustomerId);
                SELECT SCOPE_IDENTITY();
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@StartDate", reservation.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", reservation.EndDate);
            cmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);

            conn.Open();
            var newId = cmd.ExecuteScalar();
            if (newId != null)
            {
                reservation.Id = Convert.ToInt32(newId);
            }
        }

        public void Update(Reservation reservation)
        {
            string sql = @"
                UPDATE Reservations
                SET StartDate=@StartDate, EndDate=@EndDate, CustomerId=@CustomerId
                WHERE ReservationId=@Id
            ";

            using var conn = new SqlConnection(ConnectionSettings.ConnectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@StartDate", reservation.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", reservation.EndDate);
            cmd.Parameters.AddWithValue("@CustomerId", reservation.CustomerId);
            cmd.Parameters.AddWithValue("@Id", reservation.Id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
