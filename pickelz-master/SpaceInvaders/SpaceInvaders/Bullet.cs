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
        public Image bullet; 
        public double PosX { get; set; }
        public double PosY { get; set; }
        public Canvas c;
        public List<Image> bullets = new List<Image>();
        

        public void forward ()
        {

            foreach(Image b in bullets) {
                //PosY += 5;
                double y = Canvas.GetBottom(b);
            Canvas.SetBottom(b, y+5);
                if (Canvas.GetTop(b) < 0) {
                    c.Children.Remove(b);
                }
            }
        }
        public Bullet( Canvas space, double x)
        {
            bullet = new Image();
            space.Children.Add(bullet);
            c = space;
            bullet.Source = new BitmapImage(new Uri($"pack://application:,,,/Bullet.png")); // dont know how images work really 
            bullet.Width = 10;
            bullet.Height = 15;

            Canvas.SetLeft(bullet,x);
            Canvas.SetBottom(bullet, 50);
            PosY = Canvas.GetBottom(bullet);
            bullets.Add(this.bullet);
        }
    }
}
