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
    /// Interaction logic for ZavrsiZaduzivanje.xaml
    /// </summary>
    public partial class ZavrsiZaduzivanje : Page
    {
        private List<Inventar> zaduzeniPredmeti;
        public ZavrsiZaduzivanje(List<Inventar> zaduzeniPredmeti)
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
            MessageBox.Show("Stampamo izvestaj");
            using (var doc = new PdfWrapper(path))
            {
                doc.DodajNaslov("Izvestaj o zaduzivanju", 14);
                doc.DodajParagraf("Potvrdjujem da sam dana 01.01.2020. godine zaduzio sledeci inventar koji mi se stavlja na zaduzenje ... tekst osmislite samostalno");
                doc.DodajTabelu<Inventar>(zaduzeniPredmeti);
                doc.DodajMPiPotpis();
            }

            PdfWrapper.OtvoriFile(path);
        }
    }
}
