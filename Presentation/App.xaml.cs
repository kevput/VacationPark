using System.Windows;
using VacationParkApp.Domain.Controllers;
using VacationParkApp.Domain.Interfaces;
using VacationParkApp.Persistence;
using VacationParkApp.Persistence.Mappers;

namespace VacationParkApp.Presentation
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Instantiate real DB mappers
            ICustomerMapper customerMapper = new CustomerMapper();
            IHouseMapper houseMapper = new HouseMapper();
            IParkMapper parkMapper = new ParkMapper();
            IReservationMapper reservationMapper = new ReservationMapper();
            IFacilityMapper facilityMapper = new FacilityMapper();
            IHouseReservationsMapper houseReservationsMapper = new HouseReservationsMapper();
            IParkHousesMapper parkHousesMapper = new ParkHousesMapper();
            IParkFacilitiesMapper parkFacilitiesMapper = new ParkFacilitiesMapper();

            // DomainManager with 8 arguments
            var domainManager = new DomainManager(
                customerMapper,
                houseMapper,
                parkMapper,
                reservationMapper,
                facilityMapper,
                houseReservationsMapper,
                parkHousesMapper,
                parkFacilitiesMapper
            );

            // Show MainWindow
            var mainWin = new MainWindow(domainManager);
            mainWin.Show();
        }
    }
}
