using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.VisualBasic;
using Biedapoly;

namespace BiedapolyGUI
{
    public class JailFieldScreenElement :FieldScreenElement
    {

        public JailFieldScreenElement(Canvas canvasfield,Territory logicalfield,Location orientation) :base(canvasfield,logicalfield,orientation)
        {
            Canvasfield = canvasfield;
            Canvasfield.Height = 95;
            Canvasfield.Width = 95;
            Logicalfield = logicalfield;
        }


        private void AddPlayerOnScreen()
        {
            Game game = MainWindow.GetMyGame();
            List<Player> listofplayeronfield = Methods.GetVisitors(Logicalfield.Coordinates,game);
            int numberofplayeroffield = 0;
            int iterationforprisoner = 0;
            Canvasfield.Height = 95;
            Canvasfield.Width = 95;


            foreach (Player player in listofplayeronfield)
            {
                Image myimage = new Image();
                myimage.VerticalAlignment = VerticalAlignment.Top;
                myimage.HorizontalAlignment = HorizontalAlignment.Left;
                myimage.Height = 30;
                myimage.Width = 30;

                BitmapImage bmpmyimage = new BitmapImage();
                Canvasfield.Children.Add(myimage);


                if (player.Id == 0)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
             

                }


                else if (player.Id == 1)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                
                }


                else if (player.Id == 2)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                }


                else if (player.Id == 3)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\kot.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                }

                else if (player.Id == 4)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                }

                myimage.Source= bmpmyimage;

                string mystring = "";

                if(!player.PlayerBonuses.TryGetValue("ARE",out mystring)) 
                {
                    if (numberofplayeroffield == 0)
                    {
                        Canvas.SetLeft(myimage, 60);
                        Canvas.SetTop(myimage, 60);
                    }

                    else if (numberofplayeroffield == 1)
                    {
                        Canvas.SetLeft(myimage, 30);
                        Canvas.SetTop(myimage, 60);
                    }

                    else if (numberofplayeroffield == 2)
                    {
                        Canvas.SetLeft(myimage, 35);
                        Canvas.SetTop(myimage, 95);
                    }

                    else if (numberofplayeroffield == 3)
                    {
                        Canvas.SetLeft(myimage, 5);
                        Canvas.SetTop(myimage, 95);
                    }

                    else if (numberofplayeroffield == 4)
                    {
                        Canvas.SetLeft(myimage, 5);
                        Canvas.SetTop(myimage, 65);
                    }

                    else if (numberofplayeroffield == 5)
                    {
                        Canvas.SetLeft(myimage, 5);
                        Canvas.SetTop(myimage, 35);
                    }
                }

                else 
                {
                    Canvas.SetLeft(myimage, 60 + iterationforprisoner);
                    Canvas.SetTop(myimage, 20 + iterationforprisoner);
                    iterationforprisoner += 10;
                }


                numberofplayeroffield++;
               

            }
             
            
            
        }


        private void ClearAllImages()
        {
            Canvasfield.Children.Clear();
        }

        public override void Refresh()
        {
            ClearAllImages();
            AddPlayerOnScreen();
        }




    }
}
