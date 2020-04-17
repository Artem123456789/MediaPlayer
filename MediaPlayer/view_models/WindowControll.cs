using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaPlayer.view_models
{
    public class WindowControll: INotifyPropertyChanged
    {
        public WindowControll() { }

        public WindowControll(Window window, Image restoreImage)
        {
            CurrentWindow = window;
            RestoreWindowImage = restoreImage;
        }

        public AudioRecordCommand DragWindow
        {
            get
            {
                return dragWindow ?? (dragWindow = new AudioRecordCommand(obj =>
                {
                    CurrentWindow.DragMove();
                }));
            }
        }
        public AudioRecordCommand HideWindow
        {
            get
            {
                return hideWindow ?? (hideWindow = new AudioRecordCommand(obj =>
                {
                    CurrentWindow.WindowState = WindowState.Minimized;
                }));
            }
        }
        public AudioRecordCommand CloseWindow
        {
            get
            {
                return closeWindow ?? (closeWindow = new AudioRecordCommand(obj =>
                {
                    CurrentWindow.Close();
                }));
            }
        }
        public AudioRecordCommand RestoreWindow
        {
            get
            {
                return restoreWindow ?? (restoreWindow = new AudioRecordCommand(obj =>
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    if (CurrentWindow.WindowState == WindowState.Normal)
                    {
                        bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/window_control_images/normal-window.png");
                        CurrentWindow.WindowState = WindowState.Maximized;
                    }
                    else
                    {
                        bitmapImage.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/window_control_images/maximize-window.png");
                        CurrentWindow.WindowState = WindowState.Normal;
                    }
                    bitmapImage.EndInit();
                    RestoreWindowImage.Source = bitmapImage;
                }));
            }
        }

        public Window CurrentWindow { get; set; }
        public Image RestoreWindowImage { get; set; }

        AudioRecordCommand dragWindow;
        AudioRecordCommand hideWindow;
        AudioRecordCommand closeWindow;
        AudioRecordCommand restoreWindow;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
