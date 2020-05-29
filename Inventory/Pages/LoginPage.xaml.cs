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

namespace Inventory.Pages
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var username = tbUsername.Text;
            var password = tbPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                txtGreska.Text = "Morate da popunite sva polja";
                return;
            }

            using (var db = new InventoryContext())
            {
                var result = db.Radnici.Find(username);
                var hash = Hash.GetStringSha256Hash(password);
                if (result != null && hash == result.Password)
                {
                    this.NavigationService.Navigate(new ProstorijePage(result.IsAdmin));
                }
                else
                {
                    txtGreska.Text = "Neispravna lozinka ili korisnicko ime";
                }
            }
        }
    }
}
