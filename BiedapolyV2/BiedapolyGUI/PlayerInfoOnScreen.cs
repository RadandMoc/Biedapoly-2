using Biedapoly;
using BiedapolyGUI;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BiedapolyGUI
{
    public class PlayerInfoOnScreen
    {
        Canvas infocanvas;
        Player logicalPlayer;
        int number;
        

        public PlayerInfoOnScreen(Canvas infocanvas, int number)
        {
            Game myGame = MainWindow.GetMyGame();
            Infocanvas = infocanvas;
            this.number = number;
            if (myGame.Players.Contains(myGame.Players.ToList()[number]))
            {
                logicalPlayer = myGame.Players.ToList()[number];
            }
        }

        public Canvas Infocanvas { get => infocanvas; set => infocanvas = value; }
        public Player LogicalPlayer { get => logicalPlayer; set => logicalPlayer = value; }

        
        public void RefreshPlayerInfoOnScreen() //Dynamic data
        {
            
            ((Label)infocanvas.Children[3]).Content = MainWindow.GetMyGame().Map.DictionaryOfTerritories[LogicalPlayer.Coordinates];
            ((Label)infocanvas.Children[4]).Content = "Cash\n\n" + logicalPlayer.Money;

        }

        public void RefreshProperty() 
        {
            ((ListBox)infocanvas.Children[5]).Items.Clear();

           
            foreach(Territory item in MainWindow.GetMyGame().Map.Alliance.Keys) 
            {
                if(item.Owner== logicalPlayer) 
                {
                    ((ListBox)infocanvas.Children[5]).Items.Add(item.Name);
                }
            }

        }






    }
}
