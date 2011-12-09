using System;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    abstract class TimeSpanDataSource : DataSource<TimeSpan>
    {
        protected int Max;
        protected int Step;

        protected TimeSpanDataSource(int max, int step)
        {
            Max = max;
            Step = step;
        }


        /// <summary>
        /// By default, the datasource is not empty -> we always show it
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return ((Max - 1 == 0) || (Step == 0));
            }
        }

        protected int ComputeRelativeTo(int value, int delta)
        {
            int nextValue;
            int max = Max;// -(_max % _step);

            if (max > 0)
            {                
                nextValue = (max + value + (delta * Step)) % max;
                nextValue += (max <= value) ? max : 0;
            }
            else
            {
                nextValue = value;
            }

            return nextValue;
        }
    }
}
