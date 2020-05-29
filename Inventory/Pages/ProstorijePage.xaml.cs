﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ProstorijePage.xaml
    /// </summary>
    public partial class ProstorijePage : Page
    {
        public ObservableCollection<Prostorija> Prostorije { get; set; }
        public ProstorijePage(bool isAdmin = false)
        {
            InitializeComponent();
            AdminPanel.Visibility = isAdmin ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new InventoryContext())
            {
                var prostorije = await db.Prostorije.ToListAsync();
                ProstorijeDataGrid.ItemsSource = prostorije;
            }
        }

        private void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Ensure row was clicked and not empty space
            var row = ItemsControl.ContainerFromElement((DataGrid)sender,
                                                e.OriginalSource as DependencyObject) as DataGridRow;
            if (row == null) 
                return;

            //MessageBox.Show(row.Item.ToString());
            NavigationService.Navigate(new InventarPage(row.Item as Prostorija));
        }

        private void Nazad_Click(object sender, RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
        }

        private void Radnici_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new RadniciPage());
        }

        private void ObrisiProstoriju_Click(object sender, RoutedEventArgs e)
        {
            if (ProstorijeDataGrid.SelectedItem != null)
            {
                Prostorija selektovanaProstorija = ProstorijeDataGrid.SelectedItem as Prostorija;

                var dialogResult = MessageBox.Show("Da li ste sigurni da zelite da izbrisete prostoriju " + selektovanaProstorija.NazivProstorije, "Brisanje", MessageBoxButton.YesNo);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    using (var db = new InventoryContext())
                    {
                        db.Remove(selektovanaProstorija);
                        db.SaveChanges();
                    }
                    // Da osvezim listu radnika
                    Page_Loaded(null, null);
                }
            }
            else
            {
                MessageBox.Show("Morate da selektujete prostoriju koju zelite da obrisete!");
            }
        }

        private void IzmeniProstoriju_Click(object sender, RoutedEventArgs e)
        {
            if (ProstorijeDataGrid.SelectedItem != null)
            {
                Prostorija selektovanaProstorija = ProstorijeDataGrid.SelectedItem as Prostorija;
                NavigationService.Navigate(new ProstorijaDetaljiPage(selektovanaProstorija));
            }
            else
            {
                MessageBox.Show("Morate da selektujete prostoriju koju zelite da menjate!");
            }
        }

        private void DodajProstoriju_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ProstorijaDetaljiPage());
        }
    }
}
