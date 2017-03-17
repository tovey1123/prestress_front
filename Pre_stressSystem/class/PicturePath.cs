using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Pre_stressSystem
{
    class PicturePath : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private string _Userhead;
        public string Userhead
        {
            get { return _Userhead; }
            set
            {
                _Userhead = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Userhead"));
                }
            }
        }
    }
}
