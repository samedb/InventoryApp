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
    /// Interaction logic for InventarDetaljiPage.xaml
    /// </summary>
    public partial class InventarDetaljiPage : Page
    {
        private Prostorija prostorija;
        public InventarDetaljiPage(Prostorija p)
        {
            prostorija = p;
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var predmeti = db.Predmeti.ToList();
                cbPredmet.ItemsSource = predmeti;
            }
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Zapamti_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                try
                {
                    var predmet = cbPredmet.SelectedItem as Predmet;
                    var kolicina = int.Parse(tbKolicina.Text);

                    var i = new Inventory.Inventar
                    {
                        Predmet = db.Predmeti.Find(predmet.Id),
                        Kolicina = kolicina,
                        Prostorija = db.Prostorije.Find(prostorija.Id)
                    };
                    db.Add(i);
                    db.SaveChanges();

                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Greska prilikom dodavanja novog predmeta u inventar");
                }
            }
        }
    }
}
