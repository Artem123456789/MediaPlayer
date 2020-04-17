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
        //properties
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
        public Playlist DefaultPlaylist { get; set; }

        //commands properties
        public AudioRecordCommand AddPlaylist
        {
            get
            {
                return addPlaylist ?? (addPlaylist = new AudioRecordCommand(obj =>
                {
                    Playlists.Add(new Playlist(this) { Header = NewPlaylistName });
                    NewPlaylistName = string.Empty;
                }));
            }
        }

        //fields
        AudioRecordCommand addPlaylist;
        Playlist currentPlaylist;
        string newPlaylistName;

        /// <summary>
        /// Default constructor. Sets default values
        /// </summary>
        public PlaylistsCollection()
        {
            Playlists = new ObservableCollection<Playlist>();
            DefaultPlaylist = new Playlist();
            CurrentPlaylist = DefaultPlaylist;
        }

        /// <summary>
        /// Removing playlist
        /// </summary>
        /// <param name="playlist">Playlist on remove</param>
        public void RemovePlaylist(Playlist playlist) => Playlists.Remove(playlist);
        /// <summary>
        /// Selects current playlist
        /// </summary>
        /// <param name="playlist">The playlist that becomes the current one</param>
        public void ChooseCurrentPlaylist(Playlist playlist)
        {
            if (CurrentPlaylist.Header != DefaultPlaylist.Header)
            {
                CurrentPlaylist.IsPlaying = false;
                CurrentPlaylist.CurrentRecord.StopMusic();
                CurrentPlaylist.CurrentRecord.PlayBackStoped();
                CurrentPlaylist.CurrentRecord.AudioTime.Stop();
                CurrentPlaylist.CurrentRecord.AudioProgress.Stop();
                CurrentPlaylist.ChangePlayPauseImage();
            }
            CurrentPlaylist = playlist;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
