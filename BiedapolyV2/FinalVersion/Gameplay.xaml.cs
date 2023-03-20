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
    /// Logika interakcji dla klasy Gameplay.xaml
    /// </summary>
    public partial class Gameplay : Window
    {
        public Gameplay()
        {
            InitializeComponent();
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow.program.Close();
        }

        private void btnInstructions_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow.program.Close();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            Close();
            MainWindow.program.Close();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Map.Height=this.Height -35;
            Map.Width=this.Height -35;
            //lstPlayers.Height=this.Height -30;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.program.Close();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            Map.Height = this.Height - 35;
            Map.Width = this.Height - 35;
        }
    }
}
