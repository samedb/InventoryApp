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
            popuniPoljaSaProstorijom();
            UcitajSveRadnike();
        }

        public ProstorijaDetaljiPage(Prostorija p)
        {
            InitializeComponent();
            dodavanjeNoveProstorije = false;
            prostorija = p;
            popuniPoljaSaProstorijom();
            UcitajSveRadnike();
        }

        private void UcitajSveRadnike()
        {
            using (var db = new InventoryContext())
            {
                var radnici = db.Radnici.ToList();
                cbUsernameSefa.ItemsSource = radnici.Select(r => r.Username);
            }
        }

        private void popuniPoljaSaProstorijom()
        {
            tbNazivProstorije.Text = prostorija.NazivProstorije;
            tbSprat.Text = prostorija.Sprat;
            tbSirina.Text = prostorija.Sirina;
            tbDuzina.Text = prostorija.Duzina;
            tbVisina.Text = prostorija.Visina;

            if (prostorija.UsernameSefa != "")
                cbUsernameSefa.SelectedItem = prostorija.UsernameSefa;
        }

        private void uzmiPodatkeProstorijeIzPolja()
        {
            prostorija.NazivProstorije = tbNazivProstorije.Text;
            prostorija.Sprat = tbSprat.Text;
            prostorija.Sirina = tbSirina.Text;
            prostorija.Duzina = tbDuzina.Text;
            prostorija.Visina = tbVisina.Text;
            prostorija.UsernameSefa = cbUsernameSefa.SelectedItem.ToString();
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
                        p.UsernameSefa = prostorija.UsernameSefa;
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
