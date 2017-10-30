using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;
using System.Net;
using System.Diagnostics;


namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for sensorPage.xaml
    /// </summary>
    public partial class sensorPage : Page
    {
        private ObservableCollection<SensorInfo> sensorList = null;
        List<Dictionary<string, object>> ls = null;
        int func = 0;
        int lastQuery = 0;
        public sensorPage()
        {
            InitializeComponent();
        }

        private void btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Border).Opacity = 1;
            
        }
        private void btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Border).Opacity = 0.8;
        }

        private void bd_query_MouseUp(object sender, MouseButtonEventArgs e)
        {
            lastQuery = 1;
            string key = this.ComboBox_select_key.Text;
            if (key==null || key.Length == 0)
            {
                MessageBox.Show("请选择查询条件");
            }
            else
            {
                string value = this.txt_select_value.Text;
                string order = "select * from sensor_tb";
                if (value != null && value.Length != 0)
                {
                    string sql_key = "";
                    switch (key)
                    {
                        case "传感器ID":
                            sql_key = "sensor_id";
                            break;
                        case "线路名":
                            sql_key = "railway_name";
                            break;
                        case "传感器状态":
                            sql_key = "sensor_state";
                            break;
                        case "预应力状态":
                            sql_key = "stress_state";
                            break;
                    }
                    order += string.Format(" where {0} like '%{1}%'", sql_key, value);
                    ls = connecttoMysql.query(order);

                    if (this.sensorList == null)
                    {
                        this.sensorList = new ObservableCollection<SensorInfo>();
                    }
                    else
                    {
                        this.sensorList.Clear();
                    }
                    if (ls.Count == 0)
                    {
                        MessageBox.Show("没有相应的传感器");
                    }
                    else
                    {
                        this.count.Text = ls.Count.ToString();
                        foreach (var dic in ls)
                        {
                            var si = new SensorInfo();
                            si.sensor_id = dic["sensor_id"].ToString();
                            si.conver_radio = dic["conver_radio"].ToString();
                            si.railway_name = dic["railway_name"].ToString();
                            si.sensor_location = dic["sensor_location"].ToString();
                            si.sensor_SN = dic["sensor_SN"].ToString();
                            si.sensor_state = dic["sensor_state"].ToString();
                            si.stress_state = dic["stress_state"].ToString();
                            si.stress_init = dic["stress_init"].ToString();
                            si.stress_recent = dic["stress_recent"].ToString();
                            this.sensorList.Add(si);
                        }
                    }
                    sensor_list.ItemTemplate = (DataTemplate)this.FindResource("sensorTemplate");
                    sensor_list.ItemsSource = this.sensorList;
                }
                else
                {
                    MessageBox.Show("请输入查询内容");
                }
               
            }
            
        }

        private void btn_list_Click(object sender, RoutedEventArgs e)
        {
            lastQuery = 2;
            string order = "select * from sensor_tb";
            ls = connecttoMysql.query(order);

            if (this.sensorList == null)
            {
                this.sensorList = new ObservableCollection<SensorInfo>();
            }
            else
            {
                this.sensorList.Clear();
            }
            if (ls.Count == 0)
            {
                MessageBox.Show("数据库无传感器记录");
            }
            else
            {
                this.count.Text = ls.Count.ToString();
                foreach (var dic in ls)
                {
                    var si = new SensorInfo();
                    si.sensor_id = dic["sensor_id"].ToString();
                    si.conver_radio = dic["conver_radio"].ToString();
                    si.railway_name = dic["railway_name"].ToString();
                    si.sensor_location = dic["sensor_location"].ToString();
                    si.sensor_SN = dic["sensor_SN"].ToString();
                    si.sensor_state = dic["sensor_state"].ToString();
                    si.stress_state = dic["stress_state"].ToString();
                    si.stress_init = dic["stress_init"].ToString();
                    si.stress_recent = dic["stress_recent"].ToString();
                    this.sensorList.Add(si);
                }
            }
            sensor_list.ItemTemplate = (DataTemplate)this.FindResource("sensorTemplate");
            sensor_list.ItemsSource = this.sensorList;
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.LightBlue);
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as Grid).Background = new SolidColorBrush(Colors.White);
        }

        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.update.IsEnabled = true;
            this.delete.IsEnabled = true;
            string id = ((((sender as Grid).Children[0] as DockPanel).Children[0]) as Label).Content.ToString();
            foreach (var dic in ls)
            {
                if (id == dic["sensor_id"].ToString())
                {
                    this.txt_sensor_id.Text = id;
                    this.txt_conver_radio.Text = dic["conver_radio"].ToString();
                    this.txt_railway_name.Text = dic["railway_name"].ToString();
                    this.txt_sensor_location.Text= dic["sensor_location"].ToString();
                    this.txt_SN.Text = dic["sensor_SN"].ToString();
                    string sensor_status = this.comboBox_sensor_status.Text =  dic["sensor_state"].ToString();
                    //if (sensor_status != "正常")
                    //{
                    //    comboBox_sensor_status.Foreground = new SolidColorBrush(Colors.Red);
                    //}
                    //else {
                    //    comboBox_sensor_status.Foreground = new SolidColorBrush(Colors.Black);
                    //}
                    string stress_status = this.comboBox_stress_status.Text = dic["stress_state"].ToString();
                    //if (stress_status != "正常")
                    //{
                    //    comboBox_stress_status.Foreground = new SolidColorBrush(Colors.Red);
                    //}
                    //else
                    //{
                    //    comboBox_stress_status.Foreground = new SolidColorBrush(Colors.Black);
                    //}
                    this.txt_stress_init.Text = dic["stress_init"].ToString();
                    this.txt_stress_recent.Text = dic["stress_recent"].ToString();
                    break;
                }
            }
        }

        private void bd_clear_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //clear left
            this.ComboBox_select_key.Text = null;
            this.txt_select_value.Text = null;
            this.sensor_list.ItemsSource = null;
            this.count.Text = null;
            this.update.IsEnabled = false;
            this.delete.IsEnabled = false;
            //clear rigth
            this.txt_conver_radio.Text = null;
            this.txt_railway_name.Text = null;
            this.txt_sensor_id.Text = null;
            this.txt_sensor_location.Text = null;
            this.txt_SN.Text = null;
            this.txt_stress_init.Text = null;
            this.txt_stress_recent.Text = null;
            this.comboBox_sensor_status.Text = null;
            this.comboBox_stress_status.Text = null;
        }

        private void add_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //clear text
            this.txt_conver_radio.Text = null;
            this.txt_railway_name.Text = null;
            this.txt_sensor_id.Text = null;
            this.txt_SN.Text = null;
            this.txt_sensor_location.Text = null;
            this.txt_stress_init.Text = null;
            this.txt_stress_recent.Text = null;
            this.comboBox_sensor_status.Text = null;
            this.comboBox_stress_status.Text = null;
        

            //editable
            this.txt_conver_radio.IsReadOnly = false;
            this.txt_railway_name.IsReadOnly = false;
            this.txt_sensor_id.IsReadOnly = false;
            this.txt_SN.IsReadOnly = false;
            this.txt_sensor_location.IsReadOnly = false;
            this.txt_stress_init.IsReadOnly = false;
            this.txt_stress_recent.IsReadOnly = false;
            this.comboBox_sensor_status.IsEnabled = true;
            this.comboBox_stress_status.IsEnabled = true;

            //show or hide button
            this.add.Visibility = Visibility.Collapsed;
            this.update.Visibility = Visibility.Collapsed;
            this.delete.Visibility = Visibility.Collapsed;
            this.save.Visibility = Visibility.Visible;
            this.cancel.Visibility = Visibility.Visible;

            //flag=1
            func = 1;
        }

        private void update_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //editable
            this.txt_conver_radio.IsReadOnly = false;
            this.txt_railway_name.IsReadOnly = false;
            this.txt_sensor_id.IsReadOnly = false;
            this.txt_SN.IsReadOnly = false;
            this.txt_sensor_location.IsReadOnly = false;
            this.txt_stress_init.IsReadOnly = false;
            this.txt_stress_recent.IsReadOnly = false;
            this.comboBox_sensor_status.IsEnabled = true;
            this.comboBox_stress_status.IsEnabled = true;

            //show or hide button
            this.add.Visibility = Visibility.Collapsed;
            this.update.Visibility = Visibility.Collapsed;
            this.delete.Visibility = Visibility.Collapsed;
            this.save.Visibility = Visibility.Visible;
            this.cancel.Visibility = Visibility.Visible;

            //flag=2
            func = 2;
        }
      
        private void delete_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string ID = this.txt_sensor_id.Text;
            if (MessageBox.Show("确定要删除ID为" + ID + "的传感器吗?所有和此ID相关的数据都会删除", 
                "提醒", 
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string order_delete = "delete from sensor_tb where sensor_id='"+ID+"'";
                if (connecttoMysql.delete(order_delete))
                {
                    this.txt_conver_radio.Text = null;
                    this.txt_railway_name.Text = null;
                    this.txt_sensor_id.Text = null;
                    this.txt_SN.Text = null;
                    this.txt_sensor_location.Text = null;
                    this.txt_stress_init.Text = null;
                    this.txt_stress_recent.Text = null;
                    this.comboBox_sensor_status.Text = null;
                    this.comboBox_stress_status.Text = null;

                    if (lastQuery == 1)
                    {
                        //refresh list
                        bd_query_MouseUp(null, null);



                    }
                    else if (lastQuery == 2)
                    {
                        btn_list_Click(null, null);

                    }
                }
            }
        }


        private void save_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string ID = null;
            if (this.txt_sensor_id.Text == null || this.txt_sensor_id.Text == "")
            {
                MessageBox.Show("传感器ID不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_railway_name.Text == null || this.txt_railway_name.Text == "")
            {
                MessageBox.Show("线路名称不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_sensor_location.Text == null || this.txt_sensor_location.Text == "")
            {
                MessageBox.Show("传感器位置不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_stress_init == null || this.txt_stress_init.Text == "")
            {
                MessageBox.Show("预应力初始值不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_conver_radio == null)
            {
                MessageBox.Show("转换系数不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                ID = this.txt_sensor_id.Text;
                string railway_name = this.txt_railway_name.Text;
                string sensor_location = this.txt_sensor_location.Text;
                string stress_init = this.txt_stress_init.Text;
                string SN = this.txt_SN.Text;
                string conver_radio = this.txt_conver_radio.Text;
                string stress_recent = this.txt_stress_recent.Text.Length == 0 ? "NULL" : this.txt_stress_recent.Text;
                string sensor_state = this.comboBox_sensor_status.Text.Length == 0 ? "NULL" : "'" + this.comboBox_sensor_status.Text + "'";
                string stress_state = this.comboBox_stress_status.Text.Length == 0 ? "NULL" : "'" + this.comboBox_stress_status.Text + "'";

                if (func == 1)//add
                {
                    string order_checkid = "select * from sensor_tb where sensor_id = '" + ID + "'";
                    List<Dictionary<string, object>> found = connecttoMysql.query(order_checkid);
                    if (found.Count != 0)
                    {
                        MessageBox.Show("此ID的传感器已经存在", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        string order_insert = string.Format("insert into sensor_tb (sensor_id,conver_radio,railway_name,sensor_location,sensor_state,stress_state,stress_init,stress_recent,sensor_SN) values( '{0}',{1},'{2}','{3}',{4},{5},{6},{7},{8})",
                       ID, conver_radio, railway_name, sensor_location, sensor_state, stress_state, stress_init, stress_recent,SN);
                        bool re = connecttoMysql.insert(order_insert);
                        if (re)
                        {
                            MessageBox.Show("成功插入新传感器");
                            btn_cancel();
                        }
                    }
                }
                else if (func == 2) //update
                {
                    string order_update = string.Format("update sensor_tb set conver_radio = {0},railway_name='{1}',sensor_location = '{2}',sensor_state = {3},stress_state={4},stress_init={5},stress_recent={6} ,sensor_SN={7} where sensor_id = '{8}'"
                        , conver_radio, railway_name, sensor_location, sensor_state, stress_state, stress_init, stress_recent,SN,ID);
                    bool re = connecttoMysql.update(order_update);
                    if (re)
                    {
                        MessageBox.Show("更新传感器数据成功");
                        btn_cancel();
                    }

                }
            }

            //refresh
            if (lastQuery == 1)
            {
                //refresh list
                bd_query_MouseUp(null, null);

               
               
            }
            else if (lastQuery == 2)
            {
                btn_list_Click(null, null);

            }

            //refresh info
            //foreach (var dic in ls)
            //{
            //    if (ID == dic["sensor_id"].ToString())
            //    {
            //        this.txt_sensor_id.Text = ID;
            //        this.txt_conver_radio.Text = dic["conver_radio"].ToString();
            //        this.txt_railway_name.Text = dic["railway_name"].ToString();
            //        this.txt_sensor_location.Text = dic["sensor_location"].ToString();
            //        string sensor_status = this.comboBox_sensor_status.Text = dic["sensor_state"].ToString();
            //        string stress_status = this.comboBox_stress_status.Text = dic["stress_state"].ToString();
            //        this.txt_stress_init.Text = dic["stress_init"].ToString();
            //        this.txt_stress_recent.Text = dic["stress_recent"].ToString();
            //        break;
            //    }
            //}

        }

        private void cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btn_cancel();
        }

        // 增加一个传感器
        private void add_sensor()
        {
            if (this.txt_sensor_id.Text == null || this.txt_sensor_id.Text == "")
            {
                MessageBox.Show("传感器ID不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_railway_name.Text == null || this.txt_railway_name.Text == "")
            {
                MessageBox.Show("线路名称不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }         
            else if (this.txt_SN.Text == null || this.txt_SN.Text == "")
            {
                MessageBox.Show("传感器编号不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_stress_init == null || this.txt_stress_init.Text == "")
            {
                MessageBox.Show("预应力初始值不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_conver_radio == null)
            {
                MessageBox.Show("转换系数不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (this.txt_sensor_location.Text == null || this.txt_sensor_location.Text == "")
            {
                MessageBox.Show("传感器位置不能为空", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                string ID = this.txt_sensor_id.Text;
                string order_checkid = "select * from sensor_tb where sensor_id = '" + ID + "'";
                List<Dictionary<string, object>> found = connecttoMysql.query(order_checkid);
                if (found.Count != 0)
                {
                    MessageBox.Show("此ID的传感器已经存在", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string railway_name = this.txt_railway_name.Text;
                    string sensor_location = this.txt_sensor_location.Text;
                    string stress_init = this.txt_stress_init.Text;
                    string conver_radio = this.txt_conver_radio.Text;
                    string SN = this.txt_SN.Text;
                    string stress_recent = this.txt_stress_recent.Text.Length==0 ? "NULL" : this.txt_stress_recent.Text;
                    string sensor_state = this.comboBox_sensor_status.Text.Length==0 ? "NULL" : "'" + this.comboBox_sensor_status.Text+ "'";
                    string stress_state = this.comboBox_stress_status.Text.Length==0 ? "NULL" : "'" + this.comboBox_stress_status.Text+ "'" ;

                    string order_insert = string.Format("insert into sensor_tb (sensor_id,conver_radio,railway_name,sensor_location,sensor_state,stress_state,stress_init,stress_recent，sensor_SN) values( '{0}',{1},'{2}','{3}',{4},{5},{6},{7},{8})",
                        ID, conver_radio, railway_name, sensor_location, sensor_state, stress_state, stress_init, stress_recent,SN);
                    bool re = connecttoMysql.insert(order_insert);
                    if (re)
                    {
                        MessageBox.Show("成功插入新传感器");
                    }
                }
            }
        }

        //取消
        private void btn_cancel()
        {
            //read only
            this.txt_conver_radio.IsReadOnly = true;
            this.txt_railway_name.IsReadOnly = true;
            this.txt_sensor_id.IsReadOnly = true;
            this.txt_SN.IsReadOnly = true;
            this.txt_sensor_location.IsReadOnly = true;
            this.txt_stress_init.IsReadOnly = true;
            this.txt_stress_recent.IsReadOnly = true;
            this.comboBox_sensor_status.IsEnabled = false;
            this.comboBox_stress_status.IsEnabled = false;

            //show or hide button
            this.add.Visibility = Visibility.Visible;
            this.update.Visibility = Visibility.Visible;
            this.delete.Visibility = Visibility.Visible;
            this.save.Visibility = Visibility.Collapsed;
            this.cancel.Visibility = Visibility.Collapsed;

            //
            this.txt_conver_radio.Text = null;
            this.txt_railway_name.Text = null;
            this.txt_sensor_id.Text = null;
            this.txt_SN.Text = null;
            this.txt_sensor_location.Text = null;
            this.txt_stress_init.Text = null;
            this.txt_stress_recent.Text = null;
            this.comboBox_sensor_status.Text = null;
            this.comboBox_stress_status.Text = null;

        }

        private void baidu_MouseUp(object sender, MouseButtonEventArgs e)
        {
            //百度坐标拾取api网址
           // Uri uri = new Uri("http://api.map.baidu.com/lbsapi/getpoint/index.html");
            //Uri uri = new Uri("https://www.baidu.com/");
           // myWeb.Navigate(uri);
            // WebPage mypage = new WebPage("https://www.baidu.com/");
            // frame.Content = mypage;
            Process ie = new Process();
            ie.StartInfo.FileName = "IEXPLORE.EXE";
            ie.StartInfo.Arguments = "http://api.map.baidu.com/lbsapi/getpoint/index.html";
            ie.Start();


        }
    }
   
}
