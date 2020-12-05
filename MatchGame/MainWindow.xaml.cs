using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace MatchGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;
        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if(matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text = timeTextBlock.Text + " - Play again?";
            }
        }

        private void SetUpGame()
        {
            List<string> animalEmoji = new List<string>()
            {
                "😀","😀",
                "🤗","🤗",
                "😎","😎",
                "😛","😛",
                "🙄","🙄",
                "🥰","🥰",
                "🤔","🤔",
                "🤐","🤐"
            };

            Random random = new Random();

            foreach (Emoji.Wpf.TextBlock textBlock in mainGrid.Children.OfType<Emoji.Wpf.TextBlock>())
            {
                if (textBlock.Name != "timeTextBlock")
                {
                    int index = random.Next(animalEmoji.Count);
                    string nextEmoji = animalEmoji[index];
                    textBlock.Tag = nextEmoji;
                    animalEmoji.RemoveAt(index);
                }
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        Emoji.Wpf.TextBlock lastTextBlockClicked;
        bool findingMatch = false;
        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Emoji.Wpf.TextBlock textBlock = sender as Emoji.Wpf.TextBlock;

            if(findingMatch == false)
            {
                textBlock.Text = Convert.ToString(textBlock.Tag);
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }
            else if(textBlock.Tag == lastTextBlockClicked.Tag)
            {
                matchesFound++;
                textBlock.Text = Convert.ToString(textBlock.Tag);
                lastTextBlockClicked.Background = new SolidColorBrush(Colors.Green);
                textBlock.Background = new SolidColorBrush(Colors.Green);
                findingMatch = false;
            }
            else
            {
                lastTextBlockClicked.Text = "?";
                findingMatch = false;
            }
        }

        private void timeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }
    }
}
