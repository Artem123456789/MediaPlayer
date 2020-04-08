using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer
{
    public class SettingsAudioRecord : INotifyPropertyChanged
    {
        public AudioSwitcher.AudioApi.CoreAudio.CoreAudioDevice defaultPlayBack;
        string audioName;
        bool isLoop;

        public SettingsAudioRecord()
        {
            defaultPlayBack = new AudioSwitcher.AudioApi.CoreAudio.CoreAudioController().DefaultPlaybackDevice;
        }

        public string AudioName
        {
            get
            {
                return audioName;
            }
            set
            {
                audioName = value;
                OnPropertyChanged("AudioName");
            }
        }
        public bool IsLoop
        {
            get
            {
                return isLoop;
            }
            set
            {
                isLoop = value;
                OnPropertyChanged("IsLoop");
            }
        }
        public double VolumeLevel
        {
            get
            {
                return defaultPlayBack.Volume;
            }
            set
            {
                defaultPlayBack.Volume = value;
                OnPropertyChanged("VolumeLevel");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
