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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace FinalVersion
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Game gameplay = new Game();
        public static MainWindow program;
        public Queue<Heroes> heroes = new Queue<Heroes>();
        public Dictionary<int,PrePlayer> prePlayers = new Dictionary<int, PrePlayer>();
        public Queue<Map> possibleMaps = new Queue<Map>();
        public Queue<Player> players= new Queue<Player>();

        static MainWindow()
        {
            program = new MainWindow(0);
        }

        public MainWindow()
        {
            InitializeComponent();
            Close();
        }


        public MainWindow(int i)
        {
            #region setHeroes
            Heroes counter = new Heroes("Counter", 100, new Dictionary<string, string>());
            Dictionary<string, string> architectBonuses = new Dictionary<string, string>();
            architectBonuses.Add("BHA", "P00095D10");
            architectBonuses.Add("BHO", "P00095D10");
            architectBonuses.Add("RHA", "P00095D10");
            architectBonuses.Add("RHO", "P00095D10");
            Heroes architect = new Heroes("Architect", 100, architectBonuses);
            Dictionary<string, string> hauseDeveloperBonuses = new Dictionary<string, string>();
            hauseDeveloperBonuses.Add("BHA", "P00090D10");
            hauseDeveloperBonuses.Add("BHO", "P00110D10");
            hauseDeveloperBonuses.Add("RHA", "P00090D10");
            hauseDeveloperBonuses.Add("RHO", "P00110D10");
            Heroes hauseDeveloper = new Heroes("HauseDeveloper", 100, hauseDeveloperBonuses);
            Dictionary<string, string> hotelDeveloperBonuses = new Dictionary<string, string>();
            hotelDeveloperBonuses.Add("BHA", "P00090D10");
            hotelDeveloperBonuses.Add("BHO", "P00110D10");
            hotelDeveloperBonuses.Add("RHA", "P00090D10");
            hotelDeveloperBonuses.Add("RHO", "P00110D10");
            Heroes hotelDeveloper = new Heroes("HotelDeveloper", 100, hotelDeveloperBonuses);

            heroes.Enqueue(counter);
            heroes.Enqueue(architect);
            heroes.Enqueue(hauseDeveloper);
            heroes.Enqueue(hotelDeveloper);
            #endregion setHeroes

            possibleMaps.Enqueue(Methods.MakeStudentpolyMap());
            //possibleMaps.Enqueue(Methods.MakeStudentpolyMap());
            gameplay.HowLongWeWantPlay = 31536000;
            InitializeComponent();
            Menu menu = new Menu();
            menu.Show();

            Hide();

        }

    }
}
