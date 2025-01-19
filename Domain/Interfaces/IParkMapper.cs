using VacationParkApp.Domain.Models;

namespace VacationParkApp.Domain.Interfaces
{
    public interface IParkMapper
    {
        Park GetById(int id);
        IEnumerable<Park> GetAll();
        void Add(Park park);
        void Update(Park park);
    }
}
