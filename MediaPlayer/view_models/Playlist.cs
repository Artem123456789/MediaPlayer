using MediaPlayer.views;
using Microsoft.Win32;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class Playlist : INotifyPropertyChanged
    {
        public ObservableCollection<AudioRecord> AudioRecords { get; set; }
        public AudioRecord CurrentRecord
        { 
            get
            {
                return currentRecord;
            }
            set
            {
                currentRecord = value;
                OnPropertyChanged("CurrentRecord");
            }
        }
        public int CurrentRecordIndex { get; set; }
        public AudioRecordCommand AddAudio
        {
            get
            {
                return addAudio ?? (addAudio = new AudioRecordCommand(obj =>
                {
                    try
                    {
                        AudioRecord audioRecord = new AudioRecord(this);
                        audioRecord.BeforeChoose();
                        OpenFileDialog openFileDialog = new OpenFileDialog();
                        OpenFileDialog fileDialog = new OpenFileDialog();
                        fileDialog.Filter = "MP3 files|*.mp3";
                        if (fileDialog.ShowDialog() == true) audioRecord.AudioPath = fileDialog.FileName;
                        audioRecord.AfterChoose();
                        AudioRecords.Add(audioRecord);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }));
            }
        }   
        public AudioRecordCommand Play
        {
            get
            {
                return play ?? (play = new AudioRecordCommand(obj =>
                {
                    IsPlaying = !IsPlaying;
                    if(IsPlaying) ParentCollection.ChooseCurrentPlaylist(this);
                    else
                    {
                        CurrentRecord.StopMusic();
                        CurrentRecord.PlayBackStoped();
                    }
                    if (CurrentRecord == null)
                    {
                        ChoosePlayingAudio(AudioRecords.ElementAt(0));
                        CurrentRecordIndex = 0;
                    }
                    ChangePlayPauseImage();
                }, (obj)=>AudioRecords.Count > 0));
            }
        }
        public AudioRecordCommand EditName
        {
            get
            {
                return editName ?? (editName = new AudioRecordCommand(obj =>
                {
                    EditPlaylistWindow window = new EditPlaylistWindow();
                    window.ShowDialog();
                    Header = window.NewHeader;
                }));
            }
        }
        public AudioRecordCommand Remove
        {
            get
            {
                return remove ?? (remove = new AudioRecordCommand(obj =>
                {
                    ParentCollection.RemovePlaylist(this);
                }));
            }
        }
        public BitmapImage PlayPauseImage
        {
            get
            {
                return playPauseImage;
            }
            set
            {
                playPauseImage = value;
                OnPropertyChanged("PlayPauseImage");
            }
        }
        public string Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
                OnPropertyChanged("Header");
            }
        }
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }
            set
            {
                isPlaying = value;
                OnPropertyChanged("IsPlaying");
            }
        }

        public Playlist Copy()
        {
            Playlist playlist = new Playlist(ParentCollection);
            foreach (var item in AudioRecords)
            {
                playlist.AudioRecords.Add(item);
            }
            playlist.CurrentRecord = CurrentRecord;
            playlist.Header = Header;
            playlist.IsPlaying = IsPlaying;
            return playlist;
        }

        AudioRecordCommand addAudio;
        AudioRecordCommand play;
        AudioRecordCommand remove;
        AudioRecordCommand editName;
        AudioRecord currentRecord;
        BitmapImage playPauseImage;
        public PlaylistsCollection ParentCollection { get; set; }
        string header;
        bool isPlaying;

        public Playlist()
        {
            AudioRecords = new ObservableCollection<AudioRecord>();
            IsPlaying = false;
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                if (isPlaying)
                {
                    image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/pause-playlist.png");
                }
                else
                {
                    image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/play-playlist.png");
                }
                image.EndInit();
                PlayPauseImage = image;
            }
        }

        public Playlist(PlaylistsCollection ParentCollection)
        {
            AudioRecords = new ObservableCollection<AudioRecord>();
            IsPlaying = false;
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                if (isPlaying)
                {
                    image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/pause-playlist.png");
                }
                else
                {
                    image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/play-playlist.png");
                }
                image.EndInit();
                PlayPauseImage = image;
            }
            this.ParentCollection = ParentCollection;
        }

        public void ChoosePlayingAudio(AudioRecord audioRecord)
        {
            if (!IsPlaying)
            {
                ParentCollection.ChooseCurrentPlaylist(this);
                IsPlaying = true;
            }
            try { CurrentRecord.IsPlayingInPlaylist = false; }
            catch(NullReferenceException){}
            CurrentRecord = audioRecord;
            CurrentRecord.IsPlayingInPlaylist = true;
            ChangePlayPauseImage();
        }

        public void ChangePlayPauseImage()
        {
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            if (IsPlaying)
            {
                image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/pause-playlist.png");
                CurrentRecord.IsPlayingInPlaylist = true;
            }
            else
            {
                image.UriSource = new Uri("pack://application:,,,/MediaPlayer;component/music_control_images/play-playlist.png");
                CurrentRecord.IsPlayingInPlaylist = false;
            }
            image.EndInit();
            PlayPauseImage = image;
        }

        public void RemoveAudio(AudioRecord audioRecord) => AudioRecords.Remove(audioRecord);

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
