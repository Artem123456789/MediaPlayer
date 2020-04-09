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

        public WindowControllCommand DragWindow
        {
            get
            {
                return dragWindow ?? (dragWindow = new WindowControllCommand(obj =>
                {
                    CurrentWindow.DragMove();
                }));
            }
        }
        public WindowControllCommand HideWindow
        {
            get
            {
                return hideWindow ?? (hideWindow = new WindowControllCommand(obj =>
                {
                    CurrentWindow.WindowState = WindowState.Minimized;
                }));
            }
        }
        public WindowControllCommand CloseWindow
        {
            get
            {
                return closeWindow ?? (closeWindow = new WindowControllCommand(obj =>
                {
                    CurrentWindow.Close();
                }));
            }
        }
        public WindowControllCommand RestoreWindow
        {
            get
            {
                return restoreWindow ?? (restoreWindow = new WindowControllCommand(obj =>
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

        WindowControllCommand dragWindow;
        WindowControllCommand hideWindow;
        WindowControllCommand closeWindow;
        WindowControllCommand restoreWindow;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
