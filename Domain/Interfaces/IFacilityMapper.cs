using VacationParkApp.Domain.Models;

namespace VacationParkApp.Domain.Interfaces
{
    public interface IFacilityMapper
    {
        Facility GetById(int id);
        IEnumerable<Facility> GetAll();
        void Add(Facility facility);
        void Update(Facility facility);
    }
}
