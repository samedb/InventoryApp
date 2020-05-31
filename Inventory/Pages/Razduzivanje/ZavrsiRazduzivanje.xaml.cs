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
    /// Interaction logic for ZavrsiRazduzivanje.xaml
    /// </summary>
    public partial class ZavrsiRazduzivanje : Page
    {
        private List<Zaduzenje> razduzeniPredmeti;
        public ZavrsiRazduzivanje(List<Zaduzenje> razduzeniPredmeti)
        {
            this.razduzeniPredmeti = razduzeniPredmeti;
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
                doc.DodajNaslov("Izvestaj o razduzivanju", 18);
                doc.DodajParagraf($"Radnik koji prima inventar: {trenutniRadnik.Ime} {trenutniRadnik.Prezime} ({trenutniRadnik.Username})");
                doc.DodajParagraf($"Potvrdjujem da sam dana {DateTime.Now} razduzio sledeci inventar u prostoriju {razduzeniPredmeti[0].Prostorija.NazivProstorije} koji mi je bio stavljen na zaduzenje:");
                doc.DodajTabelu<ZaStampu2>(razduzeniPredmeti.Select(p => new ZaStampu2
                {
                    Id = p.Id.ToString(),
                    Naziv = p.Predmet.Naziv,
                    Marka = p.Predmet.Marka,
                    Model = p.Predmet.Model,
                    Cena = p.Predmet.Cena.ToString(),
                    Kolicina = p.Kolicina.ToString(),
                    DatumZaduzivanja = p.DatumZaduzivanja.ToString(),
                    KoJeDaoInventar = p.RadnikKojiDajeInventar.Username,
                    KoJePrimioInventar = p.RadnikKojiPrimaInventar.Username
                }).ToList());
                doc.DodajMPiPotpis();
            }

            PdfWrapper.OtvoriFile(path);
        }
    }

    class ZaStampu2
    {
        public string Id { get; set; }
        public string Naziv { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string Cena { get; set; }
        public string Kolicina { get; set; }
        public string DatumZaduzivanja { get; set; }
        public string KoJeDaoInventar { get; set; }
        public string KoJePrimioInventar { get; set; }
    }
}
