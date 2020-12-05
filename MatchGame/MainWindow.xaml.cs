using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
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
            if (matchesFound == 8)
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
            SolidColorBrush matchedBrush = new SolidColorBrush(Colors.Green);

            // Only proceed if this TextBlock hasn't already been matched.
            if (textBlock.Background == null)
            {
                if (findingMatch == false)
                {
                    // If this is the first emoji of the pair
                    textBlock.Text = Convert.ToString(textBlock.Tag);
                    lastTextBlockClicked = textBlock;
                    findingMatch = true;
                }
                else if (textBlock.Tag == lastTextBlockClicked.Tag &&
                        textBlock.Name != lastTextBlockClicked.Name)
                {
                    // If the second emoji matches the first emoji
                    matchesFound++;
                    textBlock.Text = Convert.ToString(textBlock.Tag);
                    lastTextBlockClicked.Background = matchedBrush;
                    textBlock.Background = matchedBrush;
                    findingMatch = false;
                }
                else
                {
                    // If the second emoji doesn't match the first
                    lastTextBlockClicked.Text = "?";
                    findingMatch = false;
                }
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
