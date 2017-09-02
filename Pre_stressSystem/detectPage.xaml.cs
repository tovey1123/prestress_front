using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Threading;
using System.Collections;

namespace Pre_stressSystem
{
    /// <summary>
    /// Interaction logic for detectPage.xaml
    /// </summary>
    public partial class detectPage : Page
    {
        private string[] ports;
        private bool spIsOpen = false;
        private SerialPort sp = GlobalVariable.sp;
        Queue recQueue = new Queue();
        delegate void HandleInterfaceUpdateDelagate(string text);//委托；此为重点
        HandleInterfaceUpdateDelagate interfaceUpdateHandle;
        public detectPage()
        {          
            InitializeComponent();
           // init();
        }
        //private void init() {
        //    sp.DataReceived += new SerialDataReceivedEventHandler(ComReceive);
        //    Thread _ComRec = new Thread(new ThreadStart(ComRec)); //查询串口接收数据线程声明  
        //    _ComRec.Start();//启动线程 
        //}

        private void autoSelectCOM_Click(object sender, RoutedEventArgs e)
        {
            ports = SerialPort.GetPortNames();
            if (ports.Length > 0)
            {
                if (!COMList.Items.Contains(ports[ports.Length-1]))
                {
                    COMList.Items.Add(ports[ports.Length - 1]);
                }

                COMList.SelectedValue = ports[ports.Length - 1];
            }
            else {
                MessageBox.Show("无可用串口");
            }
        }
        private void setPortClosed()
        {
            spIsOpen = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (!spIsOpen)
            {
                try
                {
                    sp.PortName = COMList.Text;
                    sp.BaudRate = int.Parse(BaudRateList.Text);
                    sp.Parity = Parity.None;
                    sp.StopBits = StopBits.One;
                    sp.ReadBufferSize = 1024;
                    sp.DataBits = 8;
                    sp.Open();
                    spIsOpen = true;
                    sp.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(data_received);
                }
                catch (Exception exception)
                {
                    setPortClosed();
                    MessageBox.Show("Error happened when open port:" + exception.Message);
                    return;
                }
                btn_open_close.Content = "关闭串口";
                BaudRateList.IsEnabled = false;
            }else
            {
                try
                {
                    sp.Close();
                }
                catch
                {
                    if (sp.IsOpen == true)
                    {
                        setPortClosed();
                    }
                    else
                    {
                        MessageBox.Show("串口无法关闭");
                        return;
                    }
                }
                btn_open_close.Content = "打开串口";
                setPortClosed();
                BaudRateList.IsEnabled = true;
            }

        }
        private void data_received(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort1 = sender as SerialPort;
            try
            {
                int n = serialPort1.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
                //received_count += n;//增加接收计数  
                serialPort1.Read(buf, 0, n);//读取缓冲数据  
                                            //因为要访问ui资源，所以需要使用invoke方式同步ui
                String result = null;
                String temp = null;
                for (int i = 0; i < buf.Length; i++)
                {
                    temp = Convert.ToString(buf[i], 2).PadLeft(8, '0');
                    result += temp;
                }
                interfaceUpdateHandle = new HandleInterfaceUpdateDelagate(UpdateTextBox);//实例化委托对象
                Dispatcher.Invoke(interfaceUpdateHandle, result);              
            }
            catch (Exception e2)
            {
                MessageBox.Show(e2.Message);
                //处理超时错误
            }
        }

        private void ReceiveData(SerialPort serialPort)
        {
            //同步阻塞接收数据线程
            Thread threadReceive = new Thread(new ParameterizedThreadStart(SynReceiveData));
            threadReceive.Start(serialPort);
           

            //也可用异步接收数据线程
            //Thread threadReceiveSub = new Thread(new ParameterizedThreadStart(AsyReceiveData));
            //threadReceiveSub.Start(serialPort);
        }

        //发送二进制数据
        //private void SendBytesData(SerialPort serialPort)
        //{
        //    byte[] bytesSend = System.Text.Encoding.Default.GetBytes(txtSend.Text);
        //    serialPort.Write(bytesSend, 0, bytesSend.Length);

        //}

        //同步阻塞读取
        private void SynReceiveData(object serialPortobj)
        {
           
            SerialPort serialPort = (SerialPort)serialPortobj;
            System.Threading.Thread.Sleep(0);
            serialPort.ReadTimeout = 6000;
            try
            {
                int n = serialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
                //received_count += n;//增加接收计数  
                serialPort.Read(buf, 0, n);//读取缓冲数据  
                //因为要访问ui资源，所以需要使用invoke方式同步ui
                interfaceUpdateHandle = new HandleInterfaceUpdateDelagate(UpdateTextBox);//实例化委托对象
             //   MessageBox.Show("4:"+ sp.IsOpen.ToString());
                Dispatcher.Invoke(interfaceUpdateHandle, new string[] { Encoding.ASCII.GetString(buf) });
               // MessageBox.Show("5:" + sp.IsOpen.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //处理超时错误
            }

            serialPort.Close();

        }

        private void UpdateTextBox(string text)
        {
            txtReceive.Text = text;
        }

        //异步读取
        private void AsyReceiveData(object serialPortobj)
        {
            SerialPort serialPort = (SerialPort)serialPortobj;
            System.Threading.Thread.Sleep(500);
            try
            {
                int n = serialPort.BytesToRead;//先记录下来，避免某种原因，人为的原因，操作几次之间时间长，缓存不一致  
                byte[] buf = new byte[n];//声明一个临时数组存储当前来的串口数据  
                serialPort.Read(buf, 0, n);//读取缓冲数据  
                //因为要访问ui资源，所以需要使用invoke方式同步ui。
                String result = null;
                for (int i = 0; i < buf.Length; i++)
                {
                    result += Convert.ToString(buf[1], 16);
                }
                interfaceUpdateHandle = new HandleInterfaceUpdateDelagate(UpdateTextBox);//实例化委托对象
                Dispatcher.Invoke(interfaceUpdateHandle,result);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                //处理错误
            }
            serialPort.Close();
        }

        private void btn_clear_Click(object sender, RoutedEventArgs e)
        {
            txtReceive.Text = "";
        }

        private void btn_analysis_Click(object sender, RoutedEventArgs e)
        {
           Boolean validity= checkValidity();
        }
        private Boolean checkValidity() {
            String rcv = txtReceive.Text;
            if (rcv == null || rcv == "")
            {
                return false;
            }
            if(rcv.Length != 112)
            {
                return false;
            }
            String Sensor_ID = rcv.Substring(9, 48);
            String Sensor_ID = rcv.Substring(9, 48);


            return true;
        }


        //-------------------------------------------------------------------------------------------------------------------------------
        //public bool ReceiveCompleted { get; set; }

        //void ComRec()//接收线程，窗口初始化中就开始启动运行  
        //{
        //    while (true)//一直查询串口接收线程中是否有新数据  
        //    {
        //        if (recQueue.Count> 0)//当串口接收线程中有新的数据时候，队列中有新进的成员recQueue.Count > 0  
        //        {
        //            string recData;//接收数据转码后缓存  
        //            byte[] recBuffer = (byte[])recQueue.Dequeue();//出列Dequeue（全局）  
        //            recData = System.Text.Encoding.Default.GetString(recBuffer);//转码  
        //            UIAction(() =>
        //            {

        //                    StringBuilder recBuffer16 = new StringBuilder();//定义16进制接收缓存  
        //                    for (int i = 0; i < recBuffer.Length; i++)
        //                    {
        //                        recBuffer16.AppendFormat("{0:X2}" + " ", recBuffer[i]);//X2表示十六进制格式（大写），域宽2位，不足的左边填0。  
        //                    }
        //                txtReceive.Text += recBuffer16.ToString();//加显到接收区  

        //            });
        //        }
        //        else
        //            Thread.Sleep(100);//如果不延时，一直查询，将占用CPU过高  
        //    }

        //}

        //private void ComReceive(object sender, SerialDataReceivedEventArgs e)//接收数据 数据在接收中断里面处理  
        //{
        //   // if (WaitClose) return;//如果正在关闭串口，则直接返回  
        //    if (recStatus)//如果已经开启接收  
        //    {
        //        try
        //        {
        //            Listening = true;////设置标记，说明我已经开始处理数据，一会儿要使用系统UI的。  
        //            Thread.Sleep(10);//发送和接收均为文本时，接收中为加入判断是否为文字的算法，发送你（C4E3），接收可能识别为C4,E3，可用在这里加延时解决  
        //            string recData;//接收数据转码后缓存  
        //            byte[] recBuffer = new byte[sp.BytesToRead];//接收数据缓存  
        //            sp.Read(recBuffer, 0, recBuffer.Length);//读取数据  
        //            recData = System.Text.Encoding.Default.GetString(recBuffer);//转码  
        //            this.txtReceive.Dispatcher.Invoke(//WPF为单线程，此接收中断线程不能对WPF进行操作，用如下方法才可操作  
        //            new Action(
        //                 delegate
        //                 {
        //                    // recCount.Text = (Convert.ToInt32(recCount.Text) + recBuffer.Length).ToString();//接收数据字节数  

        //                         StringBuilder recBuffer16 = new StringBuilder();//定义16进制接收缓存  
        //                         for (int i = 0; i < recBuffer.Length; i++)
        //                         {
        //                             recBuffer16.AppendFormat("{0:X2}" + " ", recBuffer[i]);//X2表示十六进制格式（大写），域宽2位，不足的左边填0。  
        //                         }
        //                     txtReceive.Text += recBuffer16.ToString();//加显到接收区  

        //                 }
        //            )
        //            );

        //        }
        //        finally
        //        {
        //            Listening = false;//UI使用结束，用于关闭串口时判断，避免自动发送时拔掉串口，陷入死循环  
        //        }

        //    }
        //    else//暂停接收  
        //    {
        //        sp.DiscardInBuffer();//清接收缓存  
        //    }


        //}
        //void UIAction(Action action)//在主线程外激活线程方法  
        //{
        //    System.Threading.SynchronizationContext.SetSynchronizationContext(new System.Windows.Threading.DispatcherSynchronizationContext(App.Current.Dispatcher));
        //    System.Threading.SynchronizationContext.Current.Post(_ => action(), null);
        //}
    }
}
