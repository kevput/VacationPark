using System;
using System.IO;
using System.Linq;
using System.Windows;
using VacationParkApp.Domain.Controllers;
using VacationParkApp.Domain.DTOs;

namespace VacationParkApp.Presentation
{
    public partial class MaintenanceWindow : Window
    {
        private readonly DomainManager _domainManager;

        public MaintenanceWindow(DomainManager domainManager)
        {
            InitializeComponent();
            _domainManager = domainManager;
        }

        // ------------------------------------------
        // (A) Search Houses (OR-based filtering)
        // ------------------------------------------
        private void BtnSearchHouses_Click(object sender, RoutedEventArgs e)
        {
            // 1. Fetch all houses from the domain
            var allHouses = _domainManager.GetAllHouses().ToList();

            // 2. Parse user input
            int? typedId = null;
            if (int.TryParse(TxtHouseId.Text.Trim(), out int parsedId) && parsedId > 0)
                typedId = parsedId;

            string streetFilter = TxtStreetFilter.Text.Trim();

            int? minCapacity = null;
            if (int.TryParse(TxtCapacityFilter.Text.Trim(), out int parsedCap) && parsedCap > 0)
                minCapacity = parsedCap;

            // 3. If everything empty, you can either show all houses or message the user
            bool allEmpty = (typedId == null) &&
                            string.IsNullOrEmpty(streetFilter) &&
                            (minCapacity == null);

            // 4. Filter with OR logic
            // A house matches if:
            //  - typedId is set AND house.Id == typedId
            //    OR
            //  - streetFilter is not empty AND house.Street contains streetFilter
            //    OR
            //  - minCapacity is set AND house.Capacity >= minCapacity
            var filtered = allHouses.Where(h =>
                (typedId.HasValue && h.Id == typedId.Value)
                || (!string.IsNullOrEmpty(streetFilter)
                    && h.Street.IndexOf(streetFilter, StringComparison.OrdinalIgnoreCase) >= 0)
                || (minCapacity.HasValue && h.Capacity >= minCapacity.Value)
            );

            var results = allEmpty ? allHouses : filtered.ToList();

            // 5. Show in LstResults
            LstResults.Items.Clear();
            foreach (var house in results)
            {
                LstResults.Items.Add(new HouseDisplayItem(house,this.ToString()));
            }

            // 6. Set status
            if (allEmpty)
                LblStatus.Text = $"No filters given. Showing all {results.Count} house(s).";
            else
                LblStatus.Text = $"Found {results.Count} house(s) matching your criteria.";
        }

        // ------------------------------------------
        // (B) Set Selected House In Maintenance
        // ------------------------------------------
        private void BtnSetMaintenance_Click(object sender, RoutedEventArgs e)
        {
            // We require the user to select exactly one house from the list
            if (LstResults.SelectedItem is not HouseDisplayItem selectedItem)
            {
                MessageBox.Show("Please select a house from the list.");
                return;
            }

            int houseId = selectedItem.House.Id;

            try
            {
                // 1) Place the house under maintenance
                _domainManager.PlaceHouseUnderMaintenance(houseId);

                // 2) Retrieve future reservations (conflicts)
                var futureRes = _domainManager.GetFutureReservationsForHouse(houseId).ToList();

                // Clear the list and show the conflicts
                LstResults.Items.Clear();
                foreach (var res in futureRes)
                {
                    var cust = _domainManager.GetCustomerById(res.CustomerId);
                    string cName = cust != null
                        ? $"{cust.Name} ({cust.Id})"
                        : $"Cust {res.CustomerId}";

                    LstResults.Items.Add(
                        $"Res {res.Id}, {res.StartDate:d}-{res.EndDate:d}, {cName}"
                    );
                }

                LblStatus.Text = $"House {houseId} is now inactive. Found {futureRes.Count} future reservation(s).";

                // 3) Ask user if they want to save these reservations to a text file
                if (futureRes.Any())
                {
                    var result = MessageBox.Show(
                        "Do you want to save the problematic reservations?",
                        "Save Conflicts",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question
                    );

                    if (result == MessageBoxResult.Yes)
                    {
                        // 4) Save file to Desktop
                        string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                        string filePath = Path.Combine(desktopPath, "ProblematicReservations.txt");

                        using (var sw = new StreamWriter(filePath))
                        {
                            foreach (var item in LstResults.Items)
                            {
                                sw.WriteLine(item.ToString());
                            }
                        }

                        MessageBox.Show($"Problematic reservations saved to:\n{filePath}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
