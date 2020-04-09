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
        WindowControll windowControll;

        public MainWindow()
        {
            InitializeComponent();
            playingRecord = (AudioRecord)this.Resources["PlayingRecord"];
            windowControll = (WindowControll)this.Resources["WindowControll"];
            windowControll.CurrentWindow = this;
            windowControll.RestoreWindowImage = RestoreWindowImage;
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
