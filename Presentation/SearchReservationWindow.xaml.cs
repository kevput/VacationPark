using System;
using System.Linq;
using System.Windows;
using VacationParkApp.Domain.Controllers;
using VacationParkApp.Domain.DTOs;

namespace VacationParkApp.Presentation
{
    public partial class SearchReservationWindow : Window
    {
        private readonly DomainManager _domainManager;

        public SearchReservationWindow(DomainManager domainManager)
        {
            InitializeComponent();
            _domainManager = domainManager;
            LoadParks();
        }

        private void LoadParks()
        {
            var parks = _domainManager.GetAllParks().ToList();

            var displayItems = parks
                .Select(p => new ParkDisplayItem(p))
                .ToList();

            CmbParks.ItemsSource = displayItems;
            CmbParks.DisplayMemberPath = nameof(ParkDisplayItem.DisplayText);
        }

        private void BtnSearchByName_Click(object sender, RoutedEventArgs e)
        {
            LstResults.Items.Clear();
            string name = TxtCustomerName.Text.Trim();
            if (string.IsNullOrEmpty(name)) return;

            var found = _domainManager.FindReservationsByCustomerName(name).ToList();
            foreach (var r in found)
            {
                var cust = _domainManager.GetCustomerById(r.CustomerId);
                // Show name and ID in parentheses, e.g. "John Smith (12)"
                string cName = cust != null
                    ? $"{cust.Name} ({cust.Id})"
                    : $"Cust {r.CustomerId}";

                LstResults.Items.Add(
                    $"Res {r.Id}, {r.StartDate:d}-{r.EndDate:d}, {cName}"
                );
            }
        }

        private void BtnSearchPeriodPark_Click(object sender, RoutedEventArgs e)
        {
            LstResults.Items.Clear();
            if (CmbParks.SelectedItem is not ParkDisplayItem pItem) return; // or ParkDTO if not using display items
            var park = pItem.Park;  // or just use the DTO directly
            var start = DpStartDate.SelectedDate;
            var end = DpEndDate.SelectedDate;
            if (start == null || end == null || end <= start) return;

            var found = _domainManager.FindReservationsByPeriodPark(park.Id, start.Value, end.Value).ToList();
            foreach (var r in found)
            {
                var cust = _domainManager.GetCustomerById(r.CustomerId);
                string cName = cust != null
                    ? $"{cust.Name} ({cust.Id})"
                    : $"Cust {r.CustomerId}";

                LstResults.Items.Add(
                    $"Res {r.Id}, {r.StartDate:d}-{r.EndDate:d}, {cName}"
                );
            }
        }

    }
}
