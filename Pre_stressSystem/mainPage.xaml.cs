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

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for mainPage.xaml
    /// </summary>
    public partial class mainPage : Page
    {
        public detectPage dp = null;
        public sensorPage sp = null;
        public sensorhistoryPage shp = null;
        public linehistoryPage lhp = null;
        public userPage up = null;

        public mainPage()
        {
            InitializeComponent();
            String datetime= System.DateTime.Now.ToString("yyyy/MM/dd");
            user_name.Text = datetime+" "+ "当前用户为：" + GlobalVariable.userName;
            ShowDetect();
        }


        private void logout_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //clear data 
            GlobalVariable.userName = "";
            GlobalVariable.userNumber = null;
            connecttoMysql.cleardata();
            //jump to loginpage
            this.NavigationService.Navigate(new loginPage(GlobalVariable.g_Framemain));
        }


        private void logout_MouseEnter(object sender, MouseEventArgs e)
        {
            logout.Opacity = 0.5;
        }

        private void logout_MouseLeave(object sender, MouseEventArgs e)
        {
            logout.Opacity = 1;
        }


        private void ShowDetect()
        {
            if (dp == null)
            {
                dp = new detectPage();
                this.function_frame.Content = dp;
            }
            else this.function_frame.Content = dp;
        }

        private void dectct_Click(object sender, RoutedEventArgs e)
        {
            if(dp==null)
            {
                 dp= new detectPage();
                 this.function_frame.Content = dp;
            }
            else this.function_frame.Content = dp;


        }

        private void sensor_Click(object sender, RoutedEventArgs e)
        {
            if (sp == null)
            {
                sp = new sensorPage();
                this.function_frame.Content = sp;
            }
            else this.function_frame.Content = sp;
        }

        private void sensor_history_Click(object sender, RoutedEventArgs e)
        {
            if (shp == null)
            {
                shp = new sensorhistoryPage();
                this.function_frame.Content = shp;
            }
            else this.function_frame.Content = shp;
        }

        private void line_history_Click(object sender, RoutedEventArgs e)
        {
            if (lhp == null)
            {
                lhp = new linehistoryPage();
                this.function_frame.Content = lhp;
            }
            else this.function_frame.Content = lhp;
        }

        private void user_managment_Click(object sender, RoutedEventArgs e)
        {
            if (up == null)
            {
                up = new userPage();
                this.function_frame.Content = up;
            }
            else this.function_frame.Content = up;
        }
    }
}
