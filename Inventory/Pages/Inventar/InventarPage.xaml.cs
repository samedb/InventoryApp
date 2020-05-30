using Inventory.Dialogs;
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
            if (prostorija.SefProstorije != null)
                DodelaSefaButton.Visibility = Visibility.Visible;
            else
                DodelaSefaButton.Visibility = Visibility.Collapsed;
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
                NavigationService.Navigate(new ZaduzivanjePage(InventarDataGrid.SelectedItems.Cast<Inventar>().ToList()));
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

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var inventar = db.Inventar.Where(i => i.Prostorija == prostorija).ToList();
                InventarDataGrid.ItemsSource = inventar;
            }
        }
    }
}
