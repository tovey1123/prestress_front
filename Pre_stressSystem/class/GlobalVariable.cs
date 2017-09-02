using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;

namespace Pre_stressSystem
{
    class GlobalVariable
{
    static public String userName;
    static public int? userNumber;   //可空值型int
    static public Window g_mainWindow;
    static public Frame g_Framemain;
    static public string editPictureDone = "0";
    static public SerialPort sp = new SerialPort();
    static public Boolean hasHandle = false;
    static public void clearData()
        {
            userName = "";
            userNumber = null;
            sp.Close();

        }
    }
}
