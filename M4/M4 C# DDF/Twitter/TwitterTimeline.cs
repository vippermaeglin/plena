
namespace M4
{
  using System.Windows.Forms;

    public partial class TwitterTimeline : UserControl
    {
        public TwitterTimeline(string timeStamp, string text)
        {
            InitializeComponent();            
            DateLabel.Text = timeStamp;
            TextLabel.Text = text;
        }
    }
}
