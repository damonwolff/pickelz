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
using System.Windows.Threading;

namespace SpaceInvaders {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum state { stopped, movingleft, movingright, shooting }

    public partial class MainWindow : Window {
        public DispatcherTimer theTimer;
        //double shipSpeed = 5;
        state shipState;
        Ship player;
        Bullet b;
        int noBull = 0;
        List<Bullet> bullets = new List<Bullet>();// thought having a list of bullets in the bullet class would be enough but this seems to work I'm just gonna leave it like this
        int firingInterval = 0; //so there's not a constant fire rate
        public int level = 1;
        List<Alien> aliens = new List<Alien>();

        public MainWindow() {
            InitializeComponent();

            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(5);
            theTimer.IsEnabled = true;
            theTimer.Tick += dispatcherTimer_Tick;
            player = new Ship(space);
            addAliens(level);
            
        }

        void addAliens(int lev) {
            double x =50;
            double y =15;
            for (int i=0; i < lev * 15; i++) {
                Alien a = new Alien(space,x,y,Convert.ToString(i));
                aliens.Add(a);
                x = x + 30;
            }
        }

        public void checkCollisions() {
            foreach(Bullet b in bullets) {
                foreach(Alien a in aliens) {
                    if (b.PosX >= a.PosX) {// && b.PosY<=a.PosY+a.height) {
                        
                        b.delete(b.identity);
                        a.delete(a.identity);
                        //bullets.Remove(b);
                        //aliens.Remove(a);
                        
                    }
                }
            }
        }

        public void dispatcherTimer_Tick(object sender, EventArgs e) {
            firingInterval++;
            updateShip();
            updateBullets();
            checkCollisions();
        }

        void updateBullets() {
            foreach (Bullet b in bullets) {
                b.forward();
            }
        }

        void updateShip() {
            switch (shipState) {
                case state.stopped: break;
                case state.movingleft:
                    player.LeftRight(shipState);
                    break;
                case state.movingright:
                    player.LeftRight(shipState);
                    break;
                case state.shooting:
                    if (firingInterval > 25) {
                        b = new Bullet(space, player.actualX, Convert.ToString(noBull));
                        noBull++;
                        bullets.Add(b);
                        firingInterval = 0;
                    }
                    break;
            }

        }


        private void Windows_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left: shipState = state.movingleft; break;
                case Key.Right: shipState = state.movingright; break;
                case Key.Space: shipState = state.shooting; break;
            }
        }

        private void Windows_KeyUp(object sender, KeyEventArgs e) {
            shipState = state.stopped;
        }
    }
}
