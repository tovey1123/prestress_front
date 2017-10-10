using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.IO;
//using mshtml;
using System.Management;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;


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
        private JObject weather = null;
        private ObservableCollection<WeatherInfo> weatherList = null;
        int Selected = 1;

        public mainPage(JObject weatherJo)
        {
            weather = weatherJo;
            InitializeComponent();
            LaunchTimer();
            setPortrait();
            setWeather(weather);

        }




        private void SetDateTime(object sender, EventArgs e)
        {
            String date = System.DateTime.Now.ToString("yyyy年MM月dd日 ");
            String time = DateTime.Now.ToLongTimeString();
            user_name.Text = GlobalVariable.userName;
            date_time.Text = date + time;
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


        private void homePage_Click(object sender, RoutedEventArgs e) {
            this.function_frame.Content = null;
            this.bg2.Visibility = Visibility.Visible;
            this.weather_p.Visibility = Visibility.Visible;
            select(1);
        }

        private void dectct_Click(object sender, RoutedEventArgs e)
        {
            if (dp == null)
            {
                dp = new detectPage();
                this.function_frame.Content = dp;
            }
            else this.function_frame.Content = dp;
            this.bg2.Visibility = Visibility.Collapsed;
            this.weather_p.Visibility = Visibility.Collapsed;
            // (sender as Border).Background = new SolidColorBrush(Colors.OrangeRed);
            select(2);

        }

        private void sensor_Click(object sender, RoutedEventArgs e)
        {
            if (sp == null)
            {
                sp = new sensorPage();
                this.function_frame.Content = sp;
            }
            else this.function_frame.Content = sp;
            this.bg2.Visibility = Visibility.Collapsed;
            this.weather_p.Visibility = Visibility.Collapsed;
            select(3);
        }

        private void sensor_history_Click(object sender, RoutedEventArgs e)
        {
            if (shp == null)
            {
                shp = new sensorhistoryPage();
                this.function_frame.Content = shp;
            }
            else this.function_frame.Content = shp;
            this.bg2.Visibility = Visibility.Collapsed;
            this.weather_p.Visibility = Visibility.Collapsed;
            select(4);
        }

        private void line_history_Click(object sender, RoutedEventArgs e)
        {
            if (lhp == null)
            {
                lhp = new linehistoryPage();
                this.function_frame.Content = lhp;
            }
            else this.function_frame.Content = lhp;
            this.bg2.Visibility = Visibility.Collapsed;
            this.weather_p.Visibility = Visibility.Collapsed;
            select(5);
        }

        private void user_managment_Click(object sender, RoutedEventArgs e)
        {
            if (up == null)
            {
                up = new userPage();
                this.function_frame.Content = up;
            }
            else this.function_frame.Content = up;
            this.bg2.Visibility = Visibility.Collapsed;
            this.weather_p.Visibility = Visibility.Collapsed;
            select(6);
        }

        private void select(int newSelected) {
            if(newSelected != Selected)
            {
                switch (Selected)
                {
                    case 1:
                        this.homePage.Background =null;
                        break;
                    case 2:
                        this.detect.Background = null;
                        break;
                    case 3:
                        this.sensor.Background = null;
                        break;
                    case 4:
                        this.sensor_history.Background = null; 
                        break;
                    case 5:
                        this.line_history.Background = null;
                        break;
                    case 6:
                        this.user_management.Background = null;
                        break;
                }
                switch (newSelected)
                {
                    case 1:
                        this.homePage.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                    case 2:
                        this.detect.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                    case 3:
                        this.sensor.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                    case 4:
                        this.sensor_history.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                    case 5:
                        this.line_history.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                    case 6:
                        this.user_management.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF546E58"));
                        break;
                }

                Selected = newSelected;

            }
            

        }
        private void func_hover(object sender, RoutedEventArgs e) {
            // Mouse.
        }

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


        private void setWeather(JObject jo)
        {
            this.city.TextDecorations = TextDecorations.Underline;
            string month = System.DateTime.Now.ToString("MM");
            int i = 1;
            if (this.weatherList == null)
            {
                this.weatherList = new ObservableCollection<WeatherInfo>();
            }
            else
            {
                this.weatherList.Clear();
            }

            if (jo["desc"].ToString() != "OK")
            {
                setTodayWeather(null,"error");
            }
            else {
                foreach (var item in jo["data"]["forecast"])
                {
                    if (i == 1)
                    {
                        setTodayWeather(item,null);
                    }
                    else
                    {
                        var ls = new WeatherInfo();
                        string temp_date = item["date"].ToString();
                        int index = temp_date.IndexOf("日");
                        ls.date = temp_date.Substring(0, index+1);
                        ls.week = temp_date.Substring(index + 1, 3);
                        ls.high = item["high"].ToString().Substring(2);
                        ls.low = item["low"].ToString().Substring(2);
                        ls.direction = item["fengxiang"].ToString();
                        ls.type = item["type"].ToString();
                        ls.lever = item["fengli"].ToString().Length == 0 ? "/" : getLever(item["fengli"].ToString());
                        ls.icoPath = getIcoPath(item["type"].ToString());
                        this.weatherList.Add(ls);
                    }
                    i++;
                }
            }
            lstRes.ItemTemplate = (DataTemplate)this.FindResource("weatherTemplate");
            lstRes.ItemsSource = this.weatherList;
            extraProcess(jo);
           
        }
        private void extraProcess(JObject jo)
        {
            int hour = Convert.ToInt32(DateTime.Now.ToString("HH"));
            if ( hour < 6)
            {
                this.greet.Text = "凌晨好:";
            }
            else if (hour >= 6 && hour < 9)
            {
                this.greet.Text = "早上好:";
            }
            else if (hour >= 9 && hour < 11)
            {
                this.greet.Text = "上午好:";
            }
            else if (hour >= 11 && hour < 14)
            {
                this.greet.Text = "中午好:";
            }
            else if (hour >= 14 && hour < 18)
            {
                this.greet.Text = "下午好:";
            }
            else if (hour >= 18 )
            {
                this.greet.Text = "晚上好:";
            }
            this.tmp.Text = jo["data"]["wendu"].ToString();
            this.tip.Text = "温馨提示："+jo["data"]["ganmao"].ToString();
            this.city.Text = jo["data"]["city"].ToString();
        }
        

        private string getLever(string fengji) {
            Regex re = new Regex(@"((\d+-)?\d+级)");
            Match match = re.Match(fengji);
            return match.Groups[1].ToString();

        }

        private string  getIcoPath(string type) {
            string pre = "picture/weatherico/";
            if (type.Contains("转")) {
                int index = type.IndexOf("转");
                type = type.Substring(index + 1);
            }
            switch (type)
            {
                case "晴": return pre+"晴.png";
                case "多云": return pre + "多云.png";
                case "阴": return pre + "阴天.png";
                case "小雨": return pre + "小雨.png";
                case "阵雨": return pre + "阵雨.png";
                case "中雨": return pre + "中雨.png";
                case "大雨": return pre + "大雨.png";
                case "暴雨": return pre + "暴雨.png";
                case "雷阵雨": return pre + "雷阵雨.png";
                case "小雪": return pre + "小雪.png";
                case "阵雪": return pre + "阵雪.png";
                case "中雪": return pre + "中雪.png";
                case "大雪": return pre + "大雪.png";
                case "暴雪": return pre + "暴雪.png";
                case "雨夹雪": return pre + "雨夹雪.png";
                case "雷阵雨并伴有冰雹": return pre + "雷阵雨并伴有冰雹.png";
                case "冰雹": return pre + "冰雹.png";
                case "雾": return pre + "雾霾.png";
                case "霾": return pre + "雾霾.png";
                case "沙尘暴": return pre + "沙城暴.png";
                default: return pre + "na.png";

            }
        }


        private string getLocation()
        {
            string IPLocation_URI = "http://api.map.baidu.com/location/ip?ak=jeiS1mDguIqIp9TBdrWLMSotBYSSZnWZ&coor=bd09ll";
            JObject jo = (JObject)JsonConvert.DeserializeObject(GetWebResponseString(IPLocation_URI, Encoding.UTF8, false));
            if (jo["status"].ToString() != "0")
            {
                return "status_error";
            }
            else {
                #region  根据经纬度获取的位置信息
                //string lat = jo["content"]["point"]["y"].ToString(); //30.9299
                //string lng = jo["content"]["point"]["x"].ToString(); //120.1234
                //string Geocoding_URI = "http://api.map.baidu.com/geocoder/v2/?callback=renderReverse&location="+lat+","+lng+ "&output=json&pois=1&ak=jeiS1mDguIqIp9TBdrWLMSotBYSSZnWZ";
                //string tmp = GetWebResponseString(Geocoding_URI, Encoding.UTF8);
                //int startIndex = tmp.IndexOf("(");
                //int endIndex = tmp.LastIndexOf(")");
                //string result = tmp.Substring(startIndex + 1, endIndex - startIndex - 1);
                //JObject jo2 = (JObject)JsonConvert.DeserializeObject(result);
                //if (jo2["status"].ToString() != "0")
                //{
                //    return jo["content"]["address_detail"]["city"].ToString();  //无法精确得到县级以下位置，则返回市
                //}
                //else
                //{
                //    string formatted_address = jo2["result"]["formatted_address"].ToString();
                //    return  formatted_address; 
                //}
                #endregion
                if (jo["content"]["address_detail"]["district"].ToString() != "")
                {
                    return jo["content"]["address_detail"]["district"].ToString();
                }
                else
                {
                    return jo["content"]["address_detail"]["city"].ToString();
                }

            }
        }

        private string GetWebResponseString(string strUrl, Encoding encode, bool Decompress) {
            Uri uri = new Uri(strUrl);
            WebRequest webreq = WebRequest.Create(uri);
            Stream s = Decompress ? new System.IO.Compression.GZipStream(webreq.GetResponse().GetResponseStream(), System.IO.Compression.CompressionMode.Decompress)
                                 : webreq.GetResponse().GetResponseStream();
            StreamReader sr = new StreamReader(s, encode);
            string all = sr.ReadToEnd();         //读取网站返回的数据
            return all;
        }

        private void setTodayWeather(JToken item,string status)
        {
            if (status == "error")
            {

            }
            else
            {
                //var item = jo["data"]["forecast"].Next;
                this.high.Text = item["high"].ToString().Substring(2);
                this.low.Text = item["low"].ToString().Substring(2);
                this.direction.Text = item["fengxiang"].ToString();
                this.type.Text = item["type"].ToString();
                this.lever.Text = item["fengli"].ToString().Length == 0 ? "/" : getLever(item["fengli"].ToString());
                this.ico_today.Source = new BitmapImage(new Uri(getIcoPath(item["type"].ToString()), UriKind.RelativeOrAbsolute)); 
            }
        }


    }
}
