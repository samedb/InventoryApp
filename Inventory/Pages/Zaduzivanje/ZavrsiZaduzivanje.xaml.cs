using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ZavrsiZaduzivanje.xaml
    /// </summary>
    public partial class ZavrsiZaduzivanje : Page
    {
        private List<Inventory.Inventar> zaduzeniPredmeti;
        public ZavrsiZaduzivanje(List<Inventory.Inventar> zaduzeniPredmeti)
        {
            this.zaduzeniPredmeti = zaduzeniPredmeti;
            InitializeComponent();
        }

        private void NazadNaPocetnu_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProstorijePage());
        }

        private void StampajIzvestaj_Click(object sender, RoutedEventArgs e)
        {
            string path = Directory.GetCurrentDirectory() + "\\izvestaj.pdf";
            var trenutniRadnik = (Application.Current as App).trenutniRadnik;
            using (var doc = new PdfWrapper(path))
            {
                doc.DodajNaslov("Izvestaj o zaduzivanju", 18);
                doc.DodajParagraf($"Radnik: {trenutniRadnik.Ime} {trenutniRadnik.Prezime} ({trenutniRadnik.Username})");
                doc.DodajParagraf($"Potvrdjujem da sam dana {DateTime.Now} zaduzio sledeci inventar iz prostorije {zaduzeniPredmeti[0].Prostorija.NazivProstorije} koji mi se stavlja na zaduzenje:");
                doc.DodajTabelu<ZaStampu>(zaduzeniPredmeti.Select(p => new ZaStampu{
                    Naziv = p.Predmet.Naziv,
                    Marka = p.Predmet.Marka,
                    Model = p.Predmet.Model,
                    Cena = p.Predmet.Cena.ToString(),
                    Kolicina = p.Kolicina.ToString(),
                    Prostorija = p.Prostorija.NazivProstorije
                }).ToList());
                doc.DodajMPiPotpis();
            }

            PdfWrapper.OtvoriFile(path);
        }
    }

    class ZaStampu
    {
        public String Naziv { get; set; }
        public String Marka { get; set; }
        public String Model { get; set; }
        public String Cena { get; set; }
        public String Kolicina { get; set; }
        public String Prostorija { get; set; }
    }
}
