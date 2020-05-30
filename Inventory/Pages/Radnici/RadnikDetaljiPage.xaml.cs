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
    /// Interaction logic for RadnikDetaljiPage.xaml
    /// </summary>
    public partial class RadnikDetaljiPage : Page
    {
        public Radnik radnik { get; set; }
        private bool dodavanjeNovogRadnika;
        public RadnikDetaljiPage()
        {
            InitializeComponent();
            dodavanjeNovogRadnika = true;
            radnik = new Radnik();
            popuniPoljaSaRadnikom();
            tbPassword.Text = "Default lozinka je 'lozinka123'";
        }

        public RadnikDetaljiPage(Radnik r)
        {
            InitializeComponent();
            dodavanjeNovogRadnika = false;
            tbUsername.IsReadOnly = true;
            radnik = r;
            popuniPoljaSaRadnikom();
        }

        private void popuniPoljaSaRadnikom()
        {
            tbUsername.Text = radnik.Username;
            tbPassword.Text = radnik.Password;
            tbJMBG.Text = radnik.JMBG;
            tbIme.Text = radnik.Ime;
            tbPrezime.Text = radnik.Prezime;
            tbStrucnaSprema.Text = radnik.StrucnaSprema;
            cbPol.SelectedIndex = radnik.Pol == "Muski" ? 0 : 1;
            cbIsAdmin.SelectedIndex = radnik.IsAdmin == false ? 0 : 1;
        }

        private void uzmiPodatkeRadnikaIzPolja()
        {
            radnik.Username = tbUsername.Text;
            radnik.Password = Hash.GetStringSha256Hash("lozinka123");
            radnik.JMBG = tbJMBG.Text;
            radnik.Ime = tbIme.Text;
            radnik.Prezime = tbPrezime.Text;
            radnik.StrucnaSprema = tbStrucnaSprema.Text;
            radnik.Pol = cbPol.Text;
            radnik.IsAdmin = cbIsAdmin.SelectedIndex == 1;
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

                    if (dodavanjeNovogRadnika)
                    {
                        db.Add(radnik);
                        db.SaveChanges();
                    }
                    else
                    {
                        Radnik r = db.Radnici.First(r => r.Username == radnik.Username);
                        r.Ime = radnik.Ime;
                        r.Prezime = radnik.Prezime;
                        r.JMBG = radnik.JMBG;
                        r.Pol = radnik.Pol;
                        r.StrucnaSprema = radnik.StrucnaSprema;
                        r.IsAdmin = radnik.IsAdmin;
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
