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
        private DispatcherTimer timer;
        private int totalMinutes;
        private int totalSeconds;
        private int currentMinutes;
        private int currentSeconds;
        private string timerText;

        public AudioTime()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Tick += new EventHandler(Tick);
            Start += StartTimerNew;
        }

        private void StartTimerNew(AudioFileReader audioFile)
        {
            TotalMinutes = audioFile.TotalTime.Minutes;
            TotalSeconds = audioFile.TotalTime.Seconds;
            CurrentMinutes = 0;
            CurrentSeconds = 0;
            TimerText = $"00:00/{TotalMinutes}:{TotalSeconds}";
            timer.Start();
            Start -= StartTimerNew;
            Start += StartTimerPause;
        }

        private void StartTimerPause(AudioFileReader audioFile)
        {
            CurrentSeconds++;
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
            TotalMinutes = 0;
            TotalSeconds = 0;
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
            if (CurrentSeconds == 59) CurrentMinutes++;
            CurrentSeconds = CurrentSeconds == 59 ? 0 : CurrentSeconds + 1;
            TimerText = $"{CurrentMinutes}:{CurrentSeconds}/{TotalMinutes}:{TotalSeconds}";
            if (TotalMinutes == CurrentMinutes && TotalSeconds == CurrentSeconds)
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
                OnPropertyChanged("CurrentMinutes");
            }
        }

        public int CurrentSeconds
        {
            get { return currentSeconds; }
            set
            {
                currentSeconds = value;
                OnPropertyChanged("CurrentSeconds");
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
