using MediaPlayer.views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                    AudioRecords.Add(obj as AudioRecord);
                }));
            }
        }
        public AudioRecordCommand RemoveAudio
        {
            get
            {
                return removeAudio ?? (removeAudio = new AudioRecordCommand(obj =>
                {
                    AudioRecords.Remove(AudioRecords.ElementAt((int)obj));
                }));
            }
        }
        public AudioRecordCommand Play
        {
            get
            {
                return play ?? (play = new AudioRecordCommand(obj =>
                {
                    ChoosePlayingAudio(AudioRecords.ElementAt(0));
                }));
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
                    parentCollection.RemovePlaylist(this);
                }));
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

        AudioRecordCommand addAudio;
        AudioRecordCommand removeAudio;
        AudioRecordCommand play;
        AudioRecordCommand remove;
        AudioRecordCommand editName;
        AudioRecord currentRecord;
        PlaylistsCollection parentCollection;
        string header;

        public Playlist()
        {
            AudioRecords = new ObservableCollection<AudioRecord>();
        }

        public Playlist(PlaylistsCollection parentCollection)
        {
            AudioRecords = new ObservableCollection<AudioRecord>();
            this.parentCollection = parentCollection;
        }

        public void ChoosePlayingAudio(AudioRecord audioRecord)
        {
            try { CurrentRecord.IsPlayingInPlaylist = false; }
            catch(NullReferenceException){}
            CurrentRecord = audioRecord;
            CurrentRecord.IsPlayingInPlaylist = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
