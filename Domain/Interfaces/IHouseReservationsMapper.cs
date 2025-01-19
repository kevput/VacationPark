namespace VacationParkApp.Domain.Interfaces
{
    public interface IHouseReservationsMapper
    {
        void LinkHouseToReservation(int houseId, int reservationId);
        void UnlinkHouseFromReservation(int houseId, int reservationId);
        IEnumerable<int> GetHousesForReservation(int reservationId);
        IEnumerable<int> GetReservationsForHouse(int houseId);
    }
}
