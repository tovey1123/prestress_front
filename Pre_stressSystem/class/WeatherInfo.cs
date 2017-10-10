using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Pre_stressSystem
{
    public class WeatherInfo : INotifyPropertyChanged
{
        public string week { get; set; }
        public string date { get; set; }
        public string high { get; set; }
        public string low { get; set; }
        public string direction { get; set; }
        public string type { get; set; }
        public string lever { get; set; }
        public string icoPath { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
}
}
