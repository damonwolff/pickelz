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
    public partial class MainWindow : Window {
        public DispatcherTimer theTimer;
        string shipState = "stopped";
        double shipSpeed = 5;

        public MainWindow() {
            InitializeComponent();

            theTimer = new DispatcherTimer();
            theTimer.Interval = TimeSpan.FromMilliseconds(5);
            theTimer.IsEnabled = true;
            theTimer.Tick += dispatcherTimer_Tick;
        }

        public void dispatcherTimer_Tick(object sender, EventArgs e) {
            updateShip();
        }

        void updateShip() {
            double nextX = Canvas.GetLeft(ship);
            switch (shipState) {
                case "stopped": break;
                case "movingLeft":
                    double nextX1 = Canvas.GetLeft(ship) - shipSpeed;
                    if (nextX1 > 0) Canvas.SetLeft(ship, nextX1);
                    break;
                case "movingRight":
                    double nextX2 = Canvas.GetLeft(ship) + shipSpeed;
                    if (nextX2 < space.ActualWidth-ship.ActualWidth) Canvas.SetLeft(ship, nextX2);
                    break;
            }
        }

        private void Windows_KeyDown(object sender, KeyEventArgs e) {
            switch (e.Key) {
                case Key.Left: shipState = "movingLeft"; break;
                case Key.Right: shipState = "movingRight"; break;
            }
        }

        private void Windows_KeyUp(object sender, KeyEventArgs e) {
            shipState = "stopped";
        }
    }
}
