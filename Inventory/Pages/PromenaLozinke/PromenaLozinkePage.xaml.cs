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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Inventory.Pages.PromenaLozinke
{
    /// <summary>
    /// Interaction logic for PromenaLozinkePage.xaml
    /// </summary>
    public partial class PromenaLozinkePage : Page
    {
        public PromenaLozinkePage()
        {
            InitializeComponent();
        }

        private void PromeniLozinku_Click(object sender, RoutedEventArgs e)
        {
            var trenutniKorisnik = (Application.Current as App).trenutniRadnik;
            if (string.IsNullOrEmpty(staraLozinka.Password) || string.IsNullOrEmpty(novaLozinka1.Password) || string.IsNullOrEmpty(novaLozinka2.Password))
                MessageBox.Show("Morate da popunite sva polja!");
            else if (novaLozinka1.Password != novaLozinka2.Password)
                MessageBox.Show("Nove lozinke se ne poklapaju!");
            else if (trenutniKorisnik.Password != Hash.GetStringSha256Hash(staraLozinka.Password))
                MessageBox.Show("Stara lozinka nije ispravna!");
            else
            {
                using (var db = new InventoryContext())
                {
                    var radnik = db.Radnici.Find(trenutniKorisnik.Username);
                    radnik.Password = Hash.GetStringSha256Hash(novaLozinka1.Password);
                    db.SaveChanges();
                }
                MessageBox.Show("Uspesno promenjena lozinka!");
                NavigationService.GoBack();
            }

        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
