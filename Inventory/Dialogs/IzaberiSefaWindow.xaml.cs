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
using System.Windows.Shapes;

namespace Inventory.Dialogs
{
    /// <summary>
    /// Interaction logic for IzaberiSefaWindow.xaml
    /// </summary>
    public partial class IzaberiSefaWindow : Window
    {
        private Prostorija prostorija;
        public IzaberiSefaWindow(Prostorija p)
        {
            prostorija = p;
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var radnici = db.Radnici.ToList();
                cbRadnici.ItemsSource = radnici.Select(r => r.Username);
                cbRadnici.SelectedIndex = 0;
            }
        }

        private void Potvrdi_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var p = db.Prostorije.Find(prostorija.Id);
                p.SefProstorije = cbRadnici.SelectedItem as Radnik;
                db.SaveChanges();
                DialogResult = true;
            }
        }
    }
}
