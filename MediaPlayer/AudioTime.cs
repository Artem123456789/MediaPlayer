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
    public delegate void StartTimer(AudioFileReader audioFile);

    public class AudioTime : INotifyPropertyChanged
    {
        public StartTimer Start;
        const int UPDATE_INTERVAL = 1000;
        const int SECONDS_MINUTE = 59;
        DispatcherTimer timer;
        int totalMinutes;
        int totalSeconds;
        int currentMinutes;
        int currentSeconds;
        string timerText;

        public AudioTime()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(UPDATE_INTERVAL);
            timer.Tick += new EventHandler(Tick);
            Start += StartTimerNew;
            TimerText = "0:0/0:0";
        }

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

        private void StartTimerPause(AudioFileReader audioFile)
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            CurrentMinutes = 0;
            CurrentSeconds = 0;
            Start -= StartTimerPause;
            Start += StartTimerNew;
        }

        public void Pause()
        {
            timer.Stop();
        }

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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
