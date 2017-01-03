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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GlobalVariable.g_mainWindow = this;
            GlobalVariable.g_Framemain = this.Frame_main;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Frame_main.Content = new loginPage(Frame_main);
        }
    }
}
