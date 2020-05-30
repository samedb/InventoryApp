using Inventory.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Inventory.Pages
{
    /// <summary>
    /// Interaction logic for RazduzivanjePage.xaml
    /// </summary>

    public partial class RazduzivanjePage : Page
    {
        public RazduzivanjePage()
        {
            InitializeComponent();
            InventarDataGrid.ItemsSource = new List<Inventory.Inventar>();
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void ObrisiRed_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as Inventory.Inventar;
            var list = InventarDataGrid.ItemsSource.Cast<Inventory.Inventar>().ToList();
            list.Remove(selectedItem);
            InventarDataGrid.ItemsSource = list;
        }

        private void Razduzi_Click(object sender, RoutedEventArgs e)
        {
            var zaRazduzivanje = InventarDataGrid.Items.Cast<Inventory.Inventar>().ToList();
            if (zaRazduzivanje.Count == 0)
            {
                MessageBox.Show("Lista mora da sadrzi minimum jedan element!");
                return;
            }

            using (var db = new InventoryContext())
            {
                //var sviPredmeti = db.Inventar.Where(i => i.IdProstorije == zaRazduzivanje[0].IdProstorije).ToList();
                //foreach (var predmet in zaRazduzivanje)
                //{
                //    var predmetUBazi = sviPredmeti.First(p => p.Id == predmet.Id);
                //    if (predmetUBazi.Kolicina < predmet.Kolicina)
                //    {
                //        MessageBox.Show("Ne mozete da razduzite vise predmeta nego sto se nalazi u inventaru!");
                //        return;
                //    }
                //    else if (predmetUBazi.Kolicina == predmet.Kolicina)
                //    {
                //        // ako hoce sve da uzme onda ga izbrisi iz baze
                //        db.Remove(predmetUBazi);
                //    }
                //    else
                //    {
                //        predmetUBazi.Kolicina -= predmet.Kolicina;
                //    }
                //}
                db.SaveChanges();
            }

            // Predji na page gde se stampa izvestaj
            NavigationService.Navigate(new ZavrsiRazduzivanje(zaRazduzivanje));
        }
        private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        {
            var noviPredmet = new Inventory.Inventar();
            var dialog = new DodajInventar(noviPredmet);
            var result = dialog.ShowDialog();

            if (result == true)
            {
                // Dodaj taj novi predmet u listu
                var list = InventarDataGrid.ItemsSource.Cast<Inventory.Inventar>().ToList();
                list.Add(noviPredmet);
                InventarDataGrid.ItemsSource = list;
            }
        }
    }
}
