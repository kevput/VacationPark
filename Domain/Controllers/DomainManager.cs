using VacationParkApp.Domain.Models;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Domain.DTOs;
using System.Linq;

namespace VacationParkApp.Domain.Controllers
{
    public class DomainManager
    {
        private readonly ICustomerMapper _customerMapper;
        private readonly IHouseMapper _houseMapper;
        private readonly IParkMapper _parkMapper;
        private readonly IReservationMapper _reservationMapper;
        private readonly IFacilityMapper _facilityMapper;
        private readonly IHouseReservationsMapper _houseResMapper;
        private readonly IParkHousesMapper _parkHousesMapper;
        private readonly IParkFacilitiesMapper _parkFacilitiesMapper;

        public DomainManager(
            ICustomerMapper customerMapper,
            IHouseMapper houseMapper,
            IParkMapper parkMapper,
            IReservationMapper reservationMapper,
            IFacilityMapper facilityMapper,
            IHouseReservationsMapper houseResMapper,
            IParkHousesMapper parkHousesMapper,
            IParkFacilitiesMapper parkFacilitiesMapper
        )
        {
            _customerMapper = customerMapper;
            _houseMapper = houseMapper;
            _parkMapper = parkMapper;
            _reservationMapper = reservationMapper;
            _facilityMapper = facilityMapper;
            _houseResMapper = houseResMapper;
            _parkHousesMapper = parkHousesMapper;
            _parkFacilitiesMapper = parkFacilitiesMapper;
        }

        // PARKS
        public IEnumerable<ParkDTO> GetAllParks()
        {
            var parks = _parkMapper.GetAll();
            return parks.Select(p => new ParkDTO(p.Id, p.Name, p.Location));
        }

        // CUSTOMERS
        public IEnumerable<CustomerDTO> GetAllCustomers()
        {
            var all = _customerMapper.GetAll();
            return all.Select(c => new CustomerDTO(c.Id, c.Name, c.Address));
        }

        public CustomerDTO? GetCustomerById(int customerId)
        {
            var c = _customerMapper.GetById(customerId);
            if (c == null) return null;
            return new CustomerDTO(c.Id, c.Name, c.Address);
        }

        public IEnumerable<CustomerDTO> SearchCustomers(string searchTerm)
        {
            if (int.TryParse(searchTerm, out int numericId))
            {
                var single = _customerMapper.GetById(numericId);
                if (single == null) return Enumerable.Empty<CustomerDTO>();
                return new List<CustomerDTO> { new CustomerDTO(single.Id, single.Name, single.Address) };
            }
            else
            {
                var all = _customerMapper.GetAll();
                var filtered = all.Where(c => c.Name.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
                return filtered.Select(c => new CustomerDTO(c.Id, c.Name, c.Address));
            }
        }

        // HOUSES
        public IEnumerable<HouseDTO> GetAllHouses()
        {
            var houses = _houseMapper.GetAll();
            return houses.Select(h => new HouseDTO(
                h.Id,
                h.Street,
                h.Number,
                h.IsActive,
                h.Capacity,
                0
            ));
        }

        public HouseDTO GetHouseById(int houseId)
        {
            var house = _houseMapper.GetById(houseId);
            if (house == null) return null;
            return new HouseDTO(
                house.Id,
                house.Street,
                house.Number,
                house.IsActive,
                house.Capacity,
                0
            );
        }

        public void PlaceHouseUnderMaintenance(int houseId)
        {
            var house = _houseMapper.GetById(houseId);
            if (house == null) throw new ArgumentException("House not found.");
            house.IsActive = false;
            _houseMapper.Update(house);
        }

        // FACILITIES (optional, if you do advanced facility logic)
        public IEnumerable<FacilityDTO> GetAllFacilities()
        {
            var facs = _facilityMapper.GetAll();
            return facs.Select(f => new FacilityDTO(f.Id, f.Description));
        }

        // FUTURE RESERVATIONS FOR A HOUSE
        public IEnumerable<ReservationDTO> GetFutureReservationsForHouse(int houseId)
        {
            var reservationIds = _houseResMapper.GetReservationsForHouse(houseId);
            var all = _reservationMapper.GetAll();
            var now = DateTime.Now;
            var future = all
                .Where(r => reservationIds.Contains(r.Id))
                .Where(r => r.StartDate > now);
            return future.Select(r => new ReservationDTO(r.Id, r.StartDate, r.EndDate, r.CustomerId));
        }

        // MAKE RESERVATION
        public ReservationDTO? MakeReservation(int customerId, int houseId, DateTime start, DateTime end)
        {
            var house = _houseMapper.GetById(houseId);
            if (house == null) return null;
            if (!house.IsActive) return null;

            // Overlap check
            var existingResIds = _houseResMapper.GetReservationsForHouse(houseId);
            var existingRes = _reservationMapper.GetAll().Where(r => existingResIds.Contains(r.Id)).ToList();
            foreach (var er in existingRes)
            {
                if (er.StartDate < end && er.EndDate > start)
                    return null;
            }

            // Create new Reservation with no forced Id
            var reservation = new Reservation
            {
                StartDate = start,
                EndDate = end,
                CustomerId = customerId
            };

            // Call Add => PK assigned automatically
            _reservationMapper.Add(reservation);

            // Link bridging
            _houseResMapper.LinkHouseToReservation(houseId, reservation.Id);

            return new ReservationDTO(reservation.Id, reservation.StartDate, reservation.EndDate, reservation.CustomerId);
        }

        // SEARCH RESERVATIONS
        public IEnumerable<ReservationDTO> FindReservationsByCustomerName(string name)
        {
            var allCustomers = _customerMapper.GetAll();
            var matchingIds = allCustomers
                .Where(c => c.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Id)
                .ToList();

            var allReservations = _reservationMapper.GetAll();
            var found = allReservations.Where(r => matchingIds.Contains(r.CustomerId));
            return found.Select(r => new ReservationDTO(r.Id, r.StartDate, r.EndDate, r.CustomerId));
        }

        public IEnumerable<ReservationDTO> FindReservationsByPeriodPark(int parkId, DateTime start, DateTime end)
        {
            var houseIds = _parkHousesMapper.GetHousesForPark(parkId).ToHashSet();
            var allRes = _reservationMapper.GetAll().ToList();
            var matchingRes = new List<Reservation>();

            foreach (var res in allRes)
            {
                bool overlap = (res.StartDate < end) && (res.EndDate > start);
                if (!overlap) continue;

                var linkedHouses = _houseResMapper.GetHousesForReservation(res.Id);
                bool belongsToPark = linkedHouses.Any(hid => houseIds.Contains(hid));
                if (belongsToPark) matchingRes.Add(res);
            }

            return matchingRes.Select(r => new ReservationDTO(r.Id, r.StartDate, r.EndDate, r.CustomerId));
        }

        // GET AVAILABLE HOUSES
        public IEnumerable<HouseDTO> GetAvailableHouses(int parkId, DateTime start, DateTime end)
        {
            var houseIds = _parkHousesMapper.GetHousesForPark(parkId).ToHashSet();
            var allHouses = _houseMapper.GetAll().Where(h => houseIds.Contains(h.Id)).ToList();
            var activeHouses = allHouses.Where(h => h.IsActive).ToList();
            var allReservations = _reservationMapper.GetAll().ToList();

            var availableList = new List<House>();
            foreach (var house in activeHouses)
            {
                var reservationIdsForHouse = _houseResMapper.GetReservationsForHouse(house.Id);
                var reservationsForHouse = allReservations.Where(r => reservationIdsForHouse.Contains(r.Id)).ToList();
                bool hasOverlap = reservationsForHouse.Any(r =>
                    (r.StartDate < end) && (r.EndDate > start)
                );
                if (!hasOverlap) availableList.Add(house);
            }

            return availableList.Select(h => new HouseDTO(
                h.Id,
                h.Street,
                h.Number,
                h.IsActive,
                h.Capacity,
                parkId
            ));
        }
    }
}
