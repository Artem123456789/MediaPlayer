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
    /// Логика взаимодействия для EditPlaylistWindow.xaml
    /// </summary>
    public partial class EditPlaylistWindow : Window
    {

        WindowControll windowControll;
        public string NewHeader { get; set; }

        public EditPlaylistWindow()
        {
            InitializeComponent();
            windowControll = (WindowControll)this.Resources["WindowControll"];
            windowControll.CurrentWindow = this;
        }

        private void SaveNewHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            NewHeader = NewHeaderTextBox.Text;
            Close();
        }
    }
}
