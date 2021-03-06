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

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for Weather_preview.xaml
    /// </summary>
    public partial class Weather_preview : Page
    {
        public Weather_preview()
        {
            InitializeComponent();
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.White);
        }
    }
}
