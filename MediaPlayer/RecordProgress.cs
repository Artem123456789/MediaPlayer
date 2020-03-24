using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace MediaPlayer
{

    public delegate void StartRecordProgress(int totalSeconds, int maxIndicatorLength);

    class RecordProgress:INotifyPropertyChanged
    {
        const int UPDATE_TIME = 1000;
        const int MILLISECONDS_SECOND = 1000;
        private DispatcherTimer time;
        public StartRecordProgress Start;
        private double indicatorCoord;
        private int totalSeconds;
        private int maxIndicatorLength;
        public double IndicatorCoord
        {
            get { return indicatorCoord; }
            set
            {
                indicatorCoord = value;
                OnProperyChanged("IndicatorCoord");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public RecordProgress()
        {
            time = new DispatcherTimer();
            time.Interval = TimeSpan.FromMilliseconds(UPDATE_TIME);
            time.Tick += new EventHandler(MoveIndicator);
            Start += StartProgressNew;
            IndicatorCoord = 15;
        }

        public void StartProgressNew(int totalSeconds, int maxIndicatorLength)
        {
            this.totalSeconds = totalSeconds;
            this.maxIndicatorLength = maxIndicatorLength;
            IndicatorCoord = 15;
            time.Start();
            Start -= StartProgressNew;
            Start += StartProgressPause;
        }

        public void StartProgressPause(int totalSeconds, int maxIndicatorLength)
        {
            time.Start();
        }

        private void MoveIndicator(object sender, EventArgs e)
        {
            IndicatorCoord += (double)maxIndicatorLength / (double)totalSeconds;
            if (IndicatorCoord >= maxIndicatorLength + 15)
            {
                time.Stop();
                Start -= StartProgressPause;
                Start += StartProgressNew;
            }
        }

        public void Stop()
        {
            Pause();
            IndicatorCoord = 15;
            Start -= StartProgressPause;
            Start += StartProgressNew;
        }

        public void Pause()
        {
            time?.Stop();
        }

        public void OnProperyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
