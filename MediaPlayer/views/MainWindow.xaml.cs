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
        WindowControll windowControll;
        view_models.Menu menu;

        public MainWindow()
        {
            InitializeComponent();
            windowControll = (WindowControll)this.Resources["WindowControll"];
            menu = (view_models.Menu)this.Resources["Menu"];
            menu.Collection = (PlaylistsCollection)this.Resources["PlaylistsCollection"];
            menu.BeforeInit();
            windowControll.CurrentWindow = this;
            windowControll.RestoreWindowImage = RestoreWindowImage;
        }

        private void MoveAudio(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            menu.Collection.CurrentPlaylist.CurrentRecord.MoveAudio(sender);
        }

        private void MovingAudio(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            menu.Collection.CurrentPlaylist.CurrentRecord.MovingAudio(sender);
        }

        private void PlaybackStoped(object sender, CancelEventArgs e)
        {
            menu.Collection.CurrentPlaylist.CurrentRecord.PlayBackStoped();
        }
    }
}
