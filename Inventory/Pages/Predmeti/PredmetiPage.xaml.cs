using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Inventory.Pages.Predmeti
{
    /// <summary>
    /// Interaction logic for PredmetiPage.xaml
    /// </summary>
    public partial class PredmetiPage : Page
    {
        public PredmetiPage()
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
                var predmeti = await db.Predmeti.ToListAsync();
                PredmetiDataGrid.ItemsSource = predmeti;
            }
        }
        private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PredmetDetaljiPage());
        }

        private void IzmeniPredmet_Click(object sender, RoutedEventArgs e)
        {
            if (PredmetiDataGrid.SelectedItem != null)
            {
                Predmet selektovaniPredmet = PredmetiDataGrid.SelectedItem as Predmet;
                NavigationService.Navigate(new PredmetDetaljiPage(selektovaniPredmet));
            }
            else
            {
                MessageBox.Show("Morate da selektujete predmet koji zelite da menjate!");
            }
        }

        private void ObrisiPredmet_Click(object sender, RoutedEventArgs e)
        {
            if (PredmetiDataGrid.SelectedItem != null)
            {
                Predmet selektovaniPredmet = PredmetiDataGrid.SelectedItem as Predmet;

                var dialogResult = MessageBox.Show("Da li ste sigurni da zelite da izbrisete predmet " + selektovaniPredmet, "Brisanje", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    using (var db = new InventoryContext())
                    {
                        var predmetZaBrisanje = db.Predmeti.Find(selektovaniPredmet.Id);
                        db.Remove(predmetZaBrisanje);
                        db.SaveChanges();
                    }
                    // Da osvezim listu predmeta
                    Page_Loaded(null, null);
                }
            }
            else
            {
                MessageBox.Show("Morate da selektujete predmet kojeg zelite da obrisete!");
            }
        }
    }
}
