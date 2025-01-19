namespace VacationParkApp.Domain.Interfaces
{
    public interface IParkFacilitiesMapper
    {
        void LinkFacilityToPark(int facilityId, int parkId);
        void UnlinkFacilityFromPark(int facilityId, int parkId);
        IEnumerable<int> GetFacilitiesForPark(int parkId);
        IEnumerable<int> GetParksForFacility(int facilityId);
    }
}
