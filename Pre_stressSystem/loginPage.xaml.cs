using System;
using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MySql.Data.MySqlClient;
using System.Security.Cryptography;


namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class loginPage : Page
    {

        private Frame m_parent = null;
        public loginPage(Frame parent)
        {
            this.m_parent = parent;
            InitializeComponent();
            this.textBox_username.Focus();
            LoadIni();  //加载初始化信息--账号,密码,版本信息等

        }



        //login_click
        private void button_login_Click(object sender, RoutedEventArgs e)
        {
            //if(!connecttoMysql.hasSearched)

            #region connect to mysql

                MySqlConnection mysql = connecttoMysql.getMySqlCon();
                string order = "select * from user_tb";
                MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);
                connecttoMysql.getLoginResult(mySqlCommand);

            #endregion
            

            #region check login

            if (passwordBox.Password == "" || textBox_username.Text == "")
            {
                MessageBox.Show("用户名或密码为空！");
            }
            else
            {
                if (!checknumber())
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
                        m_parent.Content = new mainPage();

                    }
                }
            }
            #endregion
            connecttoMysql.input.Clear();
            mysql.Close();
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
        private Boolean checknumber()
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



    }
}
