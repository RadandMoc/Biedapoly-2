using Biedapoly;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BiedapolyGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {

        private static MainWindow MyWindow=null;

        Game myGame;
        InfoPanel myinfopanel;
        BoardPanel myboard;
        public MainWindow()
        {
            InitializeComponent();
            Heroes counter = new Heroes("Counter", 15000, new Dictionary<string, string>());
            Map studentpoly = Methods.MakeStudentpolyMap();
            PrePlayer prePlayer = new PrePlayer("Kot");

            Player gracz = new Player(15000, counter, Biedapoly.Color.white, "Pies", "000");
            Player gracz2 = new Player(15000, counter, Biedapoly.Color.blue, "Kot", "012");
            Player gracz3 = new Player(15000, counter, Biedapoly.Color.blue, "Słoń", "032");
            Queue<Player> queue = new Queue<Player>();
            queue.Enqueue(gracz);
            queue.Enqueue(gracz2);
            queue.Enqueue(gracz3);      
            myGame = new Game(studentpoly,queue, 31536000);


            MyWindow = this;


            /*
            Canvas nowy = new Canvas();
            GameStatus newgamestatus = GameStatus.Instance;
            StreetFieldScreenElement newfield = new StreetFieldScreenElement(nowy, newgamestatus.BoardFields[1],Location.East);
            nowy.Margin = new Thickness(765, 765, 0, 0);
            newfield.SetMargin(765, 765, 0, 0);
            nowy.HorizontalAlignment = HorizontalAlignment.Left;
            nowy.VerticalAlignment = VerticalAlignment.Top;
            newfield.Refresh();
            my_grid.Children.Add(nowy);
            */




            PlayerInfoOnScreen player1 = new PlayerInfoOnScreen(CanvasPlayerone, 0);
            ActualPosition.Content = "Current Position";
            Pocket.Content = "Cash\n\n" + player1.LogicalPlayer.Money;
            PlayerName.Content = player1.LogicalPlayer.Name;


            PlayerInfoOnScreen player2 = new PlayerInfoOnScreen(CanvasPlayertwo, 1);
            ActualPosition2.Content = "Current Position";
            Pocket2.Content = "Cash\n\n" + player2.LogicalPlayer.Money;
            Player2Name.Content = player2.LogicalPlayer.Name;

            
            PlayerInfoOnScreen player3 = new PlayerInfoOnScreen(CanvasPlayerthree, 2);
            ActualPosition3.Content = "Current Position";
            Pocket3.Content = "Cash\n\n" + player3.LogicalPlayer.Money;
            Player3Name.Content = player3.LogicalPlayer.Name;

            /*
            PlayerInfoOnScreen player4 = new PlayerInfoOnScreen(CanvasPlayer4, 3);
            ActualPosition4.Content = "Current Position";
            Pocket4.Content = "Cash\n\n" + player4.LogicalPlayer.Money;
            Player4Name.Content = player4.LogicalPlayer.Name;

            */



            myinfopanel = new InfoPanel(GUIInfoPanel);

            myinfopanel.AddPlayerInfo(player1);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["001"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["003"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["005"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["007"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["009"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["011"].Name);
            Player1propertyfield.Items.Add(myGame.Map.DictionaryOfTerritories["013"].Name);
            myinfopanel.AddPlayerInfo(player2);  
            //myinfopanel.AddPlayerInfo(player3);.Map.
            //myinfopanel.AddPlayerInfo(player4);

            myboard = new BoardPanel();

            foreach (RefreshField field in myboard.Fields)
            {
                my_grid.Children.Add(field.getCanvas());
            }


            myboard.Fields[0].SetMargin(775, 775, 0, 0);

            int a = 700;
            for (int i = 1; i <= 10; i++)
            {
                myboard.Fields[i].SetMargin(a, 775, 0, 0);
                a -= 75;
            }

            myboard.Fields[10].SetMargin(0, 775, 0, 0);

            int b = 700;
            for (int i = 11; i < 20; i++)
            {
                myboard.Fields[i].SetMargin(0, b, 0, 0);
                b -= 75;
            }

            myboard.Fields[20].SetMargin(0, 0, 0, 0);

            int c = 95;
            for (int i = 21; i < 30; i++)
            {
                myboard.Fields[i].SetMargin(c, 0, 0, 0);
                c += 75;
            }


            int d = 100;
            for (int i = 31; i < 40; i++)
            {
                myboard.Fields[i].SetMargin(775, d, 0, 0);
                d += 75;
            }


            /*
            for (int i = 0; i < 33; i++)
            {

                GameStatus.Instance.Players[1].Coordinates = i;
                myboard.GroupedRefresh();
               
            }
            */


       

            myGame.Players.Peek().Coordinates = "000";
            //GameStatus.Instance.Players[2].Coordinates = 12;
            //GameStatus.Instance.BuildHouse(1);


            //GameStatus.SaveToFile("NewGame", GameStatus.Instance);


            //GameStatus.Instance=GameStatus.LoadfromFile("mojstatusgry");

            myboard.GroupedRefresh();
            myinfopanel.UpdatePlayerInfo();

            //actionPanel = new ActionPanel(ListBoxWithAction);


            /*
            for (int i = 0; i < 5; i++)
            {
                Button button = new Button();
                DynamicButton mybutton = new DynamicButton(@"C:\Users\zapar\source\repos\NewRepo\Image\House.png");
                mybutton.CreateProopertyToButton("To jest moja akcja");
                actionPanel.AddItemToListBox(mybutton);
            }
            actionPanel.SetMarginForButton();
            Label actionlabeL = new Label();
            actionlabeL.Content = $"Wszyskie możliwe akcje do wykonania dla gracza {GameStatus.Instance.GetWhoplay().Name}";
            actionlabeL.FontSize = 20;
            actionPanel.Actionlistbox.Items.Insert(0, actionlabeL);
            */
            //actionPanel.RemoveItemFromListBox();

            //UiElementGUI.GenerateAnimation(@"C:\Users\zapar\source\repos\NewRepo\Image\House.png", my_grid, @"C:\Users\zapar\source\repos\NewRepo\Image\Hotel.png", actionPanel);

            //CurrentPosition.Content = GameStatus.Instance.GetField(player1.LogicalPlayer.Coordinates);
            //CurrentPosition2.Content = GameStatus.Instance.GetField(player2.LogicalPlayer.Coordinates);
            //CurrentPosition3.Content = GameStatus.Instance.GetField(player3.LogicalPlayer.Coordinates);
            //myMainWindow = this;




        }

        


        




        static public Game GetMyGame() 
        {
            return MyWindow.myGame;
        }

        private void Wyjście_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.Close();
        }

        private void Dice_Roll_Click(object sender, RoutedEventArgs e)
        {
           
            Game game = MainWindow.GetMyGame();
            game.ThrowingDices();

            this.Dispatcher.Invoke(new Action(() => { ShowImage(game.FirstThrow, game.SecondThrow); }));

            //await Task.Run(() => movePlayer());

            Methods.PlayerRandomMove(MainWindow.GetMyGame());
            myboard.GroupedRefresh();
            Dice_Roll.IsEnabled = true;
            this.Dispatcher.Invoke(new Action(() => myinfopanel.UpdatePlayerInfo()));

        }



        private void movePlayer()
        {
            
            Game mygame = MainWindow.GetMyGame();
            List<string> kot = new List<string>{ "000", "001", "002","003" };
            for (int i = 0; i < MainWindow.GetMyGame().HowMuchMove; i++)
            {
                myGame.Players.Peek().Coordinates = kot[i%3];

                this.Dispatcher.Invoke(new Action(() => { myboard.GroupedRefresh(); }));

                Thread.Sleep(1000);
            }

        }


        public void ShowImage(int firstthrow, int secondthrow)
        {
            BitmapImage bitmap = new BitmapImage();
            BitmapImage bitmap2 = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(@$"C:\Users\zapar\source\repos\NewRepo\DiceThrow{firstthrow}.png", UriKind.RelativeOrAbsolute);
            bitmap.EndInit();

            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(@$"C:\Users\zapar\source\repos\NewRepo\DiceThrow{secondthrow}.png", UriKind.RelativeOrAbsolute);
            bitmap2.EndInit();

            firstDice.Source = bitmap;
            secondDice.Source = bitmap2;

        }
    }
}
