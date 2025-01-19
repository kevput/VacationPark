using VacationParkApp.Domain.Models;

namespace VacationParkApp.Domain.Interfaces
{
    public interface IReservationMapper
    {
        Reservation GetById(int id);
        IEnumerable<Reservation> GetAll();
        void Add(Reservation reservation);
        void Update(Reservation reservation);
    }
}
