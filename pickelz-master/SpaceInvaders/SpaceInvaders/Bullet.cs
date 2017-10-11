using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Windows.Controls;

namespace SpaceInvaders {
    class Bullet {
        public Image bullet;
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double YfromTop { get; set; }
        public double width { get; set; }
        public string identity { get; set; }
        public Canvas c;
        public List<Image> bullets = new List<Image>();
        public List<Image> alienBullets = new List<Image>();


        public void forward(bool isAlien) {
            if (isAlien == false) {
                foreach (Image b in bullets) {
                    double y = Canvas.GetBottom(b);
                    Canvas.SetBottom(b, y + 5);
                    if (Canvas.GetRight(b) > c.ActualHeight) {
                        c.Children.Remove(b);
                    }
                    PosY = c.ActualHeight - Canvas.GetBottom(b);
                }
            } else {
                foreach (Image b in alienBullets) {
                    double y = Canvas.GetTop(b);
                    Canvas.SetTop(b, y + 3);
                    if (Canvas.GetTop(b) < 0) {
                        c.Children.Remove(b);
                    }
                    PosY = c.ActualHeight + Canvas.GetTop(b);
                    YfromTop = Canvas.GetTop(b);
                }
            }
        }
        public Bullet(Canvas space, double x, double y, string i, bool isShip) {
            bullet = new Image();
            bullet.Name = "b" + i;
            identity = "b" + i;
            space.Children.Add(bullet);
            c = space;
            bullet.Source = new BitmapImage(new Uri($"pack://application:,,,/Bullet.png")); // dont know how images work really 
            bullet.Width = 10;
            bullet.Height = 15;

            if (isShip == true) {
                Canvas.SetBottom(bullet, y);
                PosY = c.ActualHeight - Canvas.GetBottom(bullet);
                bullets.Add(this.bullet);
            } else {
                Canvas.SetTop(bullet, y);
                PosY = c.ActualHeight - Canvas.GetTop(bullet);
                alienBullets.Add(this.bullet);
            }

            Canvas.SetLeft(bullet, x);
            PosX = Canvas.GetLeft(bullet);
            width = 10;
        }

        public void delete(string b) {
            var thing = (UIElement)LogicalTreeHelper.FindLogicalNode(c, b);
            c.Children.Remove(thing);
        }
    }
}
