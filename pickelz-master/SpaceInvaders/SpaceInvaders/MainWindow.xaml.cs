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
    enum state { stopped, movingleft, movingright, shooting, GameOver }

    public partial class MainWindow : Window {
        public DispatcherTimer theTimer;
        state shipState;
        Ship player;
        Bullet b;
        int noBull = 0;// used to get a unique name for each bullet, to know which one to delete
        List<Bullet> bullets = new List<Bullet>();// thought having a list of bullets in the bullet class would be enough but this seems to work I'm just gonna leave it like this
        List<Bullet> alienBullets = new List<Bullet>();
        int firingInterval = 0; //so there's not a constant fire rate
        int alienFiringInterval = 0;
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

        private void AliensMove_Tick(object sender, EventArgs e) {
            foreach (Alien a in aliens) {
                a.move();
            }
        }

        void addAliens(int lev) {
            double x = 120;
            double y = 15;
            for (int k = 0; k < lev; k++) {

                for (int i = 0; i < 10; i++) {
                    Alien a = new Alien(space, x, y, Convert.ToString(i));
                    aliens.Add(a);
                    x = x + 30;
                }
                y = y - 30;
                x = 120;
            }
        }

        public void checkCollisions() {
            if (aliens.Count == 0) {
                lvl++;

                if (lvl < 11) {
                    addAliens(lvl);
                }



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

            foreach (Bullet b in alienBullets) {
                /* so basically it should be:
                left of bullet >= left of ship &&
                left of bullet + it's width <= left of ship + it's width &&
                top of bullet + height >= top of ship &&
                top of bullet + height <= top of ship + it's height 
                 */
                double leftBull = b.PosX;
                double rightBull = b.PosX + b.width;
                double leftofShip = player.PosX;
                double rightOfShip = player.PosX + 65;
                double shipTopfromTop = space.ActualHeight - player.PosY - 30;
                double shipBotfromTop = space.ActualHeight - player.PosY;
                double TopofBulfromTop = b.YfromTop;
                double botofBulFromTop = b.YfromTop + 15;
                //if (b.PosX >= player.actualX - 27.5 && (b.PosX + b.width) <= player.actualX && b.PosY >= (space.ActualHeight - player.PosY) && b.PosY <= (space.ActualHeight - player.PosY + 30)) {
                if (leftBull >= leftofShip && rightBull <= rightOfShip && TopofBulfromTop>= shipTopfromTop && botofBulFromTop <= shipBotfromTop) {
                    b.delete(b.identity);
                    bullets.Remove(b);
                    
                        theTimer.IsEnabled = !theTimer.IsEnabled;
                        AliensMove.IsEnabled = !AliensMove.IsEnabled;
                        shipState = state.GameOver;
                        MessageBox.Show("GAME OVER");
                        highscore();
                        StartScreen open = new StartScreen(); open.Show(); this.Close();

                        break;
                  
                }

            }

        }


        public void dispatcherTimer_Tick(object sender, EventArgs e) {
            firingInterval++;
            updateShip();
            updateBullets();
            checkCollisions();
            alienFiringInterval++;
            fireAliens();
            updateAlienBullets();

        }

        void updateBullets() {
            foreach (Bullet b in bullets) {
                b.forward(false);
                if (b.PosY <= 0) {
                    b.delete(b.identity);
                    bullets.Remove(b);
                    break;
                }
            }
        }

        void updateAlienBullets() {
            foreach (Bullet b in alienBullets) {
                b.forward(true);
                if (b.PosY <= space.ActualHeight) {
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
                    if (firingInterval > 15) {
                        b = new Bullet(space, player.actualX, 50, Convert.ToString(noBull), true);
                        noBull++;
                        bullets.Add(b);
                        firingInterval = 0;
                        playSound("bullet");
                    }
                    break;
            }

        }

        void fireAliens() {
            if (alienFiringInterval > 50) {
                Random r = new Random();
                int i = r.Next(0, aliens.Count);
                b = new Bullet(space, aliens[i].PosX, aliens[i].PosY, Convert.ToString(noBull), false);
                b.bullet.Source = new BitmapImage(new Uri($"pack://application:,,,/Bullet2.png"));
                noBull++;
                alienBullets.Add(b);
                alienFiringInterval = 0;
            }
        }

  
        void highscore()
        {
            string name = Microsoft.VisualBasic.Interaction.InputBox("enter your name", "name", "");
            int pos = -1;
            if (lvl == 11 || shipState == state.GameOver)
            {
                if (File.Exists("highscores"))
                {
                    StreamReader r = File.OpenText("highscores");
                    string sline = r.ReadLine();
                    List<int> xs = new List<int>();
                    List<string> ys = new List<string>();
                    List<int> newxs = new List<int>();
                    List<string> newys = new List<string>();
                    while (sline != null)
                    {
                        string[] temp = sline.Split(' ');
                        ys.Add(temp[1]);
                        xs.Add(Convert.ToInt32(temp[2]));
                        sline = r.ReadLine();
                    }
                    r.Close();
                   
                    for (int i = 0; i < xs.Count; i++)
                    {
                        if (score > xs[i])
                        {
                            pos = i;

                        }
                        for(int k=0; k<xs.Count; k++)
                        {
                            if (k == pos)
                            {
                                newys.Add(name);
                                newxs.Add(score);
                                break;
                            }
                            newxs.Add(xs[k]);
                            newys.Add(ys[k]);
                        }
                        if (pos < xs.Count)
                        {
                            for (int k = pos; k < xs.Count; k++)
                            {
                                newxs.Add(xs[k]);
                                newys.Add(ys[k]);
                            }
                        }

                        StreamWriter w = File.CreateText("highscores");
                        for (int q = 0; q < newxs.Count; q++)
                        {

                                sline = ($"{q + 1} {newys[q]} {newxs[q]}");
                                w.WriteLine(sline);
                        }
                        w.Close();
                    }
                }
                else
                {
                    StreamWriter k = File.CreateText("highscores");
                    k.WriteLine($"1 {name} {score}");
                    k.Close();
                }
                
            }

        }


        void playSound(string x) {
            if (x == "bullet") {
                SoundPlayer bulletSound = new SoundPlayer(SpaceInvaders.Properties.Resources.little_robot_sound_factory_Laser_09);
                bulletSound.Play();
            }
            if (x == "explode") {
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
                case Key.Escape: StartScreen open = new StartScreen(); open.Show(); this.Close(); break;
            }
        }

        private void Windows_KeyUp(object sender, KeyEventArgs e) {
            shipState = state.stopped;
        }
    }
}
