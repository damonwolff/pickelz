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
    class Bullet
    {
        public Image bullet = new Image();
        public double PosX { get; set; }
        private double PosY { get; set; }
        public Canvas c;

        public void forward ()
        {
            PosY -= 5;
            Canvas.SetTop(bullet, PosY);
        }
        public Bullet( Canvas space)
        {
            space.Children.Add(bullet);
            c = space;
            bullet.Source = new BitmapImage(new Uri($"pack://application:,,,/Bullet.png")); // dont know how images work really 
           
        }
    }
}
