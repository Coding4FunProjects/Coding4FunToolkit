using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Coding4Fun.Phone.Controls
{
    public class RoundButton : Button
    {
        public RoundButton()
        {
            DefaultStyleKey = typeof(RoundButton);
            DataContext = this;
        }
    }
}
