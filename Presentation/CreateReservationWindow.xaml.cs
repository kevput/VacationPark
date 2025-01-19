using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VacationParkApp.Domain.Controllers;
using VacationParkApp.Domain.DTOs;

namespace VacationParkApp.Presentation
{
    public partial class CreateReservationWindow : Window
    {
        private readonly DomainManager _domainManager;

        public CreateReservationWindow(DomainManager domainManager)
        {
            InitializeComponent();
            _domainManager = domainManager;
            LoadParks();
        }

        private void BtnSearchCustomer_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = TxtCustomerSearch.Text.Trim();
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Enter a name or ID to search.");
                return;
            }

            var matches = _domainManager.SearchCustomers(searchTerm).ToList();
            if (!matches.Any())
            {
                MessageBox.Show("No matching customers found.");
                CmbCustomers.ItemsSource = null;
            }
            else
            {
                // Wrap each CustomerDTO in a CustomerDisplayItem
                var displayItems = matches
                    .Select(c => new CustomerDisplayItem(c))
                    .ToList();

                CmbCustomers.ItemsSource = displayItems;
                // Now use DisplayMemberPath = "DisplayName"
                CmbCustomers.DisplayMemberPath = nameof(CustomerDisplayItem.DisplayName);
            }
        }

        private void LoadParks()
        {
            var parks = _domainManager.GetAllParks().ToList();

            // Wrap them in ParkDisplayItem
            var parkDisplayItems = parks
                .Select(p => new ParkDisplayItem(p))
                .ToList();

            CmbParks.ItemsSource = parkDisplayItems;
            // Show "Id. Name, Location"
            CmbParks.DisplayMemberPath = nameof(ParkDisplayItem.DisplayText);
        }

        private void BtnShowHouses_Click(object sender, RoutedEventArgs e)
        {
            LstAvailableHouses.ItemsSource = null;

            if (CmbParks.SelectedItem is not ParkDisplayItem pItem)
            {
                MessageBox.Show("Select a park first.");
                return;
            }
            ParkDTO selectedPark = pItem.Park;

            // The rest remains the same:
            if (!int.TryParse(TxtPeopleCount.Text, out int peopleCount) || peopleCount <= 0)
            {
                MessageBox.Show("Invalid number of people.");
                return;
            }

            var start = DpStartDate.SelectedDate;
            var end = DpEndDate.SelectedDate;
            if (start == null || end == null || end <= start)
            {
                MessageBox.Show("Invalid date range.");
                return;
            }

            var houses = _domainManager.GetAvailableHouses(selectedPark.Id, start.Value, end.Value).ToList();
            houses = houses.Where(h => h.Capacity >= peopleCount).ToList();

            var items = new List<HouseDisplayItem>();
            foreach (var h in houses)
            {
                string displayText = $"{h.Street} {h.Number}, capacity {h.Capacity}, location {selectedPark.Location}";
                items.Add(new HouseDisplayItem(h, displayText));
            }

            LstAvailableHouses.ItemsSource = items;
        }


        private void BtnConfirmReservation_Click(object sender, RoutedEventArgs e)
        {
            // Previously: var cust = CmbCustomers.SelectedItem as CustomerDTO;
            var custItem = CmbCustomers.SelectedItem as CustomerDisplayItem;
            if (custItem == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }
            CustomerDTO cust = custItem.Customer;

            // The rest remains the same
            if (LstAvailableHouses.SelectedItem is not HouseDisplayItem selectedItem)
            {
                MessageBox.Show("Select a house first.");
                return;
            }
            var house = selectedItem.House;

            var start = DpStartDate.SelectedDate;
            var end = DpEndDate.SelectedDate;
            if (start == null || end == null || end <= start)
            {
                MessageBox.Show("Invalid date range.");
                return;
            }

            var newRes = _domainManager.MakeReservation(cust.Id, house.Id, start.Value, end.Value);
            if (newRes == null)
            {
                MessageBox.Show("Could not create reservation (inactive or overlap).");
                return;
            }

            MessageBox.Show($"Reservation created: ID={newRes.Id}, Customer={cust.Name}, House={house.Street} {house.Number}");
            this.Close();
        }
    }

    public class HouseDisplayItem
    {
        public HouseDTO House { get; }
        public string DisplayText { get; }

        public HouseDisplayItem(HouseDTO house, string text)
        {
            House = house;
            DisplayText = this.ToString();
        }

        public override string ToString()
        {
            // e.g.: "ID=5, Street=Main 123, Cap=6, Active=True"
            return $"ID={House.Id}, " +
                   $"Street={House.Street} {House.Number}, " +
                   $"Cap={House.Capacity}, " +
                   $"Active={House.IsActive}";
        }

    }
}
