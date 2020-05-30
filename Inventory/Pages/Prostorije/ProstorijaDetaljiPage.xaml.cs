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
    /// Interaction logic for ProstorijaDetaljiPage.xaml
    /// </summary>
    public partial class ProstorijaDetaljiPage : Page
    {

        public Prostorija prostorija { get; set; }
        private bool dodavanjeNoveProstorije;

        public ProstorijaDetaljiPage()
        {
            InitializeComponent();
            dodavanjeNoveProstorije = true;
            prostorija = new Prostorija();
            UcitajSveRadnike();
        }

        public ProstorijaDetaljiPage(Prostorija p)
        {
            InitializeComponent();
            dodavanjeNoveProstorije = false;
            prostorija = p;
            UcitajSveRadnike();
            popuniPoljaSaProstorijom();
        }

        private void UcitajSveRadnike()
        {
            using (var db = new InventoryContext())
            {
                var radnici = db.Radnici.ToList();
                cbUsernameSefa.ItemsSource = radnici;
            }
        }

        private void popuniPoljaSaProstorijom()
        {
            tbNazivProstorije.Text = prostorija.NazivProstorije;
            tbSprat.Text = prostorija.Sprat;
            tbSirina.Text = prostorija.Sirina;
            tbDuzina.Text = prostorija.Duzina;
            tbVisina.Text = prostorija.Visina;

            cbUsernameSefa.SelectedItem = cbUsernameSefa.ItemsSource.Cast<Radnik>().First(r => r.Username == prostorija.SefProstorije.Username);
        }

        private void uzmiPodatkeProstorijeIzPolja()
        {
            prostorija.NazivProstorije = tbNazivProstorije.Text;
            prostorija.Sprat = tbSprat.Text;
            prostorija.Sirina = tbSirina.Text;
            prostorija.Duzina = tbDuzina.Text;
            prostorija.Visina = tbVisina.Text;
            prostorija.SefProstorije = cbUsernameSefa.SelectedItem as Radnik;
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
                    uzmiPodatkeProstorijeIzPolja();
                    // Recimo da ovo ovako mora, zbog nekih cuda sa ef core
                    prostorija.SefProstorije = db.Radnici.Find(cbUsernameSefa.Text);

                    if (dodavanjeNoveProstorije)
                    {
                        db.Add(prostorija);
                        db.SaveChanges();
                    }
                    else
                    {
                        Prostorija p = db.Prostorije.First(p => p.Id == prostorija.Id);
                        p.NazivProstorije = prostorija.NazivProstorije;
                        p.Sprat = prostorija.Sprat;
                        p.Sirina = prostorija.Sirina;
                        p.Duzina = prostorija.Duzina;
                        p.Visina = prostorija.Visina;
                        p.SefProstorije = prostorija.SefProstorije;
                        db.SaveChanges();
                    }
                    NavigationService.GoBack();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Greska");
                }
            }
        }
    }
}
