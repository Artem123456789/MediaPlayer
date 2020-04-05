using NAudio.Wave;
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
    /// <summary>
    /// The delegate that is used to control the audio launch
    /// </summary>
    /// <param name="audioFile">The audio to launch. Serves as model</param>
    public delegate void StartTimer(AudioFileReader audioFile);

    /// <summary>
    /// The view model responsible for displaying the ratio of the current time audio and total
    /// </summary>
    public class AudioTime : INotifyPropertyChanged
    {
        public StartTimer Start;

        //properties
        public int TotalMinutes
        {
            get { return totalMinutes; }
            set
            {
                totalMinutes = value;
                OnPropertyChanged("TotalMinutes");
            }
        }
        public int TotalSeconds
        {
            get { return totalSeconds; }
            set
            {
                totalSeconds = value;
                OnPropertyChanged("TotalSeconds");
            }
        }
        public int CurrentMinutes
        {
            get { return currentMinutes; }
            set
            {
                currentMinutes = value;
                TimerText = $"{CurrentMinutes}:{CurrentSeconds}/{TotalMinutes}:{TotalSeconds}";
            }
        }
        public int CurrentSeconds
        {
            get { return currentSeconds; }
            set
            {
                currentSeconds = value;
                TimerText = $"{CurrentMinutes}:{CurrentSeconds}/{TotalMinutes}:{TotalSeconds}";
            }
        }
        public string TimerText
        {
            get { return timerText; }
            set
            {
                timerText = value;
                OnPropertyChanged("TimerText");
            }
        }

        //constants
        const int UPDATE_INTERVAL = 1000;
        const int SECONDS_MINUTE = 59;

        //fields
        DispatcherTimer timer;
        int totalMinutes;
        int totalSeconds;
        int currentMinutes;
        int currentSeconds;
        string timerText;

        /// <summary>
        /// Default contstructor. Sets default parameters
        /// </summary>
        public AudioTime()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(UPDATE_INTERVAL);
            timer.Tick += new EventHandler(Tick);
            Start += StartTimerNew;
            TimerText = "0:0/0:0";
        }

        /// <summary>
        /// Starts the timer when new audio is selected
        /// </summary>
        /// <param name="audioFile">New audio where do the parameters come from. Serves as model</param>
        private void StartTimerNew(AudioFileReader audioFile)
        {
            TotalMinutes = audioFile.TotalTime.Minutes;
            TotalSeconds = audioFile.TotalTime.Seconds;
            CurrentMinutes = audioFile.CurrentTime.Minutes;
            CurrentSeconds = audioFile.CurrentTime.Seconds;
            TimerText = $"{CurrentMinutes}:{CurrentSeconds}/{TotalMinutes}:{TotalSeconds}";
            timer.Start();
            Start -= StartTimerNew;
            Start += StartTimerPause;
        }

        /// <summary>
        /// Starts timer after it is paused
        /// </summary>
        /// <param name="audioFile">Parameter required to match the delegate signature</param>
        private void StartTimerPause(AudioFileReader audioFile)
        {
            timer.Start();
        }

        /// <summary>
        /// Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Stop();
            CurrentMinutes = 0;
            CurrentSeconds = 0;
            Start -= StartTimerPause;
            Start += StartTimerNew;
        }

        /// <summary>
        /// Put the timer on pause
        /// </summary>
        public void Pause()
        {
            timer.Stop();
        }

        /// <summary>
        /// Called when the timer interval expires. Updates the timer.
        /// </summary>
        /// <param name="sender">Object that called event</param>
        /// <param name="e">Event arguments</param>
        private void Tick(object sender, EventArgs e)
        {
            if (CurrentSeconds == SECONDS_MINUTE)
            {
                CurrentMinutes++;
                CurrentSeconds = 0;
            }
            else CurrentSeconds++;
            if (CurrentMinutes >= TotalMinutes && CurrentSeconds >= TotalSeconds)
            {
                timer.Stop();
                Start -= StartTimerPause;
                Start += StartTimerNew;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
