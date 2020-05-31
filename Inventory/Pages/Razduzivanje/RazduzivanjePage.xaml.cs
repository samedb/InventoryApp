using Inventory.Dialogs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
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
        private Prostorija prostorija;
        public RazduzivanjePage(Prostorija prostorija)
        {
            this.prostorija = prostorija; 
            InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var trenutniRadnik = (Application.Current as App).trenutniRadnik;
            using (var db = new InventoryContext())
            {
                var zaduzenja = await db.Zaduzenja.Where(z => z.Prostorija.Id == prostorija.Id)
                    .Include(z => z.Predmet).Include(z => z.Prostorija)
                    .Include(z => z.RadnikKojiDajeInventar).Include(z => z.RadnikKojiPrimaInventar).ToListAsync();
                ZaduzeniPredmetiDataGrid.ItemsSource = zaduzenja;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        //private void ObrisiRed_Click(object sender, RoutedEventArgs e)
        //{
        //    var selectedItem = (sender as Button).DataContext as Inventory.Inventar;
        //    var list = InventarDataGrid.ItemsSource.Cast<Inventory.Inventar>().ToList();
        //    list.Remove(selectedItem);
        //    InventarDataGrid.ItemsSource = list;
        //}

        private void Razduzi_Click(object sender, RoutedEventArgs e)
        {
            var zaRazduzenje = ZaduzeniPredmetiDataGrid.SelectedItems.Cast<Zaduzenje>().ToList();
            if (zaRazduzenje.Count == 0)
            {
                MessageBox.Show("Morate da selektujete bar jedan predmet za razduzenje!");
                return;
            }

            using (var db = new InventoryContext())
            {
                // Obrisi ih sve iz Zaduzenja

                foreach (var r in zaRazduzenje)
                {
                    db.Remove(r);
                }

                // Dodaj ih u Inventar te prostorije
                foreach (var r in zaRazduzenje)
                {
                    var alternativa = new Inventory.Inventar
                    {
                        Predmet = r.Predmet,
                        Prostorija = r.Prostorija,
                        Kolicina = 0
                    };
                    var inventar = db.Inventar.FirstOr(i => i.Predmet?.Id == r.Predmet.Id && i.Prostorija.Id == r.Prostorija.Id, alternativa);


                    inventar.Kolicina += r.Kolicina;

                    if (inventar == alternativa)
                        db.Add(inventar);

                    db.SaveChanges();
                }



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
            NavigationService.Navigate(new ZavrsiRazduzivanje(zaRazduzenje));
        }

        //private void DodajPredmet_Click(object sender, RoutedEventArgs e)
        //{
        //    var noviPredmet = new Inventory.Inventar();
        //    var dialog = new DodajInventar(noviPredmet);
        //    var result = dialog.ShowDialog();

        //    if (result == true)
        //    {
        //        // Dodaj taj novi predmet u listu
        //        var list = InventarDataGrid.ItemsSource.Cast<Inventory.Inventar>().ToList();
        //        list.Add(noviPredmet);
        //        InventarDataGrid.ItemsSource = list;
        //    }
        //}
    }
}
