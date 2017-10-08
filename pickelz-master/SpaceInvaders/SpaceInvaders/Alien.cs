using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows;

namespace SpaceInvaders {
    class Alien {
        public Image alien;
        public double PosX { get; set; }
        public double PosY { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public string identity { get; set; }
        public Canvas c;

        public Alien(Canvas space, double x , double y, string i) {
            alien = new Image();
            alien.Name ="n"+i;
            identity ="n"+ i;
            space.Children.Add(alien);
            alien.Source = new BitmapImage(new Uri($"pack://application:,,,/alien.png"));
            alien.Width = 25;
            alien.Height = 15;

            
            c = space;

            Canvas.SetLeft(alien, x);
            Canvas.SetTop(alien, y);
            PosX = Canvas.GetLeft(alien);
            PosY = Canvas.GetTop(alien);
            height = alien.ActualHeight;
           width = 25;
        }

        public void delete(string a) {
            var thing = (UIElement)LogicalTreeHelper.FindLogicalNode(c, a);
            c.Children.Remove(thing);
        }
    }
}
