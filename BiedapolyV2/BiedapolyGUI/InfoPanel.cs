using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;


namespace BiedapolyGUI
{
    public class InfoPanel
    {
        Canvas maininfocanvas;
        List<PlayerInfoOnScreen> playerinfo=new List<PlayerInfoOnScreen>();
        public InfoPanel(Canvas maininfocanvas)
        {
            Maininfocanvas = maininfocanvas;
        }

        public Canvas Maininfocanvas { get => maininfocanvas; set => maininfocanvas = value; }
        public List<PlayerInfoOnScreen> Playerinfo { get => playerinfo; set => playerinfo = value; }

        public void AddPlayerInfo(PlayerInfoOnScreen playerinfotoadd) 
        {
            Playerinfo.Add(playerinfotoadd);         
        }

        public void UpdatePlayerInfo() 
        {
            foreach(PlayerInfoOnScreen playerinfo in Playerinfo) 
            {
                playerinfo.RefreshPlayerInfoOnScreen();
            }
        }

        public void UpdatePlayerNameIfNewGameStart() 
        {
            int i = 0;
            foreach(PlayerInfoOnScreen playerInfo in Playerinfo) 
            {

                ((Label)playerInfo.Infocanvas.Children[0]).Content = MainWindow.GetMyGame().Players.ToList()[i].Name;
                i++;
            }
        }

    }
}
