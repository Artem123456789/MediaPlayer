using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using NAudio.Wave;

namespace MediaPlayer
{

    public delegate void StartRecordProgress(AudioFileReader audio);

    class RecordProgress:INotifyPropertyChanged
    {
        const int UPDATE_TIME = 1000;
        private DispatcherTimer time;
        public StartRecordProgress Start;
        private double indicatorCoord;
        private int totalSeconds;
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
            IndicatorCoord = 0;
        }

        public void StartProgressNew(AudioFileReader audio)
        {
            this.totalSeconds = (int)audio.TotalTime.TotalSeconds;
            IndicatorCoord = audio.CurrentTime.TotalSeconds;
            time.Start();
            Start -= StartProgressNew;
            Start += StartProgressPause;
        }

        public void StartProgressPause(AudioFileReader audio)
        {
            time.Start();
        }

        private void MoveIndicator(object sender, EventArgs e)
        {
            IndicatorCoord++;
            if (IndicatorCoord == totalSeconds)
            {
                time.Stop();
                Start -= StartProgressPause;
                Start += StartProgressNew;
            }
        }

        public void Stop()
        {
            Pause();
            IndicatorCoord = 0;
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
