using System;

namespace Coding4Fun.Phone.Controls.Toolkit
{
    abstract class TsDataSource : DataSource<TimeSpan>
    {
        protected int _max;
        protected int _step;

        protected TsDataSource(int max, int step)
        {
            _max = max;
            _step = step;
        }


        /// <summary>
        /// By default, the datasource is not empty -> we always show it
        /// </summary>
        public override bool IsEmpty
        {
            get
            {
                return ((_max - 1 == 0) || (_step == 0));
            }
        }

        protected int ComputeRelativeTo(int value, int delta)
        {
            int nextValue;
            int max = _max;// -(_max % _step);

            if (max > 0)
            {                
                nextValue = (max + value + (delta * _step)) % max;
                nextValue += (max <= value) ? max : 0;
            }
            else
            {
                nextValue = value;
            }
            return nextValue;
        }

    }

    class HourTSDataSource : TsDataSource
    {
        public HourTSDataSource()
            : base(23, 1)
        {
        }

        public HourTSDataSource(int max, int step)
            : base(max, step)
        {
        }

        protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
        {
            return new TimeSpan(ComputeRelativeTo(relativeDate.Hours, delta), relativeDate.Minutes, relativeDate.Seconds);
        }

    }

    class MinuteTSDataSource : TsDataSource
    {
        public MinuteTSDataSource()
            : base(59, 1)
        {
        }

        public MinuteTSDataSource(int max, int step)
            : base(max, step)
        {
        }

        protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
        {
            return new TimeSpan(relativeDate.Hours, ComputeRelativeTo(relativeDate.Minutes, delta), relativeDate.Seconds);
        }
    }

    class SecondTSDataSource : TsDataSource
    {
        public SecondTSDataSource()
            : base(59, 1)
        {
        }

        public SecondTSDataSource(int max, int step)
            : base(max, step)
        {
        }

        protected override TimeSpan? GetRelativeTo(TimeSpan relativeDate, int delta)
        {
            return new TimeSpan(relativeDate.Hours, relativeDate.Minutes, ComputeRelativeTo(relativeDate.Seconds, delta));
        }
    }
}
