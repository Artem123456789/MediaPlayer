using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using NAudio.Wave;

namespace MediaPlayer.view_models
{
    /// <summary>
    /// The ViewModel that is responsible for displaying the audio progress
    /// </summary>
    class AudioProgress :INotifyPropertyChanged
    {
        //public field
        public StartTimer Start;

        //properties
        public double IndicatorCoord
        {
            get { return indicatorCoord; }
            set
            {
                indicatorCoord = value;
                OnProperyChanged("IndicatorCoord");
            }
        }

        //constants
        const int UPDATE_TIME = 1000;

        //private fields
        DispatcherTimer timer;
        double indicatorCoord;
        int totalSeconds;

        /// <summary>
        /// Default contstructor. Sets default parameters
        /// </summary>
        public AudioProgress()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(UPDATE_TIME);
            timer.Tick += new EventHandler(MoveIndicator);
            Start += StartProgressNew;
            IndicatorCoord = 0;
        }

        /// <summary>
        /// Starts the timer when new audio is selected
        /// </summary>
        /// <param name="audioFile">New audio where do the parameters come from. Serves as model</param>
        public void StartProgressNew(AudioFileReader audio)
        {
            this.totalSeconds = (int)audio.TotalTime.TotalSeconds;
            IndicatorCoord = audio.CurrentTime.TotalSeconds;
            timer.Start();
            Start -= StartProgressNew;
            Start += StartProgressPause;
        }

        /// <summary>
        /// Starts timer after it is paused
        /// </summary>
        /// <param name="audioFile">Parameter required to match the delegate signature</param>
        public void StartProgressPause(AudioFileReader audio)
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            Pause();
            IndicatorCoord = 0;
            Start -= StartProgressPause;
            Start += StartProgressNew;
        }

        /// <summary>
        /// Put the timer on pause
        /// </summary>
        public void Pause()
        {
            timer?.Stop();
        }

        /// <summary>
        /// Called when the timer interval expires. moves the audio progress line
        /// </summary>
        /// <param name="sender">Object that called event</param>
        /// <param name="e">Event arguments</param>
        private void MoveIndicator(object sender, EventArgs e)
        {
            IndicatorCoord++;
            if (IndicatorCoord == totalSeconds)
            {
                timer.Stop();
                Start -= StartProgressPause;
                Start += StartProgressNew;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnProperyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
