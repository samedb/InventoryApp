﻿using System;
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
    /// Interaction logic for ZaduzivanjePage.xaml
    /// </summary>
    public partial class ZaduzivanjePage : Page
    {
        public ZaduzivanjePage(List<Inventar> inventar)
        {
            InitializeComponent();
            InventarDataGrid.ItemsSource = inventar;
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void ObrisiRed_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as Button).DataContext as Inventar;
            var list = InventarDataGrid.ItemsSource.Cast<Inventar>().ToList();
            list.Remove(selectedItem);
            InventarDataGrid.ItemsSource = list;
        }
        private void Zaduzi_Click(object sender, RoutedEventArgs e)
        {
            var zaZaduzivanje = InventarDataGrid.Items.Cast<Inventar>().ToList();
            if (zaZaduzivanje.Count == 0)
            {
                MessageBox.Show("Lista mora da sadrzi minimum jedan element!");
                return;
            }

            using (var db = new InventoryContext())
            {
                var sviPredmeti = db.Inventar.Where(i => i.IdProstorije == zaZaduzivanje[0].IdProstorije).ToList();
                foreach (var predmet in zaZaduzivanje)
                {
                    var predmetUBazi = sviPredmeti.First(p => p.Id == predmet.Id);
                    if (predmetUBazi.Kolicina < predmet.Kolicina)
                    {
                        MessageBox.Show("Ne mozete da razduzite vise predmeta nego sto se nalazi u inventaru!");
                        return;
                    }
                    else if (predmetUBazi.Kolicina == predmet.Kolicina)
                    {
                        // ako hoce sve da uzme onda ga izbrisi iz baze
                        db.Remove(predmetUBazi);
                    }
                    else
                    {
                        predmetUBazi.Kolicina -= predmet.Kolicina;
                    }
                }
                db.SaveChanges();
            }

            // Predji na page gde se stampa izvestaj
            NavigationService.Navigate(new ZavrsiZaduzivanje(zaZaduzivanje));
        }
    }
}