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
    /// Interaction logic for PredmetDetaljiPage.xaml
    /// </summary>
    public partial class PredmetDetaljiPage : Page
    {
        public Predmet predmet{ get; set; }
        private bool dodavanjeNovogPredmeta;

        public PredmetDetaljiPage()
        {
            InitializeComponent();
            dodavanjeNovogPredmeta = true;
            predmet = new Predmet();
            popuniPoljaSaPredmetom();
        }

        public PredmetDetaljiPage(Predmet p)
        {
            InitializeComponent();
            dodavanjeNovogPredmeta = false;
            predmet = p;
            popuniPoljaSaPredmetom();
        }

        private void popuniPoljaSaPredmetom()
        {
            tbId.Text = predmet.Id.ToString();
            tbNaziv.Text = predmet.Naziv;
            tbMarka.Text = predmet.Marka;
            tbModel.Text = predmet.Model;
            tbCena.Text = predmet.Cena.ToString();
        }

        private void uzmiPodatkeRadnikaIzPolja()
        {
            predmet.Id = int.Parse(tbId.Text);
            predmet.Naziv = tbNaziv.Text;
            predmet.Marka= tbMarka.Text;
            predmet.Model = tbModel.Text;
            predmet.Cena = int.Parse(tbCena.Text);
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
                    uzmiPodatkeRadnikaIzPolja();

                    if (dodavanjeNovogPredmeta)
                    {
                        db.Add(predmet);
                        db.SaveChanges();
                    }
                    else
                    {
                        Predmet p = db.Predmeti.Find(predmet.Id);
                        p.Id = predmet.Id;
                        p.Naziv = predmet.Naziv;
                        p.Marka = predmet.Marka;
                        p.Model = predmet.Model;
                        p.Cena = predmet.Cena;
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
