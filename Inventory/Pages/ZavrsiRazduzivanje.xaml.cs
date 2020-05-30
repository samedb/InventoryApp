using System;
using System.Collections.Generic;
using System.IO;
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
        private List<Inventar> razduzeniPredmeti;
        public ZavrsiRazduzivanje(List<Inventar> razduzeniPredmeti)
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
            MessageBox.Show("Stampamo izvestaj");
            using (var doc = new PdfWrapper(path))
            {
                doc.DodajNaslov("Izvestaj o razduzivanju", 14);
                doc.DodajParagraf("Potvrdjujem da sam dana 01.01.2020. godine zaduzio sledeci inventar koji mi se stavlja na zaduzenje ... tekst osmislite samostalno");
                doc.DodajTabelu<Inventar>(razduzeniPredmeti);
                doc.DodajMPiPotpis();
            }

            PdfWrapper.OtvoriFile(path);
        }
    }
}
