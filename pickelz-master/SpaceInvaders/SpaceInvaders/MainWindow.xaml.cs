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
    enum state {stopped, movingleft, movingright, shooting}

    public partial class MainWindow : Window {
        public DispatcherTimer theTimer;
        //double shipSpeed = 5;
        state shipState;
        Ship player;
        Bullet b;

        public MainWindow() {
            InitializeComponent();

            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(5);
            theTimer.IsEnabled = true;
            theTimer.Tick += dispatcherTimer_Tick;
            player = new Ship(space);
        }

        public void dispatcherTimer_Tick(object sender, EventArgs e) {
            updateShip();
                  
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
                    b = new Bullet(space);
                    b.PosX = player.PosX;
                    b.forward();
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
