using MediaPlayer.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer.view_models
{
    public class Menu
    {
        public AudioRecord SingleAudio { get; set; }
        public AudioRecordCommand ChooseAudio
        {
            get
            {
                return chooseAudio ?? (chooseAudio = new AudioRecordCommand(obj =>
                {
                    try
                    {

                    SingleAudio.BeforeChoose();
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Filter = "MP3 files|*.mp3";
                    if (fileDialog.ShowDialog() == true) SingleAudio.AudioPath = fileDialog.FileName;
                    SingleAudio.AfterChoose();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }
        public AudioRecordCommand OpenSettings
        {
            get
            {
                return openSettings ?? (openSettings = new AudioRecordCommand(obj =>
                {

                    SettingsWindow window = new SettingsWindow();
                    window.AudioRecord.AudioName = SingleAudio.AudioName;
                    window.AudioRecord.IsLoop = SingleAudio.IsLoop;
                    window.ShowDialog();
                    SingleAudio.IsLoop = window.AudioRecord.IsLoop;
                }));
            }
        }
        public AudioRecordCommand OpenPlaylists
        {
            get
            {
                return openPlaylists ?? (openPlaylists = new AudioRecordCommand(obj =>
                {
                    PlaylistsWindow playlistsWindow = new PlaylistsWindow();
                    playlistsWindow.ShowDialog();
                }));
            }
        }

        AudioRecordCommand chooseAudio;
        AudioRecordCommand openSettings;
        AudioRecordCommand openPlaylists;

        public Menu()
        {
            SingleAudio = new AudioRecord();
        }
    }
}
