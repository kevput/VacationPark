using VacationParkApp.Domain.Models;

namespace VacationParkApp.Domain.Interfaces
{
    public interface ICustomerMapper
    {
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
    }
}
