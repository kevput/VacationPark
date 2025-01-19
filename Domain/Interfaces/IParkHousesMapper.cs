namespace VacationParkApp.Domain.Interfaces
{
    public interface IParkHousesMapper
    {
        void LinkHouseToPark(int houseId, int parkId);
        void UnlinkHouseFromPark(int houseId, int parkId);
        IEnumerable<int> GetHousesForPark(int parkId);
        IEnumerable<int> GetParksForHouse(int houseId);
    }
}
