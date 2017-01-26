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
using MySql.Data.MySqlClient;
using System.Data;


namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for userPage.xaml
    /// </summary>
    public partial class userPage : Page
    {
        public userPage()
        {
            InitializeComponent();
            getUserInfo();
            showInfo();
        }

        private void button_edit_Click(object sender, RoutedEventArgs e)
        {
            this.button_edit.Visibility = Visibility.Collapsed;
            this.button_save.Visibility = Visibility.Visible;
            this.button_cancel.Visibility = Visibility.Visible;
            this.Grid_info.Opacity = 1;
            this.grid_changepwd.Opacity = 1;
            this.Grid_info.IsHitTestVisible = true;
            this.grid_changepwd.IsHitTestVisible = true;
        }
        // private static Dictionary<string, object>[] info;
        Dictionary<string, object> userInfo = null;
        private void getUserInfo()
        {
            MySqlConnection mysql = connecttoMysql.getMySqlCon();
            string order = "select * from user_tb";
            MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);
            connecttoMysql.getUserInfo(mySqlCommand);
            userInfo = connecttoMysql.userInfo;
        }

        private void showInfo()
        {
            this.textBox_Number.Text = userInfo["number"].ToString();
        }

    }
}
