using Inventory.Dialogs;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Printing;
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
    /// Interaction logic for InventarPage.xaml
    /// </summary>
    public partial class InventarPage : Page
    {
        private Prostorija prostorija;
        public InventarPage(Prostorija p)
        {
            InitializeComponent();
            prostorija = p;
            Naslov.Text = "Inventar prostorije: " + prostorija.NazivProstorije;
            if (string.IsNullOrEmpty(prostorija.UsernameSefa))
                DodelaSefaButton.Visibility = Visibility.Visible;
            else
                DodelaSefaButton.Visibility = Visibility.Collapsed;
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void ZaduziInventar_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            PrintDocument printDoc = new PrintDocument();
            printDoc.DocumentName = "Print Document";
            //Call ShowDialog  

            if (printDlg.ShowDialog() != null) printDoc.Print();
        }

        private void RazduziInventar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DodeliSefaProstorije_Click(object sender, RoutedEventArgs e)
        {
            var window = new IzaberiSefaWindow(prostorija);
            if (window.ShowDialog() == true)
            {
                DodelaSefaButton.Visibility = Visibility.Collapsed;
            }
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var inventar = db.Inventar.Where(i => i.IdProstorije == prostorija.Id).ToList();
                InventarDataGrid.ItemsSource = inventar;
            }
        }
    }
}
