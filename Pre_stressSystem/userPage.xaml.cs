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
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Threading;

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
                    image.Source = bitmap;
                   // bitmap.Freeze();
                }

            }
        }
       
        private void info_edit_Click(object sender, RoutedEventArgs e)
        {
            this.info_edit.Visibility = Visibility.Collapsed;
            this.info_save.Visibility = Visibility.Visible;
            this.info_cancel.Visibility = Visibility.Visible;
            this.Grid_info.Opacity = 1;
            this.uploadpic.Visibility = Visibility.Visible;
            this.Grid_info.IsHitTestVisible = true;           
        }
        public static string img="0";
        Dictionary<string, object> userInfo = null;
        private void getUserInfo()
        {
            userInfo = connecttoMysql.getUserInfo();
        }

        private void showInfo()
        {
            this.textBox_Number.Text = userInfo["number"].ToString().PadLeft(4,'0');
            this.textBox_name.Text = userInfo["name"].ToString();
            this.textBox_email.Text = userInfo["Email"]==null?null:userInfo["Email"].ToString();
            this.comboBox.Text = userInfo["gender"]==null?null:userInfo["gender"].ToString();
            this.textBox_phone.Text = userInfo["phone"]==null?null:userInfo["phone"].ToString();
            this.textBox_birthday.Text = userInfo["birthday"]==null?null:userInfo["birthday"].ToString();
            this.textBox_department.Text = userInfo["department"]==null?null:userInfo["department"].ToString();
            this.textBox_lever.Text = userInfo["lever"]==null?null:userInfo["lever"].ToString();
            this.textBox_address.Text = userInfo["address"]==null?null:userInfo["address"].ToString();
        }

        private void info_save_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Number.Text == "" || textBox_name.Text == "" )
            {
                MessageBox.Show("请把必填信息填写完整", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
                if (textBox_email.Text!="" && !r.IsMatch(textBox_email.Text))
                {
                    MessageBox.Show("邮箱输入格式不正确", "", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                }
                else
                {
                    MySqlConnection mysql = connecttoMysql.getMySqlCon();

                        int number = Convert.ToInt32(textBox_Number.Text);
                        string name = "'" + textBox_name.Text + "'";
                        string email = (textBox_email.Text.Length == 0) ? "NULL":"'" + textBox_email.Text + "'";
                        string gender = (comboBox.Text.Length == 0) ? "NULL" : "'" + comboBox.Text + "'";
                        string phone = (textBox_phone.Text.Length == 0) ? "NULL" : "'" + textBox_phone.Text + "'";
                        string birthday = (textBox_birthday.Text.Length == 0) ? "NULL" : "'" + textBox_birthday.Text + "'";
                        string department = (textBox_department.Text.Length == 0) ? "NULL" : "'" + textBox_department.Text + "'";
                        string address = (textBox_address.Text.Length == 0) ? "NULL" : "'" + textBox_address.Text + "'";
                        string lever = (textBox_lever.Text.Length == 0) ? "NULL" : "'" + textBox_lever.Text + "'";
                        string order2 = "update user_tb  set employee_id=" + number + "," 
                        + "employee_name=" +name + ","
                        + "gender="  + gender + "," 
                        +"phone="+ phone + "," 
                        +"birthday="+ birthday + "," 
                        +"department="+ department +"," 
                        + "Email=" + email + ","
                        + "address=" + address + "," 
                        +"lever="+lever 
                        + " where employee_id="+ GlobalVariable.userNumber;
                        MySqlCommand SqlCommandUpdate = new MySqlCommand(order2, mysql);
                        try
                        {
                            SqlCommandUpdate.ExecuteNonQuery();
                            getUserInfo();
                            showInfo();
                            this.info_edit.Visibility = Visibility.Visible;
                            this.info_save.Visibility = Visibility.Collapsed;
                            this.info_cancel.Visibility = Visibility.Collapsed;
                            this.Grid_info.Opacity = 0.65;
                            this.uploadpic.Visibility = Visibility.Collapsed;
                            this.Grid_info.IsHitTestVisible = false;
                            mysql.Close();
                        
                    }
                        catch (Exception ex)
                        {
                            mysql.Close();
                            String message = ex.Message;
                            MessageBox.Show("插入数据失败了！" + message);
                        }
                    
                }
            }
        }

        private void info_cancel_Click(object sender, RoutedEventArgs e)
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            getUserInfo();
            showInfo();
            this.info_edit.Visibility = Visibility.Visible;
            this.info_save.Visibility = Visibility.Collapsed;
            this.info_cancel.Visibility = Visibility.Collapsed;
            this.Grid_info.Opacity = 0.65;
            this.uploadpic.Visibility = Visibility.Collapsed;
            this.Grid_info.IsHitTestVisible = false;
        }
        private void pwd_Click(object sender, RoutedEventArgs e)
        {
            this.pwd.Visibility = Visibility.Collapsed;
            this.pwd_save.Visibility = Visibility.Visible;
            this.pwd_cancel.Visibility = Visibility.Visible;
            this.grid_changepwd.Opacity = 1;
            this.grid_changepwd.IsHitTestVisible = true;
        }

        private void pwd_save_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox_old.Password != userInfo["pwd"].ToString())
            {
                MessageBox.Show("旧密码输入错误", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (passwordBox_new.Password != passwordBox_new2.Password)
            {
                MessageBox.Show("两次密码输入不一致", "", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else {
                MySqlConnection mysql = connecttoMysql.getMySqlCon();
                string pwd = "'" + passwordBox_new.Password + "'";
                string order3 = "update user_tb  set employee_pwd=" + pwd + " where employee_Number=" + GlobalVariable.userNumber;
                MySqlCommand SqlCommandSetpwd = new MySqlCommand(order3, mysql);
                try
                {
                    SqlCommandSetpwd.ExecuteNonQuery();
                    this.pwd.Visibility = Visibility.Visible;
                    this.pwd_save.Visibility = Visibility.Collapsed;
                    this.pwd_cancel.Visibility = Visibility.Collapsed;
                    this.Grid_info.Opacity = 0.65;
                    this.Grid_info.IsHitTestVisible = false;
                    mysql.Close();

                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void pwd_cancel_Click(object sender, RoutedEventArgs e)
        {
            this.pwd.Visibility = Visibility.Visible;
            this.pwd_save.Visibility = Visibility.Collapsed;
            this.pwd_cancel.Visibility = Visibility.Collapsed;
            this.Grid_info.Opacity = 0.65;
            this.Grid_info.IsHitTestVisible = false;
        }


        DispatcherTimer t = new DispatcherTimer();
        private void uploadpic_Click(object sender, RoutedEventArgs e)
        {

            var openFileDialog = new Microsoft.Win32.OpenFileDialog();
            openFileDialog.Title = "选择文件";
            openFileDialog.Filter = "jpg|*.jpg|jpeg|*.jpeg|png|*.png";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "jpg";
            bool result =  openFileDialog.ShowDialog().Value;
            if (!result)
            {
                return;
            }
            string fileName = openFileDialog.FileName;
            Window pew = new piceditWindow(fileName);
            pew.ShowDialog();
            t.Interval =TimeSpan.FromMilliseconds(500);         //设置定时器，定时刷新结果
 
            t.Tick += new EventHandler(refresh);

            t.Start(); 


        }
       // PicturePath Portraint = new PicturePath();
        string savePath = Environment.CurrentDirectory + "\\" + GlobalVariable.userNumber.ToString() + ".jpg";
        string savePath2 = Environment.CurrentDirectory + "\\" + GlobalVariable.userNumber.ToString() +"_temp"+".jpg";

        public void refresh(object source, EventArgs e)
        {
            if (File.Exists(savePath2))
            {
                if (File.Exists(savePath))
                {                  
                    File.Delete(savePath);                      //删除原文件
                    FileInfo fi = new FileInfo(savePath2);      
                    fi.MoveTo(savePath);                         //将temp文件改名


                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    using (Stream ms = new MemoryStream(File.ReadAllBytes(savePath)))
                    {
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        image.Source = bitmap;
                        // bitmap.Freeze();
                    }

                }

            }
            else if (File.Exists(savePath))
            {
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                using (Stream ms = new MemoryStream(File.ReadAllBytes(savePath)))
                {
                    bitmap.StreamSource = ms;
                    bitmap.EndInit();
                    image.Source = bitmap;
                    // bitmap.Freeze();
                }
            }
            if (GlobalVariable.editPictureDone.Equals("1") || GlobalVariable.editPictureDone.Equals("2"))
            {
                t.Stop();
                GlobalVariable.editPictureDone = "0";
            }
           
            
        }
       
       
    }
}
