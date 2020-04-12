using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.view_models
{
    public class PlaylistsCollection : INotifyPropertyChanged
    {
        public ObservableCollection<Playlist> Playlists { get; set; }
        public Playlist CurrentPlaylist 
        {
            get
            {
                return currentPlaylist;
            }
            set
            {
                currentPlaylist = value;
                OnPropertyChanged("CurrentPlaylist");
            }
        }
        public int CurrentPlaylistIndex { get; set; }
        public string NewPlaylistName
        {
            get
            {
                return newPlaylistName;
            }
            set
            {
                newPlaylistName = value;
                OnPropertyChanged("NewPlaylistName");
            }
        }
        public AudioRecordCommand AddPlaylist
        {
            get
            {
                return addPlaylist ?? (addPlaylist = new AudioRecordCommand(obj =>
                {
                    Playlists.Add(new Playlist(this) { Header = NewPlaylistName });
                }));
            }
        }
        public Playlist DefaultPlaylist { get; set; }

        AudioRecordCommand addPlaylist;
        Playlist currentPlaylist;
        string newPlaylistName;

        public PlaylistsCollection()
        {
            Playlists = new ObservableCollection<Playlist>();
            DefaultPlaylist = new Playlist();
            CurrentPlaylist = DefaultPlaylist;
        }

        public void RemovePlaylist(Playlist playlist) => Playlists.Remove(playlist);
        public void ChooseCurrentPlaylist(Playlist playlist) => CurrentPlaylist = playlist;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
