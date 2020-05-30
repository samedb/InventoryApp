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

namespace Inventory.Pages.Inventar
{
    /// <summary>
    /// Interaction logic for SviZaduzeniPredmetiPage.xaml
    /// </summary>
    public partial class SviZaduzeniPredmetiPage : Page
    {
        private Prostorija prostorija;
        public SviZaduzeniPredmetiPage(Prostorija prostorija)
        {
            this.prostorija = prostorija;
            InitializeComponent();
            Naslov.Text = "Svi zaduzeni predmeti za " + prostorija.NazivProstorije;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var trenutniRadnik = (Application.Current as App).trenutniRadnik;
            using (var db = new InventoryContext())
            {
                var zaduzenja = await db.Zaduzenja.Where(z => z.Prostorija.Id == prostorija.Id)
                    .Include(z => z.Predmet).Include(z => z.Prostorija).Include(z => z.Radnik).ToListAsync();
                ZaduzeniPredmetiDataGrid.ItemsSource = zaduzenja;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }
    }
}
