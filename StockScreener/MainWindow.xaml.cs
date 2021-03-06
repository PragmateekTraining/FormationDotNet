﻿using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StockScreener
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public StockScreenerViewModel Model { get; set; }

        public MainWindow()
        {
            Model = new StockScreenerViewModel(new Market());

            InitializeComponent();

            DataContext = this;
        }

        private void BindableDataSeries_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {

        }

        private void BindableDataSeries_Loaded_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
