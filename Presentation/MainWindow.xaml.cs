using System.Windows;
using VacationParkApp.Domain.Controllers;

namespace VacationParkApp.Presentation
{
    public partial class MainWindow : Window
    {
        private readonly DomainManager _domainManager;

        public MainWindow(DomainManager domainManager)
        {
            InitializeComponent();
            _domainManager = domainManager;
        }

        public MainWindow() : this(null) { }

        private void BtnCreateReservation_Click(object sender, RoutedEventArgs e)
        {
            if (_domainManager == null)
            {
                MessageBox.Show("DomainManager is null.");
                return;
            }
            var createWin = new CreateReservationWindow(_domainManager);
            createWin.ShowDialog();
        }

        private void BtnSearchReservation_Click(object sender, RoutedEventArgs e)
        {
            if (_domainManager == null)
            {
                MessageBox.Show("DomainManager is null.");
                return;
            }
            var searchWin = new SearchReservationWindow(_domainManager);
            searchWin.ShowDialog();
        }

        private void BtnMaintenance_Click(object sender, RoutedEventArgs e)
        {
            if (_domainManager == null)
            {
                MessageBox.Show("DomainManager is null.");
                return;
            }
            var maintWin = new MaintenanceWindow(_domainManager);
            maintWin.ShowDialog();
        }
    }
}
