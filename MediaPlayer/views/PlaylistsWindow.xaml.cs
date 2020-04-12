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
    /// Логика взаимодействия для PlaylistsWindow.xaml
    /// </summary>
    public partial class PlaylistsWindow : Window
    {

        WindowControll windowControll;
        PlaylistsCollection playlistsCollection;

        public PlaylistsWindow()
        {
            InitializeComponent();
            windowControll = (WindowControll)this.Resources["WindowControll"];
            playlistsCollection = (PlaylistsCollection)this.Resources["PlaylistsCollection"];
            windowControll.CurrentWindow = this;
            windowControll.RestoreWindowImage = RestoreWindowImage;
            playlistsCollection.Playlists.Add(new Playlist(playlistsCollection) { Header = "XX1" });
            playlistsCollection.Playlists.ElementAt(0).AudioRecords.Add(new AudioRecord(playlistsCollection.Playlists.ElementAt(0)) { AudioName = "test1" });
            playlistsCollection.Playlists.ElementAt(0).AudioRecords.Add(new AudioRecord(playlistsCollection.Playlists.ElementAt(0)) { AudioName = "test2" });
            playlistsCollection.Playlists.ElementAt(0).AudioRecords.Add(new AudioRecord(playlistsCollection.Playlists.ElementAt(0)) { AudioName = "test3" });
        }
    }
}
