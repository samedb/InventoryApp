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

namespace Inventory.Pages
{
    /// <summary>
    /// Interaction logic for ZaduzivanjePage.xaml
    /// </summary>
    public partial class ZaduzivanjePage : Page
    {
        public ZaduzivanjePage(List<Inventory.Inventar> inventar)
        {
            InitializeComponent();
            using (var db = new InventoryContext())
            {
                var radnici = db.Radnici.ToList();
                cbRadnici.ItemsSource = radnici;
            }
            InventarDataGrid.ItemsSource = inventar;
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
        private void Zaduzi_Click(object sender, RoutedEventArgs e)
        {
            var zaZaduzivanje = InventarDataGrid.Items.Cast<Inventory.Inventar>().ToList();
            if (zaZaduzivanje.Count == 0)
            {
                MessageBox.Show("Lista mora da sadrzi minimum jedan element!");
                return;
            }

            var radnikKojiPrimaInventar = cbRadnici.SelectedItem as Radnik;
            if(radnikKojiPrimaInventar == null)
                    {
                MessageBox.Show("Morate za odaberete radnika koji zaduzuje inventar!");
                return;
            }


            using (var db = new InventoryContext())
            {
                foreach (var predmet in zaZaduzivanje)
                {
                    var inventar = db.Inventar.Include(i => i.Predmet);
                    var predmetUBazi = inventar.First(i => i.Predmet.Id == predmet.Predmet.Id);
                    if (predmetUBazi.Kolicina < predmet.Kolicina)
                    {
                        MessageBox.Show("Ne mozete da razduzite vise predmeta nego sto se nalazi u inventaru!");
                        return;
                    }
                    else if (predmetUBazi.Kolicina == predmet.Kolicina)
                    {
                        // ako hoce sve da uzme onda ga izbrisi iz baze
                        db.Remove(predmetUBazi);
                    }
                    else
                    {
                        predmetUBazi.Kolicina -= predmet.Kolicina;
                    }

                    // Unesi u tabelu Zaduzenje
                    db.Add(new Zaduzenje
                    {
                        RadnikKojiDajeInventar = db.Radnici.Find((Application.Current as App).trenutniRadnik.Username),
                        RadnikKojiPrimaInventar = db.Radnici.Find(radnikKojiPrimaInventar.Username),
                        Predmet = db.Predmeti.Find(predmet.Predmet.Id),
                        Prostorija = db.Prostorije.Find(predmet.Prostorija.Id),
                        Kolicina = predmet.Kolicina,
                        DatumZaduzivanja = DateTime.Now
                    });
                }
                db.SaveChanges();
            }

            // Predji na page gde se stampa izvestaj
            NavigationService.Navigate(new ZavrsiZaduzivanje(zaZaduzivanje, radnikKojiPrimaInventar));
        }
    }
}
