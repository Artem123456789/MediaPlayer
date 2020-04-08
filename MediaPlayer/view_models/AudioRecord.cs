using GalaSoft.MvvmLight.Command;
using MediaPlayer.view_models;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace MediaPlayer
{

    /// <summary>
    /// The ViewModel that is responsible for all the logic of the audio being played.
    /// </summary>
    public class AudioRecord : INotifyPropertyChanged
    {
        //properties and simultaneously models for this ViewModel.
        public WaveOutEvent OutputDevice { get; set; }
        public AudioFileReader Audio { get; set; }
        public AudioTime AudioTime { get; set; }
        public AudioProgress AudioProgress { get; set; }

        //commands properties
        public AudioRecordCommand ChooseAudioCommand
        {
            get
            {
                return chooseAudioCommand ??
                    (chooseAudioCommand = new AudioRecordCommand(obj =>
                    {
                        try
                        {
                            PlayBackStoped();
                            StopMusic();
                            OpenFileDialog fileDialog = new OpenFileDialog();
                            fileDialog.Filter = "MP3 files|*.mp3";
                            if (fileDialog.ShowDialog() == true) AudioPath = fileDialog.FileName;
                            AudioName = GetMusicName(AudioPath);
                            AudioTime.TimerText = "0:0/0:0";
                            AudioTime.Stop();
                            AudioProgress.Stop();
                        }
                        catch (Exception ex)
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
                        PlayAudio();
                        TotalAudioSecondsTime = Audio.TotalTime.TotalSeconds;
                        ChangePlayPauseImage(false);
                        AudioTime.Start(Audio);
                        AudioProgress.Start(Audio);
                    }
                    else
                    {
                        try
                        {
                            PauseAudio();
                            ChangePlayPauseImage(true);
                            AudioTime.Pause();
                            AudioProgress.Pause();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }, (obj) => audioPath != null));
            }
        }
        public AudioRecordCommand RestartMusicCommand
        {
            get
            {
                return restartMusicCommand ?? (restartMusicCommand = new AudioRecordCommand(obj =>
                {
                    SetAudioTime(new TimeSpan(0, 0, 0));
                    ChangePlayPauseImage(false);
                    PlayAudio();
                }, (obj) => Audio != null));
            }
        }
        public AudioRecordCommand ShowHideVolumeControl
        {
            get
            {
                return showHideVolumeControl ?? (showHideVolumeControl = new AudioRecordCommand(obj =>
                {
                    if ((obj as RowDefinition).Height.Value == 0)
                        (obj as RowDefinition).Height = new GridLength(50, GridUnitType.Pixel);
                    else
                        (obj as RowDefinition).Height = new GridLength(0, GridUnitType.Pixel);
                }));
            }
        }

        //all other properties
        public BitmapImage PlayPauseImageSource
        {
            get { return playPauseImageSource; }
            set
            {
                playPauseImageSource = value;
                OnPropertyChanged("PlayPauseImageSource");
            }
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
        public float VolumeLevel
        {
            get
            {
                return OutputDevice.Volume;
            }
            set
            {
                OutputDevice.Volume = value / 100f;
                OnPropertyChanged("VolumeLevel");
            }
        }

        //fields
        AudioRecordCommand chooseAudioCommand;
        AudioRecordCommand playPauseMusicCommand;
        AudioRecordCommand restartMusicCommand;
        AudioRecordCommand showHideVolumeControl;
        BitmapImage playPauseImageSource;
        TimeSpan newTime;
        int movedSeconds;
        string audioPath;
        string audioName;
        string playButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/play-music.png";
        string pauseButtonImagePath = "pack://application:,,,/MediaPlayer;component/music_control_images/stop-music.png";
        double totalAudioSecondsTime;

        /// <summary>
        /// Default constructor. Sets default values
        /// </summary>
        public AudioRecord()
        {
            AudioTime = new AudioTime();
            AudioProgress = new AudioProgress();
            TotalAudioSecondsTime = 0;
            ChangePlayPauseImage(true);
        }

        /// <summary>
        /// Called when the slider that controls the audio progress is completed
        /// </summary>
        /// <param name="sender">The slider object is passed here</param>
        public void MoveAudio(object sender)
        {
            Slider slider = sender as Slider;
            AudioProgress.IndicatorCoord = slider.Value;
            newTime = TimeSpan.FromSeconds(movedSeconds);
            SetAudioTime(newTime);
        }

        /// <summary>
        /// Called when the slider slider is moving to control the audio progress
        /// </summary>
        /// <param name="sender">The slider object is passed here</param>
        public void MovingAudio(object sender)
        {
            movedSeconds = (int)(sender as Slider).Value;
            AudioTime.CurrentSeconds = TimeSpan.FromSeconds(movedSeconds).Seconds;
            AudioTime.CurrentMinutes = TimeSpan.FromSeconds(movedSeconds).Minutes;
        }

        [DllImport("user32.dll")]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("winmm.dll", EntryPoint = "waveOutGetVolume")]
        static extern void GetWaveVolume(IntPtr devicehandle, out int Volume);

        /// <summary>
        /// Adjusting volume
        /// </summary>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        private void AdjustVolume(int oldValue, int newValue)
        {
            if (oldValue > newValue)
            {
                for (int i = newValue; i < oldValue; i++)
                {
                    keybd_event((byte)System.Windows.Forms.Keys.VolumeDown, 0, 0, 0);
                }
            }
            else if (oldValue < newValue)
            {
                for (int i = oldValue; i < newValue; i++)
                {
                    keybd_event((byte)System.Windows.Forms.Keys.VolumeUp, 0, 0, 0);
                }
            }
        }

        /// <summary>
        /// Starts playing audio
        /// </summary>
        private void PlayAudio()
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

        /// <summary>
        /// Pauses audio playback
        /// </summary>
        private void PauseAudio()
        {
            OutputDevice?.Pause();
        }

        /// <summary>
        /// Peridural audio on the time span specified as a parameter
        /// </summary>
        /// <param name="timeSpan">New audio time</param>
        private void SetAudioTime(TimeSpan timeSpan)
        {
            try
            {
                Audio.CurrentTime = timeSpan;
                AudioTime.Stop();
                AudioProgress.Stop();
                AudioTime.Start(Audio);
                AudioProgress.Start(Audio);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Pleace choose and play aduio");
            }
        }

        /// <summary>
        /// Called when audio playback stops
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlaybackStoped(object sender, StoppedEventArgs e)
        {
            ChangePlayPauseImage(true);
        }

        /// <summary>
        /// Called to force audio to stop
        /// </summary>
        public void PlayBackStoped()
        {
            OutputDevice?.Dispose();
            OutputDevice = null;
            Audio?.Dispose();
            Audio = null;
        }

        /// <summary>
        /// Changes play or pause image
        /// </summary>
        /// <param name="stop"></param>
        private void ChangePlayPauseImage(bool stop)
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            if (stop) image.UriSource = new Uri(playButtonImagePath);
            else image.UriSource = new Uri(pauseButtonImagePath);
            image.EndInit();
            PlayPauseImageSource = image;
        }

        /// <summary>
        /// Gets the short name of the audio
        /// </summary>
        /// <param name="path">Full audio path</param>
        /// <returns>Short name of the audio</returns>
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

        /// <summary>
        /// Stops the music
        /// </summary>
        private void StopMusic()
        {
            OutputDevice?.Stop();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
