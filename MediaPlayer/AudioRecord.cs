using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MediaPlayer
{
    class AudioRecord:INotifyPropertyChanged
    {
        const int MAX_PROGRESS_LEN = 545;
        public WaveOutEvent OutputDevice { get; set; }
        public AudioFileReader Audio { get; set; }
        public AudioTime AudioTime { get; set; }
        public RecordProgress RecordProgress { get; set; }
        AudioRecordCommand chooseAudioCommand;
        AudioRecordCommand playPauseMusicCommand;
        AudioRecordCommand restartMusicCommand;
        BitmapImage playPauseImageSource;
        string audioPath;
        string audioName;
        string playButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/play-music.png";
        string pauseButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/stop-music.png";
        public int r = 100;
        double totalAudioSecondsTime;
        public double TotalAudioSecondsTime
        {
            get
            {
                return totalAudioSecondsTime;
            }
            set
            {
                totalAudioSecondsTime = value;
                OnPropertyChanged("TotalAudioSecondsTime");
            }
        }
        TimeSpan newTime;

        public AudioRecord()
        {
            AudioTime = new AudioTime();
            RecordProgress = new RecordProgress();
            TotalAudioSecondsTime = 0;
            ChangePlayPauseImage(true);
        }

        public AudioRecordCommand ChooseAudioCommand
        {
            get
            {
                return chooseAudioCommand ??
                    (chooseAudioCommand = new AudioRecordCommand(obj =>
                    {
                        try
                        {
                            StopMusic();
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Filter = "MP3 files|*.mp3";
                            if (fileDialog.ShowDialog() == true) AudioPath = fileDialog.FileName;
                            AudioName = GetMusicName(AudioPath);
                            AudioTime.TimerText = "0:0/0:0";
                            AudioTime.Stop();
                            RecordProgress.Stop();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        };
                    }));
            }
        }

        public AudioRecordCommand PlayPauseMusicCommand
        {
            get
            {
                return playPauseMusicCommand ?? (playPauseMusicCommand = new AudioRecordCommand(obj =>
                {
                    if (OutputDevice == null || OutputDevice.PlaybackState == PlaybackState.Paused)
                    {
                        PlayMusic();
                        TotalAudioSecondsTime = Audio.TotalTime.TotalSeconds;
                        ChangePlayPauseImage(false);
                        AudioTime.Start(Audio);
                        RecordProgress.Start(Convert.ToInt32(Audio.TotalTime.TotalSeconds), MAX_PROGRESS_LEN);
                    }
                    else
                    {
                        try
                        {
                        PauseMusic();
                        ChangePlayPauseImage(true);
                        AudioTime.Pause();
                        RecordProgress.Pause();
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }, (obj)=> audioPath != null));
            }
        }

        public AudioRecordCommand RestartMusicCommand
        {
            get
            {
                return restartMusicCommand ?? (restartMusicCommand = new AudioRecordCommand(obj =>
                {
                    SetAudioTime(new TimeSpan(0,0,0));
                    ChangePlayPauseImage(false);
                    PlayMusic();
                },(obj)=> Audio != null));
            }
        }

        private void SetAudioTime(TimeSpan timeSpan)
        {
            Audio.CurrentTime = timeSpan;
            AudioTime.Stop();
            RecordProgress.Stop();
            AudioTime.Start(Audio);
            RecordProgress.Start(Convert.ToInt32(Audio.TotalTime.TotalSeconds), MAX_PROGRESS_LEN);
            RecordProgress.IndicatorCoord = timeSpan.TotalSeconds * MAX_PROGRESS_LEN / Audio.TotalTime.TotalSeconds;
        }

        private void PlayMusic()
        {
            if (OutputDevice == null)
            {
                OutputDevice = new WaveOutEvent();
                OutputDevice.PlaybackStopped += PlaybackStoped;
            }
            if (Audio == null)
            {
                Audio = new AudioFileReader(AudioPath);
                OutputDevice.Init(Audio);
            }
            OutputDevice.Play();
        }

        private void PauseMusic()
        {
            OutputDevice?.Pause();
        }

        private void PlaybackStoped(object sender, StoppedEventArgs e)
        {
            ChangePlayPauseImage(true);
        }

        public void PlayBackStoped()
        {
            OutputDevice?.Dispose();
            OutputDevice = null;
            Audio?.Dispose();
            Audio = null;
        }

        private void ChangePlayPauseImage(bool stop)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            if (stop) image.UriSource = new Uri(playButtonImagePath);
            else image.UriSource = new Uri(pauseButtonImagePath);
            image.EndInit();
            PlayPauseImageSource = image;
        }

        private string GetMusicName(string path)
        {
            int slashIndex = 0;
            for (int i = path.Length - 1; i > 0; i--)
            {
                if (path[i] == '\\')
                {
                    slashIndex = i;
                    break;
                }
            }
            return path.Substring(slashIndex + 1, path.Length - 1 - slashIndex);
        }

        private void StopMusic()
        {
            OutputDevice?.Stop();
        }

        public string AudioPath
        {
            get { return audioPath; }
            set
            {
                audioPath = value;
                OnPropertyChanged("AudioPath");
            }
        }

        public string AudioName
        {
            get { return audioName; }
            set
            {
                audioName = value;
                OnPropertyChanged("AudioName");
            }
        }

        public BitmapImage PlayPauseImageSource
        {
            get { return playPauseImageSource; }
            set
            {
                playPauseImageSource = value;
                OnPropertyChanged("PlayPauseImageSource");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
