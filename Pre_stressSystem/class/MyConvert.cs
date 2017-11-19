using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Pre_stressSystem;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Data;
using System.Globalization;

namespace Pre_stressSystem
{

    class Select2ImageConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "0":
                    return "/Pre_stressSystem;component/picture/Portrait.png";
                case "1":
                    return "/Pre_stressSystem;component/bin/Debug/" + UserInfo.employee_id + ".jpg";
                default:
                    return "/Pre_stressSystem;component/picture/Portrait.png";
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

