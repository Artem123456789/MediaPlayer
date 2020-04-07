using MediaPlayer.view_models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MediaPlayer.views
{
    /// <summary>
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public AudioRecord AudioRecord { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();
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
    }
}
