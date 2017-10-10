using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace SpaceInvaders
{
    class Ship
    {
        public Image ship = new Image();
        public double PosX { get; set; }
        private double PosY { get; set; }
        public int VelX { get; set; }
        public double actualX;

        public Canvas c;
        public void LeftRight(state s)
        {
            double nextX = Canvas.GetLeft(ship);
            if (s == state.movingleft)
            {

                if (PosX - VelX > 0)
                {
                    PosX -= VelX;
                    Canvas.SetLeft(ship, PosX);
                }
            } else if (s == state.movingright)
            {
                
                if (PosX + VelX < c.ActualWidth - ship.ActualWidth)
                {
                    PosX += VelX;
                    Canvas.SetLeft(ship, PosX - ship.ActualWidth / 2);
                }
                    
            }
            actualX = Canvas.GetLeft(ship)+27.5;// + (ship.ActualWidth/2);
        }
        public Ship(Canvas space)
        {
            space.Children.Add(ship);
            ship.Source = new BitmapImage(new Uri($"pack://application:,,,/ship.png"));
            ship.Height = 30;
            ship.Width = 65;
            VelX = 5;
            c = space;
            Canvas.SetLeft(ship, 250);
            Canvas.SetBottom(ship, 30);
            PosX = 250;


        }
    }
}
