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
        AudioRecord playingRecord;

        public MainWindow()
        {
            InitializeComponent();
            musicTime = (AudioTime)this.Resources["MusicTimer"];
            playingRecord = (AudioRecord)this.Resources["PlayingRecord"];
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

        private void PlaybackStoped(object sender, CancelEventArgs e)
        {
            outputDevice?.Dispose();
            outputDevice = null;
            audioFile?.Dispose();
            audioFile = null;
        }
    }
}
