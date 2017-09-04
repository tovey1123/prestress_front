using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;

namespace Pre_stressSystem
{
    class InitFile
{
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);


        private string Path;
        public InitFile(string path) {
            this.Path = path;
        }

        //写INI文件
        public void WriteValue(string section, string key, string value) {
            WritePrivateProfileString(section, key, value, this.Path);
        }
        public string ReadValue(String section,string key)
        {
            //FileStream file = new FileStream(Path+"\\Info.ini", FileMode.OpenOrCreate);
            StringBuilder temp = new StringBuilder(255);
            int i = GetPrivateProfileString(section, key, "", temp, 255, this.Path);
            return temp.ToString();

        }


    }
}
