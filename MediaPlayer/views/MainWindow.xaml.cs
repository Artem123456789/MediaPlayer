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
using MediaPlayer.view_models;
using MediaPlayer.views;

namespace MediaPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        AudioRecord playingRecord;

        public MainWindow()
        {
            InitializeComponent();
            playingRecord = (AudioRecord)this.Resources["PlayingRecord"];
        }
            
        private void OpenSettings(object sender, RoutedEventArgs e)
        {
            SettingsWindow window = new SettingsWindow();
            window.AudioRecord.OutputDevice = playingRecord.OutputDevice;
            window.AudioRecord.Audio = playingRecord.Audio;
            window.AudioRecord.AudioName = playingRecord.AudioName;
            window.ShowDialog();
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

        private void MoveAudio(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            playingRecord.MoveAudio(sender);
        }

        private void MovingAudio(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            playingRecord.MovingAudio(sender);
        }

        private void PlaybackStoped(object sender, CancelEventArgs e)
        {
            playingRecord.PlayBackStoped();
        }
    }
}
