using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.Pages
{
    /// <summary>
    /// Interaction logic for RadniciPage.xaml
    /// </summary>
    public partial class RadniciPage : Page
    {
        public RadniciPage()
        {
            InitializeComponent();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var radnici = await db.Radnici.ToListAsync();
                RadniciDataGrid.ItemsSource = radnici;
            }
        }

        private void DodajRadnika_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RadnikDetaljiPage());
        }

        private void IzmeniRadnika_Click(object sender, RoutedEventArgs e)
        {
            if (RadniciDataGrid.SelectedItem != null)
            {
                Radnik selektovaniRadnik = RadniciDataGrid.SelectedItem as Radnik;
                NavigationService.Navigate(new RadnikDetaljiPage(selektovaniRadnik));
            }
            else
            {
                MessageBox.Show("Morate da selektujete radnika kojeg zelite da menjate!");
            }
        }

        private void ObrisiRadnika_Click(object sender, RoutedEventArgs e)
        {
            if (RadniciDataGrid.SelectedItem != null)
            {
                Radnik selektovaniRadnik = RadniciDataGrid.SelectedItem as Radnik;

                var dialogResult = MessageBox.Show("Da li ste sigurni da zelite da izbrisete radnika " + selektovaniRadnik.Username, "Brisanje", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    using (var db = new InventoryContext())
                    {
                        db.Remove(selektovaniRadnik);
                        db.SaveChanges();
                    }
                    // Da osvezim listu radnika
                    Page_Loaded(null, null);
                }
            }
            else
            {
                MessageBox.Show("Morate da selektujete radnika kojeg zelite da obrisete!");
            }
        }
    }
}
