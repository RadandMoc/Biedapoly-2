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
    /// Logika interakcji dla klasy MapSelector.xaml
    /// </summary>
    public partial class MapSelector : Window
    {
        public MapSelector()
        {
            //lstMaps.Items.Clear();
            InitializeComponent();
            /*if(lstMaps.Items != null)
                lstMaps.Items.Clear();*/
            
        }

        private void lstMaps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MainWindow.program.gameplay.Map = (Map)lstMaps.SelectedItem;
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Menu menu = new Menu();
            menu.Show();
            Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.program.gameplay.Map != null)
            {
                PrePlayersGenerator generator = new PrePlayersGenerator();
                MainWindow.program.prePlayers.Clear();
                for (int i = 16; i > 1 + MainWindow.program.gameplay.Map.MaxNumOfPlayers; i--)
                {
                    generator.spSecond.Children.RemoveAt(generator.spSecond.Children.Count - 1);
                    if (generator.Height >= 490)
                        generator.Height -= 40;
                    else if (generator.Height > 450)
                        generator.Height = 450;
                }
                for (int i = 0; i < MainWindow.program.gameplay.Map.MaxNumOfPlayers; i++)
                {
                    MainWindow.program.prePlayers.Add(i, new PrePlayer(""));
                }
                generator.Show();
                Close();
            }
        }
    }
}
