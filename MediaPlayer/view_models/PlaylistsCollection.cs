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
    class PlaylistsCollection : INotifyPropertyChanged
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
        public AudioRecordCommand AddPlaylist
        {
            get
            {
                return addPlaylist ?? (addPlaylist = new AudioRecordCommand(obj =>
                {
                    Playlists.Add(obj as Playlist);
                }));
            }
        }
        public AudioRecordCommand RemovePlaylist
        {
            get
            {
                return removePlaylist ?? (removePlaylist = new AudioRecordCommand(obj =>
                {
                    Playlists.Remove(Playlists.ElementAt((int)obj));
                }));
            }
        }

        AudioRecordCommand addPlaylist;
        AudioRecordCommand removePlaylist;
        Playlist currentPlaylist;

        public PlaylistsCollection()
        {
            Playlists = new ObservableCollection<Playlist>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
