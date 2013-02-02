using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Memory : PhoneApplicationPage
    {
        public Memory()
        {
            InitializeComponent();
        }

        List<Byte[]> _memory = new List<byte[]>();

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            _memory.Add(new Byte[1024 * 1024 * 2]);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (_memory.Count > 0)
                _memory.RemoveAt(_memory.Count - 1);
            
            GC.Collect();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}