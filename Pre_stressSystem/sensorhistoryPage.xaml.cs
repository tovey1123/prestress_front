using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Visifire.Charts;
using System.Windows.Media.Animation;

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for sensorhistoryPage.xaml
    /// </summary>
    public partial class sensorhistoryPage : Page
    {

        private ObservableCollection<SensorInfo> date_List = null;
        List<Dictionary<string, object>> ls = null;

        int lastbd = 0;
        int newbd;
        public sensorhistoryPage()
        {
            InitializeComponent();
        }

        private void key_DropDownClosed(object sender, EventArgs e)
        {
            //MessageBox.Show(key.Text);
            if (key.Text == "传感器线路")
            {
                this.value.Content = "名称";
                this.from.Content = "起始编号";
                this.to.Content = "终止编号";
                this.date_from.Visibility = Visibility.Collapsed;
                this.date_to.Visibility = Visibility.Collapsed;
                this.number_from.Visibility = Visibility.Visible;
                this.number_to.Visibility = Visibility.Visible;
            }
            else if (key.Text == "传感器ID")
            {
                this.value.Content = "ID值";
                this.from.Content = "起始时间";
                this.to.Content = "终止时间";
                this.date_from.Visibility = Visibility.Visible;
                this.date_to.Visibility = Visibility.Visible;
                this.number_from.Visibility = Visibility.Collapsed;
                this.number_to.Visibility = Visibility.Collapsed;
            }
        }




        #region 按钮的进入退出效果
        private void btn_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

            (sender as Border).Opacity = 1;
        }
        private void btn_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            (sender as Border).Opacity = 0.8;
        }


        #endregion

        private void bd_ok_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string value = txt_value.Text;
           
            if (key.Text == "传感器线路")
            {
                if (value == "")
                {
                    MessageBox.Show("请输入线路名称");
                    return;
                }
                string from = number_from.Text;
                string to = number_to.Text;
                string order_query1 = string.Format("select * from sensor_tb where railway_name = '{0}' and sensor_SN >= {1} and sensor_SN <= {2} order by sensor_SN",
                   value, from, to);
                if (ls != null)
                    ls.Clear();
                ls = ConnecttoMysql.query(order_query1);
                if (ls.Count == 0)
                {
                    MessageBox.Show("没有相应的传感器记录");
                }
                else
                {
                    this.change_chart.Visibility = Visibility.Visible;
                    Chart chart_col= CreateChartColumn(ls, "sensor_SN", "stress_recent");   //默认展示柱状图
                    Grid gr = new Grid();
                    gr.Children.Add(chart_col);
                    this.mychart.Children.Add(gr);
                }
               
            }
            else if (key.Text == "传感器ID")
            {
                if (value == "")
                {
                    MessageBox.Show("请输入要查询的ID值");
                    return;
                }
                this.change_chart.Visibility = Visibility.Collapsed;
                string from = Convert.ToDateTime(date_from.Text).ToString("yyyy-MM-dd");
                string to = Convert.ToDateTime(date_to.Text).ToString("yyyy-MM-dd");
                string order_query1 = string.Format("select * from sensor_data_tb where sensor_id = '{0}' and date >= '{1}' and date <= '{2}' order by date",
                   value, from, to);
                if (ls != null)
                    ls.Clear();
                ls = ConnecttoMysql.query(order_query1);
                if (ls.Count == 0) {
                    MessageBox.Show("没有相应的传感器记录");
                }else
                {
                    CreateChartSpline(ls, "date", "data_value");
                }
                
            }
        }

        private void bd_cancel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            this.txt_value.Text = null;
            this.date_from.Text = null;
            this.date_to.Text = null;

        }


        public void CreateChartSpline(List<Dictionary<string, object>> ls,string x_name ,string y_name)
        {
            Chart chart_spline= new Chart();
            //设置图标的宽度和高度
            chart_spline.Width = 680;
            chart_spline.Height = 400;
            //chart.Margin = new Thickness(100, 5, 10, 5);
            //是否启用打印和保持图片
            chart_spline.ToolBarEnabled = true;

            //设置图标的属性
            chart_spline.ScrollingEnabled = false;//是否启用或禁用滚动
            chart_spline.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            title.Text = "预应力数据折线图";
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart_spline.Titles.Add(title);

            //初始化一个新的Axis
            Axis xaxis = new Axis();
            //设置Axis的属性
            //图表的X轴坐标按什么来分类，如时分秒
            xaxis.IntervalType = IntervalTypes.Auto;
                             
            //图表的X轴坐标间隔如2,3,20等，单位为xAxis.IntervalType设置的时分秒。
             xaxis.Interval = 1;
            //设置X轴的时间显示格式为7-10 11：20     
            xaxis.ValueFormatString = "yyyy/MM/dd";
            xaxis.Title = "日期";
           
           
           
            //给图标添加Axis            
            chart_spline.AxesX.Add(xaxis);

            Axis yAxis = new Axis();
            //设置图标中Y轴的最小值永远为0           
            //yAxis.AxisMinimum = 0;

            yAxis.Title = "预应力";
            //设置图表中Y轴的后缀          
            yAxis.Suffix = "kN";
            chart_spline.AxesY.Add(yAxis);

            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();
            // 设置数据线的格式。               

            dataSeries.RenderAs = RenderAs.Spline;//折线图

            dataSeries.XValueType = ChartValueTypes.DateTime;
            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < ls.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点   
                if (x_name == "date")
                {
                    dataPoint.XValue = ls[i][x_name].ToString();
                }
                else
                {
                    
                    dataPoint.XValue =ls[i][x_name].ToString();
                }
            
                //设置Y轴点                   
                dataPoint.YValue = double.Parse(ls[i][y_name].ToString());
                dataPoint.MarkerSize = 8;
                //dataPoint.Tag = tableName.Split('(')[0];
                //设置数据点颜色                  
                // dataPoint.Color = new SolidColorBrush(Colors.LightGray);                   
                //dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);
                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart_spline.Series.Add(dataSeries);

            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.  
            this.mychart.Children.Clear();
            Grid gr = new Grid();
            gr.Children.Add(chart_spline);
            this.mychart.Children.Add(gr);
        }


 
        public Chart CreateChartPie(List<Dictionary<string, object>> ls, string key)
        {
            #region 预处理数据
            List<string> strListx = null;
            List<int> strListy = null;
            if (key == "stress_state")
            {
                strListx = new List<string>() {"失效","正常","偏小","偏大"};
                int useless = 0;
                int normal = 0;
                int smaller = 0;
                int larger = 0;
                foreach (var dic in ls)
                {
                    switch (dic[key].ToString()) {
                        case "失效":
                            useless++;
                            break;
                        case "正常":
                            normal++;
                            break;
                        case "偏小":
                            smaller++;
                            break;
                        case "偏大":
                            larger++;
                            break;
                    }
                    strListy = new List<int>() { useless, normal, smaller, larger };

                }
            }
            else  if(key =="sensor_state"){
                strListx = new List<string>() { "失效", "正常"};
                int useless = 0;
                int normal = 0;
                foreach (var dic in ls)
                {
                    switch (dic[key].ToString())
                    {
                        case "失效":
                            useless++;
                            break;
                        case "正常":
                            normal++;
                            break;

                    }
                    strListy = new List<int>() { useless, normal};
                }
            }
            #endregion

            //创建一个图标
            Chart chart = new Chart();

            //设置图标的宽度和高度
            chart.Width = 680;
            chart.Height = 400;
            //chart.Margin = new Thickness(100, 5, 10, 5);
            //是否启用打印和保持图片
            chart.ToolBarEnabled = true;

            //设置图标的属性
            chart.ScrollingEnabled = false;//是否启用或禁用滚动
            chart.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            if (key == "stress_state")
            {
                title.Text = "预应力状态扇形图";
            }
            else {
                title.Text = "传感器状态扇形图";
            }
               
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart.Titles.Add(title);

            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();
            // 设置数据线的格式。               

            dataSeries.RenderAs = RenderAs.Pie;//扇形图


            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < strListx.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.AxisXLabel = strListx[i];

                dataPoint.LegendText = "##" + strListx[i];
                //设置Y轴点                   
                dataPoint.YValue = strListy[i];            
                dataSeries.DataPoints.Add(dataPoint);

            }


            // 添加数据线到数据序列。                
            chart.Series.Add(dataSeries);
            return chart;


            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.           
            //Grid gr = new Grid();
            //gr.Children.Add(chart);
            //this.mychart.Children.Add(gr);
        }

        #region 柱状图
        public Chart CreateChartColumn( List<Dictionary<string, object>> ls, string x_name, string y_name)
        {
            //创建一个图标
            Chart chart_col = new Chart();

            //设置图标的宽度和高度
            chart_col.Width = 680;
            chart_col.Height = 400;

            //是否启用打印和保持图片
            chart_col.ToolBarEnabled = true;

            //设置图标的属性
            chart_col.ScrollingEnabled = false;//是否启用或禁用滚动
            chart_col.View3D = true;//3D效果显示

            //创建一个标题的对象
            Title title = new Title();

            //设置标题的名称
            title.Text = "传感器信息柱状图";
            title.Padding = new Thickness(0, 10, 5, 0);

            //向图标添加标题
            chart_col.Titles.Add(title);

            Axis xAxis = new Axis();
            xAxis.Title = "编号";
            chart_col.AxesX.Add(xAxis);

            Axis yAxis = new Axis();

            yAxis.Title = "预应力";
            //设置图表中Y轴的后缀          
            yAxis.Suffix = "kN";
            chart_col.AxesY.Add(yAxis);

            // 创建一个新的数据线。               
            DataSeries dataSeries = new DataSeries();

            // 设置数据线的格式
            dataSeries.RenderAs = RenderAs.StackedColumn;//柱状Stacked


            // 设置数据点              
            DataPoint dataPoint;
            for (int i = 0; i < ls.Count; i++)
            {
                // 创建一个数据点的实例。                   
                dataPoint = new DataPoint();
                // 设置X轴点                    
                dataPoint.XValue = ls[i][x_name].ToString();
                //设置Y轴点                   
                dataPoint.YValue = double.Parse(ls[i][y_name].ToString());
                //添加一个点击事件        

                //添加数据点                   
                dataSeries.DataPoints.Add(dataPoint);
            }

            // 添加数据线到数据序列。                
            chart_col.Series.Add(dataSeries);


            return chart_col;
            //将生产的图表增加到Grid，然后通过Grid添加到上层Grid.      
            //this.mychart.Children.Clear();
            //Grid gr = new Grid();
            //gr.Children.Add(chart_col);
            //this.mychart.Children.Add(gr);
        }
        #endregion

        private void bd_changeChart_MouseUp(object sender, MouseButtonEventArgs e)
        {
            string bd_name = (sender as Border).Name;

            if (bd_name == "bd_stress_Spline")
            {
                newbd = 0;

                //动画

                bg.RenderTransform = new TranslateTransform();
                DoubleAnimation Flow = new DoubleAnimation();
                Flow.From = lastbd * 112;
                Flow.To = newbd * 112;
                Flow.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(0.2));
                Storyboard sd = new Storyboard();
                sd.FillBehavior = FillBehavior.HoldEnd;
                sd.AutoReverse = false;
                Storyboard.SetTarget(Flow, bg);
                Storyboard.SetTargetProperty(Flow, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                sd.Children.Add(Flow);
                sd.Begin();

                lastbd = 0;
                //添加到表格
                Chart chart_col = CreateChartColumn(ls, "sensor_SN", "stress_recent");
                Grid gr = new Grid();
                gr.Children.Add(chart_col);
                this.mychart.Children.Add(gr);

            }
            else if (bd_name == "bd_stress_pie")
            {
                newbd = 1;
                bg.RenderTransform = new TranslateTransform();
                DoubleAnimation Flow = new DoubleAnimation();
                Flow.From = lastbd * 112;
                Flow.To = newbd * 112;
                Flow.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(0.2));
                Storyboard sd = new Storyboard();
                sd.FillBehavior = FillBehavior.HoldEnd;
                sd.AutoReverse = false;
                Storyboard.SetTarget(Flow, bg);
                Storyboard.SetTargetProperty(Flow, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                sd.Children.Add(Flow);
                sd.Begin();
                lastbd = 1;

                Chart chart_pie_stress= CreateChartPie(ls, "stress_state");
                Grid gr = new Grid();
                gr.Children.Add(chart_pie_stress);
                this.mychart.Children.Add(gr);
            }
            else if (bd_name == "bd_sensor_pie")
            {
                newbd = 2;
                bg.RenderTransform = new TranslateTransform();
                DoubleAnimation Flow = new DoubleAnimation();
                Flow.From = lastbd * 112;
                Flow.To = newbd * 112;
                Flow.Duration = new System.Windows.Duration(TimeSpan.FromSeconds(0.2));
                Storyboard sd = new Storyboard();
                sd.FillBehavior = FillBehavior.HoldEnd;
                sd.AutoReverse = false;
                Storyboard.SetTarget(Flow, bg);
                Storyboard.SetTargetProperty(Flow, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
                sd.Children.Add(Flow);
                sd.Begin();
                lastbd = 2;

                Chart chart_pie_sensor=chart_pie_sensor = CreateChartPie(ls,"sensor_state");
                Grid gr = new Grid();
                gr.Children.Add(chart_pie_sensor);
                this.mychart.Children.Add(gr);

            }
        }
    }
}
