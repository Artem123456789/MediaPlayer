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
        public SettingsAudioRecord AudioRecord { get; set; }
        WindowControll windowControll;

        public SettingsWindow()
        {
            InitializeComponent();
            AudioRecord = (SettingsAudioRecord)this.Resources["AudioRecord"];
            windowControll = (WindowControll)this.Resources["WindowControll"];
            windowControll.CurrentWindow = this;
            windowControll.RestoreWindowImage = RestoreWindowImage;
        }
    }
}
