using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VacationParkApp.Domain.DTOs
{
    public record CustomerDTO(int Id, string Name, string Address);

    // 6 parameters: (Id, Street, Number, IsActive, Capacity, ParkId)
    public record HouseDTO(int Id, string Street, string Number, bool IsActive, int Capacity, int ParkId);

    public record ParkDTO(int Id, string Name, string Location);

    public record ReservationDTO(int Id, DateTime StartDate, DateTime EndDate, int CustomerId);

    public record FacilityDTO(int Id, string Description);
    public class CustomerDisplayItem
    {
        public CustomerDTO Customer { get; }
        public string DisplayName => $"{Customer.Name} ({Customer.Id})";

        public CustomerDisplayItem(CustomerDTO customer)
        {
            Customer = customer;
        }
    }

    // Wraps a ParkDTO and provides "Id. Name, Location" for display
    public class ParkDisplayItem
    {
        public ParkDTO Park { get; }
        public string DisplayText => $"{Park.Id}. {Park.Name}, {Park.Location}";

        public ParkDisplayItem(ParkDTO park)
        {
            Park = park;
        }
    }
    public class HouseDisplayItem
    {
        public HouseDTO House { get; }

        public HouseDisplayItem(HouseDTO house)
        {
            House = house;
        }

        public override string ToString()
        {
            // e.g.: "ID=5, Street=Main 123, Cap=6, Active=True"
            return $"ID={House.Id}, Street={House.Street} {House.Number}, " +
                   $"Cap={House.Capacity}, Active={House.IsActive}";
        }
    }
}
