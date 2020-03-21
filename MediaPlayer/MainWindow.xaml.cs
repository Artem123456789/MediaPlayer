using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System.ComponentModel;

namespace MediaPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        WaveOutEvent outputDevice;
        AudioFileReader audioFile;
        AudioTime musicTime;
        string playingMusicFileName;

        public MainWindow()
        {
            InitializeComponent();
            musicTime = (AudioTime)this.Resources["MusicTimer"];
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void HideWindowButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RestoreWindowButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            if (WindowState == WindowState.Normal)
            {
                bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/window_control_images/normal-window.png");
                WindowState = WindowState.Maximized;
            }
            else
            {
                bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/window_control_images/maximize-window.png");
                WindowState = WindowState.Normal;
            }
            bitmapImage.EndInit();
            RestoreWindowImage.Source = bitmapImage;
        }

        private void ChooseMusicButton_Click(object sender, RoutedEventArgs e)
        {
            StopMusic();
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "MP3 files|*.mp3";
            if(fileDialog.ShowDialog() == true) playingMusicFileName = fileDialog.FileName;
            MusicNameTextBox.Text = GetMusicName(playingMusicFileName);
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

        private void PlayPauseMusicButton_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            if(outputDevice == null || outputDevice.PlaybackState == PlaybackState.Paused)
            {
                PlayMusic();
                bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/stop-music.png");
                musicTime.Start(audioFile);
            }
            else
            {
                PauseMusic();
                bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/play-music.png");
                musicTime.Pause();
            }
            bitmapImage.EndInit();
            PlayPauseMusicImage.Source = bitmapImage;
        }

        private void PlayMusic()
        {
            if(outputDevice == null)
            {
                outputDevice = new WaveOutEvent();
                outputDevice.PlaybackStopped += PlaybackStoped;
            }
            if(audioFile == null)
            {
                audioFile = new AudioFileReader(playingMusicFileName);
                outputDevice.Init(audioFile);
            }
            outputDevice.Play();
        }

        private void PauseMusic()
        {
            outputDevice?.Pause();
        }

        private void StopMusic()
        {
            outputDevice?.Stop();
        }

        private void PlaybackStoped(object sender, StoppedEventArgs e)
        {
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/play-music.png");
            image.EndInit();
            PlayPauseMusicImage.Source = image;
        }

        private void PlaybackStoped(object sender, CancelEventArgs e)
        {
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
        }
    }
}
