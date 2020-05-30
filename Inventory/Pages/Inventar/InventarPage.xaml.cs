using Inventory.Dialogs;
using Inventory.Pages.Inventar;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for InventarPage.xaml
    /// </summary>
    public partial class InventarPage : Page
    {
        private Prostorija prostorija;
        public InventarPage(Prostorija p)
        {
            InitializeComponent();
            prostorija = p;
            Naslov.Text = "Inventar prostorije: " + prostorija.NazivProstorije;

            DodelaSefaButton.Visibility = (prostorija.SefProstorije != null) ? Visibility.Collapsed : Visibility.Visible;

            var trenutniRadnik = (Application.Current as App).trenutniRadnik;
            SefProstorijePanel.Visibility = trenutniRadnik.Username == prostorija.SefProstorije.Username ? Visibility.Visible : Visibility.Collapsed;

        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var inventar = db.Inventar.Where(i => i.Prostorija == prostorija).Include(i => i.Predmet).Include(i => i.Prostorija).ToList();
                InventarDataGrid.ItemsSource = inventar;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void ZaduziInventar_Click(object sender, RoutedEventArgs e)
        {
            if (InventarDataGrid.SelectedItems.Count == 0)
                MessageBox.Show("Morate da selektujete minimum jedan predmet iz inventara!");
            else
                NavigationService.Navigate(new ZaduzivanjePage(InventarDataGrid.SelectedItems.Cast<Inventory.Inventar>().ToList()));
        }

        private void RazduziInventar_Click(object sender, RoutedEventArgs e)
        {
                NavigationService.Navigate(new RazduzivanjePage());
        }

        private void DodeliSefaProstorije_Click(object sender, RoutedEventArgs e)
        {
            var window = new IzaberiSefaWindow(prostorija);
            if (window.ShowDialog() == true)
            {
                DodelaSefaButton.Visibility = Visibility.Collapsed;
            }
        }

        private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new InventarDetaljiPage(prostorija));
        }

        private void ObrisiPredmet_Click(object sender, RoutedEventArgs e)
        {
            if (InventarDataGrid.SelectedItem != null)
            {
                var selektovaniInventar = InventarDataGrid.SelectedItem as Inventory.Inventar;

                var dialogResult = MessageBox.Show("Da li ste sigurni da zelite da izbrisete iz inventara: " + selektovaniInventar.Predmet, "Brisanje", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    using (var db = new InventoryContext())
                    {
                        db.Remove(selektovaniInventar);
                        db.SaveChanges();
                    }
                    // Da osvezim listu inventara
                    Page_Loaded(null, null);
                }
            }
            else
            {
                MessageBox.Show("Morate da selektujete predmet iz inventara koju zelite da obrisete!");
            }
        }
    }
}
