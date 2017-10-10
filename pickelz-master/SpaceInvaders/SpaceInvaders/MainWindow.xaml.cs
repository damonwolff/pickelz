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
using System.IO;
using Microsoft.VisualBasic;
using System.Media;

namespace SpaceInvaders {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    enum state { stopped, movingleft, movingright, shooting, GameOver}
    
    public partial class MainWindow : Window {
        public DispatcherTimer theTimer;
        state shipState;
        Ship player;
        Bullet b;
        int noBull = 0;// used to get a unique name for each bullet, to know which one to delete
        List<Bullet> bullets = new List<Bullet>();// thought having a list of bullets in the bullet class would be enough but this seems to work I'm just gonna leave it like this
        int firingInterval = 0; //so there's not a constant fire rate
        List<Alien> aliens = new List<Alien>();
        public DispatcherTimer AliensMove; 
        public int score = 0;
        int lvl = 1;
        
        public MainWindow() {
            InitializeComponent();

            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(5);
            theTimer.IsEnabled = true;
            theTimer.Tick += dispatcherTimer_Tick;
            player = new Ship(space);
            addAliens(lvl);

            AliensMove = new DispatcherTimer();
            AliensMove.Interval = TimeSpan.FromMilliseconds(1000);
            AliensMove.IsEnabled = true;
            AliensMove.Tick += AliensMove_Tick;

        }

        private void AliensMove_Tick(object sender, EventArgs e)
        {
            foreach(Alien a in aliens)
            {
                a.move();
            }
        }

        void addAliens(int lev) {
            double x = 120;
            double y = 15;
            for (int k = 0; k < lev; k++)
            {
                
                for (int i = 0; i < 10; i++)
                {
                    Alien a = new Alien(space, x, y, Convert.ToString(i));
                    aliens.Add(a);
                    x = x + 30;
                }
                y = y - 30;
                x = 120;
            }
        }

        public void checkCollisions() {
            if (aliens.Count == 0)
            {
                lvl++;
                addAliens(lvl);
                

            }
            foreach (Bullet b in bullets) {
                foreach (Alien a in aliens) {
                    if (b.PosX >= a.PosX && (b.PosX + b.width) <= (a.width + a.PosX) && b.PosY <= a.PosY + a.height) {
                        b.delete(b.identity);
                        a.delete(a.identity);
                        score += 100;
                        textBox.Text = $"{score}";
                        aliens.Remove(a);
                        bullets.Remove(b);
                        playSound("explode");
                        break;
                    } 
                }
                break;
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
                if (b.PosY <= 0) {
                    b.delete(b.identity);
                    bullets.Remove(b);
                    break;
                }
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
                        playSound("bullet");
                        noBull++;
                        bullets.Add(b);
                        firingInterval = 0;
                    }
                    break;
            }

        }
         
        void highscore()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("enter your name", "name", "");
            if (lvl == 11 || shipState == state.GameOver )
            {
                StreamReader r = File.OpenText("highscores");
                string sline = r.ReadLine();
                List<int> xs = new List<int>();
                List<string> ys = new List<string>();
                int k = 0;
                while (sline != null)
                {
                    string[] temp = sline.Split(' ');
                   ys[k] = temp[1];
                   xs[k] = Convert.ToInt32(temp[2]);
                   k++;
                }
                r.Close();
                int pos = -1;
                for(int i =0; i < xs.Count; i++)
                {
                    if (score > xs[i])
                    {
                        pos = i;
                        xs.Insert(i, score);
                        xs.Remove(xs[xs.Count]);
                        ys.Insert(i, name);
                        ys.Remove(ys[ys.Count]);
                        break;
                        
                    }
                 
                }
                int j = 0;
                StreamWriter w = File.CreateText("highscore");
                foreach(string s in ys)
                {
                    sline = ($"{ys.IndexOf(s)+1} {s} {xs[j]}");
                    j++;
                }

                
                // change image on background to be a congratulations
                
                
            }
            
        }

        void playSound(string x)
        {
            if (x == "bullet")
            {
                SoundPlayer bulletSound = new SoundPlayer(SpaceInvaders.Properties.Resources.little_robot_sound_factory_Laser_09);
                bulletSound.Play();
            }
            if (x == "explode")
            {
                SoundPlayer explodeSound = new SoundPlayer(SpaceInvaders.Properties.Resources.leisure_video_game_retro_8bit_explosion_or_gun_001);
                explodeSound.Play();
            }
        }

        private void Windows_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left: shipState = state.movingleft; break;
                case Key.Right: shipState = state.movingright; break;
                case Key.Space: shipState = state.shooting; break;
                case Key.P:
                    theTimer.IsEnabled = !theTimer.IsEnabled;
                    AliensMove.IsEnabled = !AliensMove.IsEnabled;
                    break;
                case Key.Escape: this.Close();break;
            }
        }

        private void Windows_KeyUp(object sender, KeyEventArgs e) {
            shipState = state.stopped;
        }
    }
}
