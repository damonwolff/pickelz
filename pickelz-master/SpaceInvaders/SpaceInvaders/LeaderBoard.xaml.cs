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
            
            string leadname;
            StreamReader file = File.OpenText("highscores");
            leadname = file.ReadLine();
            while (leadname != null)
            {
                highScoreTxtBox.Text += $"{leadname} \n";
                leadname = file.ReadLine();
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
