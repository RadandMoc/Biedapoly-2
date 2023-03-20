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
using Biedapoly;
using Microsoft.VisualBasic;




namespace BiedapolyGUI
{
    public enum Location
    {
        South,
        West,
        North,
        East
    }
    public class FieldScreenElement : RefreshField
    {
        Canvas canvasfield;
        Territory logicalfield;
        Location orientation;

        public FieldScreenElement(Canvas canvasfield, Territory logicalfield, Location orientation)
        {
            Canvasfield = canvasfield;
            Logicalfield = logicalfield;
            Orientation = orientation;
            SettingsPropertyToCanvas();
        }

        public Canvas Canvasfield { get => canvasfield; set => canvasfield = value; }
        public Territory Logicalfield { get => logicalfield; set => logicalfield = value; }
        public Location Orientation { get => orientation; set => orientation = value; }

        public Canvas getCanvas() { return canvasfield; }

        private void SettingsPropertyToCanvas()
        {
            Canvasfield.HorizontalAlignment = HorizontalAlignment.Left;
            Canvasfield.VerticalAlignment = VerticalAlignment.Top;
            if (Orientation == Location.North || Orientation == Location.South)
            {
                Canvasfield.Height = 97;
                Canvasfield.Width = 75;

            }

            else if (Orientation == Location.East || Orientation == Location.West)
            {
                Canvasfield.Width = 97;
                Canvasfield.Height = 75;
            }
            
        }


        private void AddPlayerOnScreen()
        {
            Game game = MainWindow.GetMyGame();
            List<Player> listofplayeronfield = Methods.GetVisitors(Logicalfield.Coordinates,game);
            int iteration = 0;
            ScaleTransform scaletransform = new ScaleTransform();
            scaletransform.ScaleX = -1;

            foreach (Player player in listofplayeronfield)
            {
                Image myimage = new Image();

                BitmapImage bmpmyimage = new BitmapImage();
                Canvasfield.Children.Add(myimage);


                if (player.Id == 0)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                    ImageAdjust(Orientation, myimage, bmpmyimage, iteration, scaletransform);

                }


                else if (player.Id == 1)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                    ImageAdjust(Orientation, myimage, bmpmyimage, iteration, scaletransform);
                }


                else if (player.Id == 2)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                    ImageAdjust(Orientation, myimage, bmpmyimage, iteration, scaletransform);
                }


                else if (player.Id ==3)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\kot.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                    ImageAdjust(Orientation, myimage, bmpmyimage, iteration, scaletransform);
                }

                else if (player.Id == 4)
                {
                    bmpmyimage.BeginInit();
                    bmpmyimage.UriSource = new Uri(@"C:\Users\zapar\source\repos\NewRepo\Słoń.png", UriKind.RelativeOrAbsolute);
                    bmpmyimage.EndInit();
                    ImageAdjust(Orientation, myimage, bmpmyimage, iteration, scaletransform);
                }




                iteration++;

            }
        }

        void ImageAdjust(Location imagelocation, Image myimage, BitmapImage bmpmyimage, int iteration, ScaleTransform scaletransform)
        {
            myimage.Height = 30;
            myimage.Width = 30;
            if (Orientation == Location.South)
            {
                TransformImageSouth(myimage, bmpmyimage, iteration);
            }
            else if (Orientation == Location.West)
            {
                TranformImageWest(myimage, bmpmyimage, scaletransform, iteration);
            }
            else if (Orientation == Location.North)
            {
                TransformImageNorth(myimage, bmpmyimage, scaletransform, iteration);

            }
            else if (Orientation == Location.East)
            {
                TranformImageEast(myimage, bmpmyimage, scaletransform, iteration);
            }
        }



        void TransformImageSouth(Image image_to_transform, BitmapImage bitmapImage, int playerplacement)
        {
            image_to_transform.Source = new BitmapImage(bitmapImage.UriSource);
            if (playerplacement == 0)
            {
                Canvas.SetLeft(image_to_transform, 8);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 1)
            {
                Canvas.SetLeft(image_to_transform, 28);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 2)
            {
                Canvas.SetLeft(image_to_transform, 28);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 3)
            {
                Canvas.SetLeft(image_to_transform, 48);
                Canvas.SetTop(image_to_transform, 52);
            }

            else if (playerplacement == 4)
            {
                Canvas.SetLeft(image_to_transform, 28);
                Canvas.SetTop(image_to_transform, 52);
            }

            else if (playerplacement == 5)
            {
                Canvas.SetLeft(image_to_transform, 48);
                Canvas.SetTop(image_to_transform, 52);
            }

        }


        void TranformImageWest(Image image_to_transform, BitmapImage bitmapImage, ScaleTransform scaletransform, int playerplacement)
        {
            TransformedBitmap transformBmp = new TransformedBitmap();

            transformBmp.BeginInit();

            transformBmp.Source = bitmapImage;

            RotateTransform transform = new RotateTransform(90);

            transformBmp.Transform = transform;

            transformBmp.EndInit();

            image_to_transform.Source = transformBmp;

            image_to_transform.RenderTransform = scaletransform;


            if (playerplacement == 0)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 8);
            }

            else if (playerplacement == 1)
            {
                Canvas.SetLeft(image_to_transform, 22);
                Canvas.SetTop(image_to_transform, 8);
            }

            else if (playerplacement == 2)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 28);
            }

            else if (playerplacement == 3)
            {
                Canvas.SetLeft(image_to_transform, 22);
                Canvas.SetTop(image_to_transform, 28);
            }

            else if (playerplacement == 4)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 48);
            }

            else if (playerplacement == 5)
            {
                Canvas.SetLeft(image_to_transform, 22);
                Canvas.SetTop(image_to_transform, 48);
            }

        }

        void TransformImageNorth(Image image_to_transform, BitmapImage bitmapImage, ScaleTransform scaletranform, int playerplacement)
        {
            image_to_transform.Source = bitmapImage;
            image_to_transform.RenderTransform = scaletranform;



            if (playerplacement == 0)
            {
                Canvas.SetLeft(image_to_transform, 48);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 1)
            {
                Canvas.SetLeft(image_to_transform, 48);
                Canvas.SetTop(image_to_transform, 52);
            }

            else if (playerplacement == 2)
            {
                Canvas.SetLeft(image_to_transform, 28);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 3)
            {
                Canvas.SetLeft(image_to_transform, 28);
                Canvas.SetTop(image_to_transform, 52);
            }

            else if (playerplacement == 4)
            {
                Canvas.SetLeft(image_to_transform, 8);
                Canvas.SetTop(image_to_transform, 32);
            }

            else if (playerplacement == 5)
            {
                Canvas.SetLeft(image_to_transform, 8);
                Canvas.SetTop(image_to_transform, 52);
            }

        }

        void TranformImageEast(Image image_to_transform, BitmapImage bitmapImage, ScaleTransform scaletransform, int playerplacement)
        {
            TransformedBitmap transformBmp = new TransformedBitmap();

            transformBmp.BeginInit();

            transformBmp.Source = bitmapImage;

            RotateTransform transform = new RotateTransform(270);

            transformBmp.Transform = transform;

            transformBmp.EndInit();

            image_to_transform.Source = transformBmp;

            image_to_transform.RenderTransform = scaletransform;


            if (playerplacement == 0)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 48);
            }

            else if (playerplacement == 1)
            {
                Canvas.SetLeft(image_to_transform, 62);
                Canvas.SetTop(image_to_transform, 48);
            }

            else if (playerplacement == 2)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 28);
            }

            else if (playerplacement == 3)
            {
                Canvas.SetLeft(image_to_transform, 62);
                Canvas.SetTop(image_to_transform, 28);
            }

            else if (playerplacement == 4)
            {
                Canvas.SetLeft(image_to_transform, 42);
                Canvas.SetTop(image_to_transform, 8);
            }

            else if (playerplacement == 5)
            {
                Canvas.SetLeft(image_to_transform, 62);
                Canvas.SetTop(image_to_transform, 8);
            }

        }

        private void ClearAllImages()
        {
            Canvasfield.Children.Clear();
        }

        public virtual void Refresh()
        {
            ClearAllImages();
            AddPlayerOnScreen();
        }

        public void SetMargin(double left, double right, double top, double down)
        {
            Canvasfield.Margin = new Thickness(left, right, top, down);
        }
    }
}
