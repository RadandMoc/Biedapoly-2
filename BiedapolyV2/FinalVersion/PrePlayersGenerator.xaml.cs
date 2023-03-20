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
    /// Logika interakcji dla klasy PrePlayersGenerator.xaml
    /// </summary>
    public partial class PrePlayersGenerator : Window
    {
        public PrePlayersGenerator()
        {
            InitializeComponent();
        }

        #region textBoxes
        private void txtNick0_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[0].Name = txtNick0.Text;
        }
        private void txtNick1_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[1].Name = txtNick1.Text;
        }
        private void txtNick2_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[2].Name = txtNick2.Text;
        }
        private void txtNick3_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[3].Name = txtNick3.Text;
        }
        private void txtNick4_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[4].Name = txtNick4.Text;
        }
        private void txtNick5_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[5].Name = txtNick5.Text;
        }
        private void txtNick6_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[6].Name = txtNick6.Text;
        }
        private void txtNick7_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[7].Name = txtNick7.Text;
        }
        private void txtNick8_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[8].Name = txtNick8.Text;
        }
        private void txtNick9_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[9].Name = txtNick9.Text;
        }
        private void txtNick10_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[10].Name = txtNick10.Text;
        }
        private void txtNick11_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[11].Name = txtNick11.Text;
        }
        private void txtNick12_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[12].Name = txtNick12.Text;
        }
        private void txtNick13_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[13].Name = txtNick13.Text;
        }
        private void txtNick14_TextChanged(object sender, TextChangedEventArgs e)
        {
            MainWindow.program.prePlayers[14].Name = txtNick14.Text;
        }
        #endregion textBoxes

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MapSelector mapSelector = new MapSelector();
            foreach (Map item in MainWindow.program.possibleMaps)
            {
                mapSelector.lstMaps.Items.Add(item);
            }
            mapSelector.Show();
            Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.program.players.Clear();
            PlayerGenerator plGen = new PlayerGenerator();
            if(!(bool)this.cBox.IsChecked)
            {
                plGen.spSecond.Children.RemoveAt(plGen.spSecond.Children.Count - 1);
                plGen.spSecond.Children.RemoveAt(plGen.spSecond.Children.Count - 1);
            }
            //Queue<Player> heroes= new Queue<Player>();
            Player ple;
            int numPlayer = 0;
            foreach (var item in MainWindow.program.prePlayers)
            {
                if (item.Value.Name != "")
                {
                    ple = new Player();
                    ple.Name = item.Value.Name;
                    ple.PlayerHero = MainWindow.program.heroes.Peek();
                    ple.Money = (int)((ple.PlayerHero.Hero_pocket/100) * MainWindow.program.gameplay.Map.DefaultMoney[numPlayer]);
                    numPlayer++;
                    ple.Id = numPlayer;
                    MainWindow.program.players.Enqueue(ple);
                    plGen.cbPlayer.Items.Add(ple);
                    plGen.lstPlayerSequence.Items.Add(ple);
                }
            }/*
            foreach (var item in MainWindow.program.prePlayers)
            {
                if(item.Value.Name!="")
                    plGen.cbPlayer.Items.Add(item.Value);
            }*/
            plGen.Show();
            Close();
            //MainWindow.program.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.program.Close();
        }
    }
}
