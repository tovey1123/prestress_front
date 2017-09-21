using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
using System.Management;


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
            LaunchTimer();
            setPortrait();
            InitInfo();

        }
        private void InitInfo() {
            //获取本机IP地址

            string ServerIp = null;
            ManagementClass mcNetworkAdapterConfig = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc_NetworkAdapterConfig = mcNetworkAdapterConfig.GetInstances();
            foreach (ManagementObject mo in moc_NetworkAdapterConfig)
            {
                string mServiceName = mo["ServiceName"] as string;

                //过滤非真实的网卡  
                if (!(bool)mo["IPEnabled"])
                { continue; }
                if (mServiceName.ToLower().Contains("vmnetadapter")
                 || mServiceName.ToLower().Contains("ppoe")
                 || mServiceName.ToLower().Contains("bthpan")
                 || mServiceName.ToLower().Contains("tapvpn")
                 || mServiceName.ToLower().Contains("ndisip")
                 || mServiceName.ToLower().Contains("sinforvnic"))
                { continue; }


                //bool mDHCPEnabled = (bool)mo["IPEnabled"];//是否开启了DHCP  
                //string mCaption = mo["Caption"] as string;  
                //string mMACAddress = mo["MACAddress"] as string; 
                // foreach (IPAddress addr in mo["IPAddress"])
                //{ if (addr.AddressFamily.ToString() == "InterNetwork")

                string[] mIPAddress = mo["IPAddress"] as string[];
                // }
                //string[] mIPSubnet = mo["IPSubnet"] as string[];  
                //string[] mDefaultIPGateway = mo["DefaultIPGateway"] as string[];  
                //string[] mDNSServerSearchOrder = mo["DNSServerSearchOrder"] as string[];  

                //Console.WriteLine(mDHCPEnabled);  
                //Console.WriteLine(mCaption);  
                //Console.WriteLine(mMACAddress);  
                //PrintArray(mIPAddress);  
                //PrintArray(mIPSubnet);  
                //PrintArray(mDefaultIPGateway);  
                //PrintArray(mDNSServerSearchOrder);  

                if (mIPAddress != null)
                {

                    foreach (string ip in mIPAddress)
                    {

                        if (ip != "0.0.0.0")
                        {
                            //iniFileReader.WriteValue("Login", "IpAddress", ip);
                            ServerIp = ip;
                            MessageBox.Show(ServerIp);
                            break;
                        }
                    }
                }
                mo.Dispose();
            }
        }
    



        private void SetDateTime(object sender, EventArgs e)
        {
            String date = System.DateTime.Now.ToString("yyyy.MM.dd ");
            String time = DateTime.Now.ToLongTimeString();
            user_name.Text = date + time + " " + "当前用户为：" + GlobalVariable.userName;
        }
        private void LaunchTimer()
        {
            DispatcherTimer innerTimer = new DispatcherTimer(TimeSpan.FromSeconds(1.0),
                    DispatcherPriority.Loaded, new EventHandler(this.SetDateTime), this.Dispatcher);
            innerTimer.Start();
        }
        private void logout_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Forms.Application.Restart();
            Application.Current.Shutdown();
            ////clear frame
            //this.function_frame.Content = null;

            ////clear Global data 
            //GlobalVariable.clearData();
           
            ////close database connection
            //connecttoMysql.clearData();

            ////clear all pages
            ////clearPages();

            ////jump to loginpage
            //this.NavigationService.Navigate(new loginPage(GlobalVariable.g_Framemain));
        }


        private void logout_MouseEnter(object sender, MouseEventArgs e)
        {
            Logout.Background = new SolidColorBrush(Color.FromRgb(255, 100, 0));
           
        }

        private void logout_MouseLeave(object sender, MouseEventArgs e)
        {
            Logout.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            //Logout.Opacity = 1;
        }

        

        //private void ShowDetect()
        //{
        //    if (dp == null)
        //    {
        //        dp = new detectPage();
        //        this.function_frame.Content = dp;
        //    }
        //    else this.function_frame.Content = dp;
        //}
        private void homePage_Click(object sender, RoutedEventArgs e) {
            this.function_frame.Content = null;
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

        private void func_hover(object sender, RoutedEventArgs e) {
           // Mouse.
        }

        //    private void clearPages()
        //    {

        //     dp = null;
        //     sp = null;
        //     shp = null;
        //     lhp = null;
        //     up = null;
        //}
        string savePath = Environment.CurrentDirectory + "\\" + GlobalVariable.userNumber.ToString() + ".jpg";
        private void setPortrait() {
            if (File.Exists(savePath))
            {
                //image.Source = new BitmapImage(new Uri(savePath, UriKind.RelativeOrAbsolute));
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                using (Stream ms = new MemoryStream(File.ReadAllBytes(savePath)))
                {
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    portrait.Background = new ImageBrush(bitmap);
                    
                    // bitmap.Freeze();
                }


            }
        }



    }
}
