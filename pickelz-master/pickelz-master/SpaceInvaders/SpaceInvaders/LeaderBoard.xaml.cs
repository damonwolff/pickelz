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
using System.Windows.Shapes;
using System.IO;

namespace SpaceInvaders
{
    /// <summary>
    /// Interaction logic for LeaderBoard.xaml
    /// </summary>
    public partial class LeaderBoard : Window
    {
        public LeaderBoard()
        {
            InitializeComponent();
            showLeaders();
        }

        void showLeaders()
        {
            int count = 0;
            string leadname;
            int leadno;
            StreamReader file = new StreamReader("highscore");
            string[] high = new string[5];
            while ((leadname = file.ReadLine()) != null)
            {
                highScoreTxtBox.Text = leadname;
                
            }
            file.Close();


            

            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            StartScreen open = new StartScreen();
            open.Show();
            this.Close();
        }
    }
}
