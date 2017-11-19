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
using System.Text.RegularExpressions;


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
               // MessageBox.Show(("wzw" + "" + "spy").Length.ToString());
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

            ConnecttoMysql.getLoginResult();

            #region check login

            if (passwordBox.Password == "" || textBox_username.Text == "")
            {
                MessageBox.Show("用户名或密码为空！");
            }
            else
            {


                //if (!checkID())
                //{
                //    MessageBox.Show("用户名不存在！");
                //}

                //else
                //{
                //    if (!checkpwd())
                //    {
                //        MessageBox.Show("密码输入错误！");
                //    }
                //    else
                //    {

                //        writeIntoIniFile();
                //        m_parent.Content = new mainPage(weather);

                //    }
                //}


                //先获取公钥，将密码加密发送
              
                
                string getKey = "http://localhost:8080/spring_MyServer/getPublicKey";
                string modexp = MyHttpRequest.GetWebResponse_Get(getKey, Encoding.UTF8, false);

                //服务器返回的是字符串会把换行转换为\r\n , C#端会字符串就成了\\r\\n；  需重新转换为换行
                MatchEvaluator match = delegate (Match m)
                {
                    return "\r\n";
                };
                string[] me = Regex.Replace(modexp.Substring(1, modexp.Length - 2), @"\\r\\n", match).Split(',');   // \\r\\n用\r\n代替

                //向服务器发送账号密码              
                //密码用RSA加密
                try
                {
                    InitFile initFileWriter = new InitFile(System.Environment.CurrentDirectory + "\\InitFile.ini");
                   

                    string severIP = "http://localhost:8080/spring_MyServer/login";
                    RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                    RSAParameters para = new RSAParameters();
                    initFileWriter.WriteValue("Test", "mod", me[0].Substring(0, me[0].Length - 2));
                    initFileWriter.WriteValue("Test", "exp", me[1].Substring(0, me[1].Length - 2));

                    para.Modulus =  Convert.FromBase64String(me[0].Substring(0, me[0].Length - 2));
                    para.Exponent = Convert.FromBase64String(me[1].Substring(0, me[1].Length - 2));
                    rsa.ImportParameters(para);
                    byte[] enBytes = rsa.Encrypt(UTF8Encoding.UTF8.GetBytes(passwordBox.Password), false);
                    string pwd = Convert.ToBase64String(enBytes);
                    string postData = "ID=" + textBox_username.Text + "&PWD=" + pwd;
                    string response = MyHttpRequest.GetWebResponse_Post(severIP, Encoding.UTF8, false, postData);

                    JObject jo = (JObject)JsonConvert.DeserializeObject(response);

                    if (jo["errCode"].ToString() == "1")
                    {
                        MessageBox.Show("账号不存在！请重试", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else if (jo["errCode"].ToString() == "2")
                    {
                        MessageBox.Show("密码错误！请重试", "", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else {
                        UserInfo.employee_id = "1234";
                        UserInfo.employee_id   = jo["data"]["employee_id"].ToString();
                        UserInfo.employee_name = jo["data"]["employee_name"].ToString();
                        UserInfo.employee_pwd  = jo["data"]["employee_pwd"].ToString();
                        UserInfo.gender = jo["data"]["gender"] == null ? null : jo["data"]["gender"].ToString();
                        UserInfo.phone = jo["data"]["phone"] == null ? null : jo["data"]["phone"].ToString();
                        UserInfo.birthday = jo["data"]["birthday"] == null ? null : jo["data"]["birthday"].ToString();
                        UserInfo.department = jo["data"]["department"] == null ? null : jo["data"]["department"].ToString();
                        UserInfo.Email = jo["data"]["Email"] == null ? null : jo["data"]["Email"].ToString();
                        UserInfo.address = jo["data"]["address"] == null ? null : jo["data"]["address"].ToString();
                        UserInfo.lever = jo["data"]["lever"] == null ? null : jo["data"]["lever"].ToString();

                        writeIntoIniFile();
                        m_parent.Content = new mainPage(weather);

                    }
                }
                catch(Exception exc)                
                {
                    MessageBox.Show(exc.Message);
                }



            }
            #endregion
            ConnecttoMysql.input.Clear();
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
        //private Boolean checkID()
        //{
        //    bool numberExist = false;
            
        //    if (ConnecttoMysql.input.ContainsKey(Convert.ToInt32(textBox_username.Text)))
        //    { numberExist = true; }
        //    return numberExist;
        //}

        //校验密码
        //private Boolean checkpwd()
        //{

        //    KeyValuePair<string, string> namepwd = new KeyValuePair<string, string>();
        //    bool idcorrect = false;
        //    foreach (var item in ConnecttoMysql.input)
        //    {
        //        if (item.Key== Convert.ToInt32(textBox_username.Text))
        //        {

        //            namepwd=item.Value;
        //         if (namepwd.Value== passwordBox.Password)
        //            {

        //                idcorrect = true;
        //                GlobalVariable.userName = namepwd.Key;
        //                GlobalVariable.userNumber = Convert.ToInt32(textBox_username.Text);
        //                return idcorrect;
        //            }
        //        }
        //    }
        //    return idcorrect;
        //}

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


        //内网IP  暂时没有用
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
            JObject jo = (JObject)JsonConvert.DeserializeObject(MyHttpRequest.GetWebResponse_Get(weather_URI, UTF8Encoding.UTF8, true));
            return jo;

        }

        private string getLocation()
        {
            string IPLocation_URI = "http://api.map.baidu.com/location/ip?ak=jeiS1mDguIqIp9TBdrWLMSotBYSSZnWZ&coor=bd09ll";
            JObject jo = (JObject)JsonConvert.DeserializeObject(MyHttpRequest.GetWebResponse_Get(IPLocation_URI, Encoding.UTF8, false));
            if (jo["status"].ToString() != "0")
            {
                MessageBox.Show("无法定位您的IP地址，请检查是否开启了代理","定位失败",MessageBoxButton.OK,MessageBoxImage.Error);
                return "北京";
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

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //var client = new RestClient("http://localhost:8080/spring_MyServer/getPublicKey");
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("postman-token", "30f260fa-5bc6-6903-5b0d-1a6a126dd96d");
            //request.AddHeader("cache-control", "no-cache");
            //request.AddHeader("content-type", "application/x-www-form-urlencoded");
            //request.AddParameter("application/x-www-form-urlencoded", "word=12345", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);

        
            //string severIP = "http://localhost:8080/spring_MyServer/query";
            //string re = MyHttpRequest.GetWebResponse_Post(severIP, Encoding.UTF8, false, "word1=施颖培&word2=1314王志文");
            //MessageBox.Show(re);
        }
    }
}
