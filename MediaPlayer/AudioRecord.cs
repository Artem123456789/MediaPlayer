using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MediaPlayer
{
    class AudioRecord:INotifyPropertyChanged
    {
        public WaveOutEvent OutputDevice { get; set; }
        public AudioFileReader Audio { get; set; }
        public AudioTime AudioTime { get; set; }
        AudioRecordCommand chooseAudioCommand;
        AudioRecordCommand playPauseMusicCommand;
        BitmapImage playPauseImageSource;
        string audioPath;
        string audioName;
        string playButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/play-music.png";
        string pauseButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/stop-music.png";

        public AudioRecord()
        {
            AudioTime = new AudioTime();
            ChangePlayPauseImage(true);
        }

        public AudioRecordCommand ChooseAudioCommand
        {
            get
            {
                return chooseAudioCommand ??
                    (chooseAudioCommand = new AudioRecordCommand(obj =>
                    {
                        StopMusic();
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "MP3 files|*.mp3";
                        if (fileDialog.ShowDialog() == true) AudioPath = fileDialog.FileName;
                        AudioName = GetMusicName(AudioPath);
                        AudioTime.TimerText = "0:0/0:0";
                        AudioTime.Stop();
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
                        ChangePlayPauseImage(false);
                        AudioTime.Start(Audio);
                    }
                    else
                    {
                        PauseMusic();
                        ChangePlayPauseImage(true);
                        AudioTime.Pause();
                    }
                }));
            }
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
            OutputDevice?.Dispose();
            OutputDevice = null;
            Audio?.Dispose();
            Audio = null;
            ChangePlayPauseImage(true);
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
                    slashIndex = path.Length - i;
                    break;
                }
            }
            return path.Substring(slashIndex, path.Length - slashIndex);
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
