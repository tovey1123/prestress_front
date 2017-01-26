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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for registWindow.xaml
    /// </summary>
    public partial class registWindow : Window
    {
        public registWindow()
        {
            InitializeComponent();
        }


        private void button_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_ok_Click(object sender, RoutedEventArgs e)
        {
           // MessageBox.Show((textBox_phone.Text == "").ToString());


            if (textBox_ID.Text == "" || textBox_number.Text == "" || textBox_password.Text == "" || textBox_password2.Text == "")
            {
                MessageBox.Show("请把必填信息填写完整");
            }
            else
            {
                if (!textBox_password.Text.Equals(textBox_password2.Text))       
                {
                        MessageBox.Show("两次密码输入不一致");                     
                }
                else
                {
                    MySqlConnection mysql = connecttoMysql.getMySqlCon();
                    string order = "select * from user_tb";
                    MySqlCommand mySqlCommand = new MySqlCommand(order, mysql);
                    connecttoMysql.getLoginResult(mySqlCommand);
                    if (checkNumber())
                    {
                        MessageBox.Show("此用户已注册");
                    }
                    else  //correct, add a recond
                    {
                        int number =Convert.ToInt32(textBox_number.Text) ;
                        string ID = "'" + textBox_ID .Text + "'"; ;
                        string pwd = "'" + textBox_password.Text + "'"; ;
                        string gender = (comboBox.Text .Length==0) ? "NULL" :"'"+ comboBox.Text + "'";
                        string phone = (textBox_phone.Text .Length==0) ? "NULL" : "'" + textBox_phone.Text + "'";
                        string birthday= (textBox_birthday.Text .Length==0) ? "NULL" : "'"+textBox_birthday.Text + "'";
                        string department = (textBox_department.Text .Length==0) ? "NULL" : "'"+textBox_department.Text + "'";
                        string order2 = "insert into user_tb  values("+ number+","+ID+","+pwd+","+gender+","+phone+","+birthday+","+department+")";
                        MySqlCommand SqlCommandInsert = new MySqlCommand(order2, mysql);
                        try
                        {
                            SqlCommandInsert.ExecuteNonQuery();
                            MessageBox.Show("注册成功");
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            String message = ex.Message;
                           MessageBox.Show("插入数据失败了！" + message);
                        }
                    }
                }
            }
            connecttoMysql.input.Clear();

        }

        private Boolean checkNumber()
        {
            bool numberExist = false;
            foreach (var id in connecttoMysql.input)
            {
                if (id.Key == Convert.ToInt32(textBox_number.Text))
                {
                    numberExist = true;
                    return numberExist;
                }
            }
            return numberExist;
        }

    }
}
