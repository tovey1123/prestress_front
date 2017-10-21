using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Pre_stressSystem
{
    public class SensorInfo : INotifyPropertyChanged
    {
        public string sensor_id { get; set; }
        public string conver_radio { get; set; }
        public string railway_name { get; set; }
        public string sensor_location { get; set; }
        public string sensor_state { get; set; }
        public string stress_state { get; set; }
        public string stress_init { get; set; }
        public string stress_recent { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
