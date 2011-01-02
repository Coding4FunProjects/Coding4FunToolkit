using System;

using Microsoft.Phone.Controls;

namespace Coding4Fun.Phone.TestApplication.Samples
{
    public partial class Timespan : PhoneApplicationPage
    {
        public Timespan()
        {
            InitializeComponent();
            DataContext = this;
        }


        public TimeSpan TimeSpan5Min { get { return TimeSpan.FromMinutes(5); } }
        public TimeSpan TimeSpan10Min { get { return TimeSpan.FromMinutes(10); } }
        public TimeSpan TimeSpan30Min { get { return TimeSpan.FromMinutes(30); } }
        public TimeSpan TimeSpan2Hour { get { return TimeSpan.FromHours(2); } }
    }
}