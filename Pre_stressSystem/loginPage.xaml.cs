using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Security.Cryptography;
using System.Management;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;


namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class loginPage : Page
    {

        private Frame m_parent = null;
        private JObject weather = null;
        public loginPage(Frame parent)
        {
            this.m_parent = parent;
            InitializeComponent();
            this.textBox_username.Focus();
            if (isConnected2Intnet())
            {
                LoadIni();  //加载初始化信息--账号,密码,版本信息,并且提前获取天气信息
            }
            else
            {
                MessageBox.Show("请检查网络");
            }

        }



        //login_click
        private void button_login_Click(object sender, RoutedEventArgs e)
        {

            connecttoMysql.getLoginResult();

            #region check login

            if (passwordBox.Password == "" || textBox_username.Text == "")
            {
                MessageBox.Show("用户名或密码为空！");
            }
            else
            {
                if (!checkID())
                {
                    MessageBox.Show("用户名不存在！");
                }

                else
                {
                    if (!checkpwd())
                    {
                        MessageBox.Show("密码输入错误！");
                    }
                    else
                    {

                        writeIntoIniFile();
                        m_parent.Content = new mainPage(weather);

                    }
                }
            }
            #endregion
            connecttoMysql.input.Clear();
        }

        //press "Enter", the same as login_click
        private void page_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter: button_login_Click(sender, e); break;
                //case Key.Tab: ChangeFoucs(); break;


            }
        }   

 
        //检查工号是不是存在
        private Boolean checkID()
        {
            bool numberExist = false;
            
            if (connecttoMysql.input.ContainsKey(Convert.ToInt32(textBox_username.Text)))
            { numberExist = true; }
            return numberExist;
        }
        private Boolean checkpwd()
        {

            KeyValuePair<string, string> namepwd = new KeyValuePair<string, string>();
            bool idcorrect = false;
            foreach (var item in connecttoMysql.input)
            {
                if (item.Key== Convert.ToInt32(textBox_username.Text))
                {

                    namepwd=item.Value;
                 if (namepwd.Value== passwordBox.Password)
                    {

                        idcorrect = true;
                        GlobalVariable.userName = namepwd.Key;
                        GlobalVariable.userNumber = Convert.ToInt32(textBox_username.Text);
                        return idcorrect;
                    }
                }
            }
            return idcorrect;
        }

        private void label_regsit_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Window reg = new registWindow();
            reg.ShowDialog();
        }

        //加载配置信息
        private void LoadIni()
        {
            //GetIP();
            weather = getWeather();
            InitFile initFileReader = new InitFile(System.Environment.CurrentDirectory + "\\InitFile.ini");
            this.textBox_username.Text = initFileReader.ReadValue("Login", "user");
            Boolean isSelected = false;
            try
            {
                isSelected = Convert.ToBoolean(initFileReader.ReadValue("Login", "rememberPwd"));
            }catch(FormatException)
            {
                isSelected = false;
            }
            if (isSelected)
            {
                this.savePassword.IsChecked = true;
                this.passwordBox.Password = RSADecrypt(initFileReader.ReadValue("Login", "passworld"));

            }
        }

       //把信息写入INI文件
        private void writeIntoIniFile()
        {
            InitFile initFileWriter = new InitFile(System.Environment.CurrentDirectory + "\\InitFile.ini");
            initFileWriter.WriteValue("Login", "user", this.textBox_username.Text.PadLeft(4,'0'));
            initFileWriter.WriteValue("Login", "passworld", RSAEncrypt(this.passwordBox.Password));
            initFileWriter.WriteValue("Login", "rememberPwd", this.savePassword.IsChecked.ToString());
        }
    
        //RSA加密
        public static string RSAEncrypt(string content)
        {
           string publickey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv+
                            SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e
                            +BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=
                            </Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publickey);
            byte[] cipherbytes;
            cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(content), false);
            return Convert.ToBase64String(cipherbytes);

        }

        public static string RSADecrypt(string content)
        {
            string privatekey = @"<RSAKeyValue><Modulus>5m9m14XH3oqLJ8bNGw9e4rGpXpcktv9MSkHSVFVMjHbfv
                                +SJ5v0ubqQxa5YjLN4vc49z7SVju8s0X4gZ6AzZTn06jzWOgyPRV54Q4I0DCYadWW4Ze3e
                                +BOtwgVU1Og3qHKn8vygoj40J6U85Z/PTJu3hN1m75Zr195ju7g9v4Hk=
                                </Modulus>
                                <Exponent>AQAB</Exponent>
                                <P>/hf2dnK7rNfl3lbqghWcpFdu778hUpIEBixCDL5WiBtpkZdpSw90aERmHJYaW2RGvGRi6zSftLh00KHsPcNUMw==</P>
                                <Q>6Cn/jOLrPapDTEp1Fkq+uz++1Do0eeX7HYqi9rY29CqShzCeI7LEYOoSwYuAJ3xA/DuCdQENPSoJ9KFbO4Wsow==</Q>
                                <DP>ga1rHIJro8e/yhxjrKYo/nqc5ICQGhrpMNlPkD9n3CjZVPOISkWF7FzUHEzDANeJfkZhcZa21z24aG3rKo5Qnw==</DP>
                                <DQ>MNGsCB8rYlMsRZ2ek2pyQwO7h/sZT8y5ilO9wu08Dwnot/7UMiOEQfDWstY3w5XQQHnvC9WFyCfP4h4QBissyw==</DQ>
                                <InverseQ>EG02S7SADhH1EVT9DD0Z62Y0uY7gIYvxX/uq+IzKSCwB8M2G7Qv9xgZQaQlLpCaeKbux3Y59hHM+KpamGL19Kg==</InverseQ>
                                <D>vmaYHEbPAgOJvaEXQl+t8DQKFT1fudEysTy31LTyXjGu6XiltXXHUuZaa2IPyHgBz0Nd7znwsW/S44iql0Fen1kzKioEL3svANui63O3o5xdDeExVM6zOf1wUUh/oldovPweChyoAdMtUzgvCbJk1sYDJf++Nr0FeNW1RB1XG30=</D>
                                </RSAKeyValue>";
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            byte[] cipherbytes;
            rsa.FromXmlString(privatekey);
            cipherbytes = rsa.Decrypt(Convert.FromBase64String(content), false);

            return Encoding.UTF8.GetString(cipherbytes);
        }


        private void GetIP()
        {
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
                            //getWeather(ServerIp);
                            break;
                        }
                    }
                }
                mo.Dispose();
            }
        }

        private JObject getWeather()
        {
            string location = getLocation();
            string weather_URI = "http://wthrcdn.etouch.cn/weather_mini?city=" + location;
            JObject jo = (JObject)JsonConvert.DeserializeObject(GetWebResponseString(weather_URI, UTF8Encoding.UTF8, true));
            return jo;

        }

        private string getLocation()
        {
            string IPLocation_URI = "http://api.map.baidu.com/location/ip?ak=jeiS1mDguIqIp9TBdrWLMSotBYSSZnWZ&coor=bd09ll";
            JObject jo = (JObject)JsonConvert.DeserializeObject(GetWebResponseString(IPLocation_URI, Encoding.UTF8, false));
            if (jo["status"].ToString() != "0")
            {
                return "status_error";
            }
            else
            {
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

        private string GetWebResponseString(string strUrl, Encoding encode, bool Decompress)
        {
               Uri uri = new Uri(strUrl);
               WebRequest webreq = WebRequest.Create(uri);
               Stream s = Decompress ? new System.IO.Compression.GZipStream(webreq.GetResponse().GetResponseStream(), System.IO.Compression.CompressionMode.Decompress)
                                     : webreq.GetResponse().GetResponseStream();
               StreamReader sr = new StreamReader(s, encode);
               string all = sr.ReadToEnd();         //读取网站返回的数据
               return all;         
        }

        [DllImport("wininet")]
        private static extern bool InternetGetConnectedState(out int connectionDescription, int reservedValue);

        private  bool isConnected2Intnet()
        {
            int i = 0;

            if (InternetGetConnectedState(out i, 0))

            {
                //已联网
                return true;
            }
            else

            {
                //未联网
                return false;
            }

        }

    }
}
