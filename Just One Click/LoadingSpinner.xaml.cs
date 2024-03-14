using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Just_One_Click
{
    public partial class LoadingSpinner : UserControl
    {
        private readonly Storyboard _storyboard;

        public LoadingSpinner()
        {
            InitializeComponent();
            _storyboard = FindResource("SpinnerStoryboard") as Storyboard;
        }

        public void StartSpin()
        {
            _storyboard.Begin();
        }

        public void StopSpin()
        {
            _storyboard.Stop();
        }
    }
}
