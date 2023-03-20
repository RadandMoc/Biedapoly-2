using Biedapoly;
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

namespace FinalVersion
{
    /// <summary>
    /// Logika interakcji dla klasy Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        
        public Menu()
        {
            InitializeComponent();
            
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow.program.Close();
        }

        private void btnInstruction_Click(object sender, RoutedEventArgs e)
        {

            Close();
            MainWindow.program.Close();
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Document"; // Default file name
            dialog.DefaultExt = ".bin"; // Default file extension
            dialog.Filter = "Binary documents (.bin)|*.bin"; // Filter files by extension
            
            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                MainWindow.program.gameplay = (Game)Game.ReadBinary(filename);
               
            }
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            MapSelector mapSelector = new MapSelector();
            foreach (Map item in MainWindow.program.possibleMaps)
            {
                mapSelector.lstMaps.Items.Add(item);
            }
            mapSelector.Show();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.program.Close();
        }
    }
}
