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

                       // GlobalVariable.userName = textBox_username.Text;
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
    }
}
