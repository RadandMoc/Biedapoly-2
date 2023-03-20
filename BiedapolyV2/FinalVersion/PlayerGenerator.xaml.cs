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
    /// Logika interakcji dla klasy PlayerGenerator.xaml
    /// </summary>
    public partial class PlayerGenerator : Window
    {
        public PlayerGenerator()
        {
            InitializeComponent();
        }

        private void txtMoney_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                ((Player)cbPlayer.SelectedItem).Money = Int32.Parse(txtMoney.Text);
            }
            catch (Exception)
            {
                ((Player)cbPlayer.SelectedItem).Money = 1;
            }
        }


        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.program.players = Methods.RandomizePlayers(MainWindow.program.players);
            lstPlayerSequence.Items.Clear();
            //int num = 0;
            foreach (Player item in MainWindow.program.players)
            {
                //num++;
                //item.Id = num;
                lstPlayerSequence.Items.Add(item);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //int num = 0;
            Queue<Player> queue = new Queue<Player>();
            foreach (Player item in cbPlayer.Items)
            {
                queue.Enqueue(item);
                //item.WasChangeOfLane = false;
                //item.Id = num;
                //item.Money = 15000;
                //MainWindow.program.gameplay.Players.Enqueue(item);
                //num++;
            //MainWindow.program.gameplay.Players.Count()
            }
            MainWindow.program.gameplay.Players = new Queue<Player>();
            foreach (Player item in lstPlayerSequence.Items)
            {
                foreach (Player item2 in queue)
                {
                    if (item2.Id == item.Id)
                    {
                        item.Coordinates = MainWindow.program.gameplay.Map.StartCoordinates;
                        item.PlayerBonuses = new Dictionary<string, string>();
                        Methods.BonusesFromHeroToPlayer(item);
                        MainWindow.program.gameplay.Players.Enqueue(item);
                    }
                }
            }
            Gameplay game = new Gameplay();
            int helper = game.lstPlayers.Items.Count;
            for (int i = 0; i < helper - MainWindow.program.players.Count ; i++)
            {
                game.lstPlayers.Items.RemoveAt(game.lstPlayers.Items.Count - 1);
            }
            foreach (Canvas item in game.lstPlayers.Items)
            {
                int num = 0;
                foreach (var item2 in item.Children)
                {
                    if(num == 0)
                    {
                        ((Label)item2).Content = MainWindow.program.gameplay.Players.Peek().Name;
                    }
                    else if(num ==2)
                    {
                        ((Label)item2).Content = MainWindow.program.gameplay.Map.DictionaryOfTerritories[MainWindow.program.gameplay.Players.Peek().Coordinates].Name;
                    }
                    else if (num == 3)
                    {
                        ((Label)item2).Content = $"Money: \n\n{MainWindow.program.gameplay.Players.Peek().Money}";
                    }
                    num++;
                }
                MainWindow.program.gameplay.Players.Enqueue(MainWindow.program.gameplay.Players.Dequeue());
            }
            game.Show();
            Close();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
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

        private void txtTime_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                MainWindow.program.gameplay.HowLongWeWantPlay = Int32.Parse(txtTime.Text);
            }
            catch (Exception)
            {
                MainWindow.program.gameplay.HowLongWeWantPlay = 1;
            }
        }

        private void cbPlayer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbHero.Items.Clear();
            foreach (var item in MainWindow.program.heroes)
            {
                cbHero.Items.Add(item);
            }
            cbHero.SelectedItem = ((Player)cbPlayer.SelectedItem).PlayerHero;
            txtMoney.Text = ((Player)cbPlayer.SelectedItem).Money.ToString();
        }

        private void cbHero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbHero.SelectedIndex > -1)
                foreach (Player item in MainWindow.program.players)
                {
                    if (item == cbPlayer.SelectedItem)
                        item.PlayerHero = (Heroes)cbHero.SelectedItem;
                }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MainWindow.program.Close();
        }
    }
}
