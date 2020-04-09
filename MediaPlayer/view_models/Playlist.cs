using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer.view_models
{
    class Playlist : INotifyPropertyChanged
    {
        public List<AudioRecord> AudioRecords { get; set; }
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
        AudioRecord currentRecord;
        string header;

        public Playlist()
        {
            AudioRecords = new List<AudioRecord>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
