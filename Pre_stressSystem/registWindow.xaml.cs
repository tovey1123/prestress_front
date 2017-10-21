using System;
using System.Windows;

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
                    
                    connecttoMysql.getLoginResult();
                    if (checkNumber())
                    {
                        MessageBox.Show("此用户已注册");
                    }
                    else  //correct, add a recond
                    {
                        int ID =Convert.ToInt32(textBox_number.Text) ;
                        string name = "'" + textBox_ID .Text + "'"; ;
                        string pwd = "'" + textBox_password.Text + "'"; ;
                        string gender = (comboBox.Text .Length==0) ? "NULL" :"'"+ comboBox.Text + "'";
                        string phone = (textBox_phone.Text .Length==0) ? "NULL" : "'" + textBox_phone.Text + "'";
                        string birthday= (textBox_birthday.Text .Length==0) ? "NULL" : "'"+textBox_birthday.Text + "'";
                        string department = (textBox_department.Text .Length==0) ? "NULL" : "'"+textBox_department.Text + "'";
                        string order2 = "insert into user_tb (employee_id,employee_name,employee_pwd,gender,phone,birthday,department) values(" + ID+","+name+","+pwd+","+gender+","+phone+","+birthday+","+department+")";
                        bool result = connecttoMysql.insert(order2);
                        if (result)
                        {
                            this.Close();
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
