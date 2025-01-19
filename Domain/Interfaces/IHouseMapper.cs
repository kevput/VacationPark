using VacationParkApp.Domain.Models;

namespace VacationParkApp.Domain.Interfaces
{
    public interface IHouseMapper
    {
        House GetById(int id);
        IEnumerable<House> GetAll();
        void Add(House house);
        void Update(House house);
    }
}