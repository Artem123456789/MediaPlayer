using MediaPlayer.views;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MediaPlayer.view_models
{
    public class Menu:INotifyPropertyChanged
    {
        public AudioRecord SingleAudio { get; set; }
        public Playlist DefaultPlaylist { get; set; }
        public PlaylistsCollection Collection 
        {
            get
            {
                return collection;
            }
            set
            {
                collection = value;
                OnPropertyChanged("Collection");
            }
        }
        public AudioRecordCommand ChooseAudio
        {
            get
            {
                return chooseAudio ?? (chooseAudio = new AudioRecordCommand(obj =>
                {
                    try
                    {
                        Collection.CurrentPlaylist = DefaultPlaylist;
                        DefaultPlaylist.CurrentRecord.BeforeChoose();
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "MP3 files|*.mp3";
                        if (fileDialog.ShowDialog() == true) DefaultPlaylist.CurrentRecord.AudioPath = fileDialog.FileName;
                            DefaultPlaylist.CurrentRecord.AfterChoose();
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
                    window.AudioRecord.AudioName = DefaultPlaylist.CurrentRecord.AudioName;
                    window.AudioRecord.IsLoop = DefaultPlaylist.CurrentRecord.IsLoop;
                    window.ShowDialog();
                    DefaultPlaylist.CurrentRecord.IsLoop = window.AudioRecord.IsLoop;
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
                    foreach (var item in Collection.Playlists)
                    {
                        item.ParentCollection = playlistsWindow.PlaylistsCollection;
                        playlistsWindow.PlaylistsCollection.Playlists.Add(item);
                    }
                    playlistsWindow.PlaylistsCollection.CurrentPlaylist = Collection.CurrentPlaylist;
                    playlistsWindow.ShowDialog();
                    Collection.Playlists = playlistsWindow.PlaylistsCollection.Playlists;
                    try { Collection.CurrentPlaylist = playlistsWindow.PlaylistsCollection.CurrentPlaylist; }
                    catch (NullReferenceException) { MessageBox.Show("f"); }
                }));
            }
        }

        AudioRecordCommand chooseAudio;
        AudioRecordCommand openSettings;
        AudioRecordCommand openPlaylists;
        PlaylistsCollection collection;

        public Menu()
        {
            SingleAudio = new AudioRecord();
            DefaultPlaylist = new Playlist();
            SingleAudio.ParentPlayList = DefaultPlaylist;
            Collection = new PlaylistsCollection();
            Collection.CurrentPlaylist = DefaultPlaylist;
            DefaultPlaylist.AudioRecords.Add(SingleAudio);
            DefaultPlaylist.CurrentRecord = SingleAudio;
        }

        public void BeforeInit()
        {
            SingleAudio = new AudioRecord();
            DefaultPlaylist = new Playlist();
            SingleAudio.ParentPlayList = DefaultPlaylist;
            Collection.CurrentPlaylist = DefaultPlaylist;
            DefaultPlaylist.AudioRecords.Add(SingleAudio);
            DefaultPlaylist.CurrentRecord = SingleAudio;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
