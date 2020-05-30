using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Inventory.Dialogs
{
    /// <summary>
    /// Interaction logic for DodajInventar.xaml
    /// </summary>
    public partial class DodajInventar : Window
    {
        private Inventar inventar;
        public DodajInventar(Inventar inventar)
        {
            this.inventar = inventar;
            InitializeComponent();
        }

        private void Dodaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                inventar.Naziv = tbNaziv.Text;
                inventar.Marka = tbMarka.Text;
                inventar.Model = tbMarka.Text;
                inventar.Cena = int.Parse(tbCena.Text);
                inventar.Kolicina = int.Parse(tbKolicina.Text);
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Proverite unete podatke i pokusajte ponovo! \nCena i kolicina moraju biti brojevi!");
            }
        }

        private void Odbaci_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
